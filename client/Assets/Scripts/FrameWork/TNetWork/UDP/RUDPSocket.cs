//
// Class:   SRUSocket.cs
// Date:    2017/12/9 12:06
// Author:  Miller
// Email:   wangquan <wangquancomi@gmail.com>
// QQ:      408310416
// Desc:
//
//
// Copyright (c) 2017 - 2018
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using TG.ThreadX;

namespace TG.Net
{
    public class RUDPSocket : SocketBase
    {
        private const string UPDATE_THREAD_NAME = "LogicThread";
        private const string RECEIVE_THREAD_NAME = "ReceiveThread";
        private const int DefaultUpdateTime = 15;

        private readonly int mtu = 0;

        private readonly NetPacket[] sendingPackets;
        private Queue<NetPacket> pendingPackages = new Queue<NetPacket> ();
        private ushort fragmentId;
        private int localSeqence;
        private int curWindowSize;
        private int localWindowStart;
        private ThreadEx logicThread;

        private readonly NetPacket[] receivedPackets;
        private EndPoint receiveEndPoint;
        private NetIPEndPoint remotePoint;
        private ByteBuffer receiveBuffer = new ByteBuffer();
        private ThreadEx receiveThread;

        private SpinLock spinLock = new SpinLock (10);

        public RUDPSocket(SocketManager socketMgr, OnMessageReceived messageReceived) 
            : base(socketMgr, messageReceived){

            this.mtu = NetConst.MAX_UDP_MTU;
            this.sendingPackets = new NetPacket[NetConst.SLIDING_WINDOW];
            this.receivedPackets = new NetPacket[NetConst.SLIDING_WINDOW];
        }

        protected override void OnClose (){
            if (logicThread != null && logicThread.IsRunning) {
                logicThread.Stop ();
            }

            if (receiveThread != null && receiveThread.IsRunning) {
                receiveThread.Stop ();
            }
        }

        protected override void OnConnect (){
            this.socket = new Socket (IPEndPoint.GetAddressFamily(), SocketType.Dgram, ProtocolType.Udp);
            this.NetType = TNetType.ROUDP;

            MLPoolManager.Instance.CreatePool<MLObjectPool<UDPNetPacket>, UDPNetPacket>(preloadAmount: 50, isLimit: false);

            this.receiveBuffer.Alloc (NetConst.RECEIVE_BUFFER_SIZE);
            receiveEndPoint = new IPEndPoint(socket.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, 0);
            this.remotePoint = new NetIPEndPoint((IPEndPoint)receiveEndPoint);

            this.logicThread = new ThreadEx(UPDATE_THREAD_NAME, DefaultUpdateTime, UpdateLogic);
            this.logicThread.Start();

            this.receiveThread = new ThreadEx (RECEIVE_THREAD_NAME, -1, UpdateReceive);
            this.receiveThread.Start ();
        }

        #region send func logic
        public override void SendTo (byte[] bytes){
            UDPNetPacket packet = null;
            int headSize = NetPacket.GetHeadSize(PacketProperty.ReliableOrdered);
            int bodySize = bytes.Length;

            // split packet if size > udp mtu
            if (headSize + bodySize > mtu) {

                int fullPacketSize = mtu - headSize;
                int bodyDataSize = fullPacketSize - NetConst.FRAGMENT_HEAD_SIZE;
                int packetProbNum = bodySize / bodyDataSize;
                int leftPacketSize = bodySize % bodyDataSize;
                int totalPacketNum = packetProbNum + (leftPacketSize == 0 ? 0 : 1);

                if (totalPacketNum > ushort.MaxValue)
                {
                    throw new Exception("Too many fragments: " + totalPacketNum + " > " + ushort.MaxValue);
                }

                for(int i = 0; i < packetProbNum; i++){
                    packet = GetNetPacket (TNetType.RUUDP, PacketProperty.ReliableOrdered, fullPacketSize) as UDPNetPacket;
                    packet.IsFragment = true;
                    packet.FragmentId = fragmentId;
                    packet.FragmentPart = (ushort)i;
                    packet.FragmentsTotal = (ushort)totalPacketNum;

                    packet.WriteBytes(bytes, i * bodyDataSize, bodyDataSize);

                    DoPendingPacket (packet);
                }

                if (leftPacketSize > 0) {
                    packet = GetNetPacket (TNetType.RUUDP, PacketProperty.ReliableOrdered, 
                        leftPacketSize + NetConst.FRAGMENT_HEAD_SIZE) as UDPNetPacket;
                    packet.IsFragment = true;
                    packet.FragmentId = fragmentId;
                    packet.FragmentPart = (ushort)packetProbNum;
                    packet.FragmentsTotal = (ushort)totalPacketNum;

                    packet.WriteBytes(bytes, packetProbNum * bodyDataSize, leftPacketSize);

                    DoPendingPacket (packet);
                }

                fragmentId++;
                fragmentId %= ushort.MaxValue;
                return;
            }

            packet = GetNetPacket (TNetType.RUUDP, PacketProperty.ReliableOrdered, bytes) as UDPNetPacket;
            DoPendingPacket (packet);
        }

