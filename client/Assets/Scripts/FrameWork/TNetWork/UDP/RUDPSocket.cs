//
// Class:	SRUSocket.cs
// Date:	2017/12/9 12:06
// Author: 	Miller
// Email:	wangquan <wangquancomi@gmail.com>
// QQ:		408310416
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
using System.Net.Sockets;
using UnityEngine;

namespace TG.Net
{
	public class RUDPSocket : SocketBase
	{
		private readonly int mtu = 0;

		private NetPacket[] sendingPackets;
		private Queue<NetPacket> pengdingPackages = new Queue<NetPacket> ();

		private int localSeqence;
		private int curWindowSize;
		private int localWindowStart;

		private readonly NetPacket[] receivedPackets;

		private ByteBuffer byteBuffer;

		public RUDPSocket(SocketManager socketMgr, OnMessageReceived messageReceived) 
			: base(socketMgr, messageReceived){

			this.NetType = TNetType.ROUDP;
			this.mtu = NetConst.MAX_UDP_MTU;
			this.sendingPackets = new NetPacket[NetConst.SLIDING_WINDOW];
			this.receivedPackets = new NetPacket[NetConst.SLIDING_WINDOW];
		}

		protected override void OnConnect (){
			this.socket = new Socket (IPEndPoint.GetAddressFamily(), SocketType.Dgram, ProtocolType.Udp);
		}

		public override void SendTo (byte[] bytes){
			if(NetConst.SEQUENCE_HEAD_SIZE + bytes.Length > NetConst.MAX_UDP_MTU){
				
			}else{
				
			}
		}

		protected override void ReceiveMessage ()
		{
			
		}

		private bool IsRecentPacket(ushort preSeq, ushort curSeq){
			const ushort halfVal = ushort.MaxValue / 2;
			if (preSeq < curSeq && curSeq - preSeq <= halfVal
				|| preSeq > curSeq && preSeq - curSeq > halfVal) {
				return true;
			}

			return false;
		}
	}
}