        private void DoSendPacket(UDPNetPacket packet){

            int result = -1;
            try{
                result = socket.SendTo(packet.RawData, 0, packet.Length, SocketFlags.None, IPEndPoint.EndPoint);
                Debug.LogFormat("[S]Send packet to {0}, result: {1}", IPEndPoint.EndPoint, result);

            }catch(SocketException ex){
                Debug.LogErrorFormat("[S]" + ex);
            }

            if (result != 0 && result != (int)SocketError.MessageSize && result != (int)SocketError.HostUnreachable)
            {
                CreateEvent (NetEventType.Error, result);
            }

            if (result == (int)SocketError.MessageSize)
            {
                Debug.LogFormat("[SRD] 10040, datalen: {0}", packet.Length);
            }
        }

        private void DoPendingPacket(UDPNetPacket packet){
            spinLock.Enter ();
            pendingPackages.Enqueue (packet);
            spinLock.Exit ();
        }

        private void UpdateLogic(){
            ProcessSendingMsg ();
        }

        private void ProcessSendingMsg(){
            spinLock.Enter ();

            if (curWindowSize >= NetConst.SLIDING_WINDOW || pendingPackages.Count <= 0) {
                spinLock.Exit ();
                return;
            }

            ProcessPendingMsg ();

            for (int i = 0; i < curWindowSize; i++) {
                UDPNetPacket packet = sendingPackets [localWindowStart % NetConst.SLIDING_WINDOW] as UDPNetPacket;
                DoSendPacket (packet);
                localWindowStart++;
            }

            spinLock.Exit ();

            // TODO start check resend logic timer
        }

        private void ProcessPendingMsg(){
            while (pendingPackages.Count > 0) {
                UDPNetPacket packet = pendingPackages.Dequeue () as UDPNetPacket;
                packet.Sequence = (ushort)localSeqence;

                sendingPackets [localSeqence % NetConst.SLIDING_WINDOW] = packet;

                localSeqence = (localSeqence + 1) % NetConst.MaxSequence;
                curWindowSize++;
            }
        }

        private void UpdateSendPingMsg(){

        }

        private bool IsRecentPacket(ushort preSeq, ushort curSeq){
            const ushort halfVal = ushort.MaxValue / 2;
            if (preSeq < curSeq && curSeq - preSeq <= halfVal
                || preSeq > curSeq && preSeq - curSeq > halfVal) {
                return true;
            }

            return false;
        }
        #endregion

        #region receive func logic
        private void UpdateReceive (){
            if (socket == null) {
                return;
            }

            int result = -1;
            try{
                receiveBuffer.Clear();

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint senderRemote = (EndPoint)sender;

                result = socket.ReceiveFrom(receiveBuffer.Buffers, 0, receiveBuffer.Length, SocketFlags.None, ref receiveEndPoint);
                if (!remotePoint.EndPoint.Equals(receiveEndPoint)){
                    remotePoint.EndPoint = receiveEndPoint as IPEndPoint;
                }

                NetPacket packet = GetNetPacket (TNetType.RUUDP, receiveBuffer.Buffers, 0, result);
                messageReceived(packet, 0, remotePoint);
            }catch (SocketException ex){
                if (ex.SocketErrorCode == SocketError.ConnectionReset 
                    || ex.SocketErrorCode == SocketError.MessageSize){
                    //10040 - message too long
                    //10054 - remote close (not error)
                    //Just UDP
                    Debug.LogFormat("[R] Ingored error: {0} - {1}", (int)ex.SocketErrorCode, ex.ToString());
                    return;
                }

                Debug.LogErrorFormat("[R]Error code: {0} - {1}", (int)ex.SocketErrorCode, ex.ToString());
                messageReceived(null, (int)ex.SocketErrorCode, remotePoint);
            }
        }

        public override void ProcessPacket(NetPacket packet)
        {
            
        }
        #endregion
    }
}

