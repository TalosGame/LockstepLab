//
// Class:	SocketBase.cs
// Date:	2017/11/8 22:35
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
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TG.ThreadX;

namespace TG.Net
{
	public abstract class SocketBase
	{
		protected Socket socket;
		protected OnMessageReceived messageReceived;

		protected NetIPEndPoint _ipEndPoint;
        public NetIPEndPoint IPEndPoint{
            get { return _ipEndPoint; }
        }

		public TNetType NetType {
			get{
                return _ipEndPoint.NetType;
			}

			set{
                _ipEndPoint.NetType = value;
			}
		}

		private SocketManager socketMgr;

		public SocketBase(SocketManager socketMgr, OnMessageReceived messageReceived) {
			this.socketMgr = socketMgr;
			this.messageReceived = messageReceived;
		}

		public void Connect (string host, int port){
            _ipEndPoint = new NetIPEndPoint(host, port);

			OnConnect ();

			CreateEvent (NetEventType.Connect);
		}

		protected virtual void OnConnect(){}

		public void Close(){
			OnClose ();

			if (socket != null) {
				socket.Close ();
				socket = null;
			}
		}

		protected virtual void OnClose(){}

		protected NetPacket GetNetPacket(TNetType type, PacketProperty property, int size){
            NetPacket packet = CreatePacket(type);
			if (packet == null) {
				throw new NullReferenceException ("packet");
			}

			size += NetPacket.GetHeadSize(property);
			packet.Init (property, size);
			return packet;
		}

        protected NetPacket GetNetPacket(TNetType type, byte[] datas, int offset, int size){
            NetPacket packet = CreatePacket(type);
            if (packet == null) {
                throw new NullReferenceException ("packet");
            }

            packet.Init(datas, 0, size);
            return packet;
        }

        protected NetPacket GetNetPacket(TNetType type, PacketProperty property, byte[] datas){
            NetPacket packet = CreatePacket(type);
            if (packet == null) {
                throw new NullReferenceException ("packet");
            }

            packet.Init(property, datas);
            return packet;
        }

        private NetPacket CreatePacket(TNetType type){
            NetPacket packet = null;
            MLPoolManager poolMgr = MLPoolManager.Instance;
            // TODO need update here.
            switch (type) {
                case TNetType.TCP:

                    break;
                default:
                    // udp packet
                    packet = poolMgr.Spawn<UDPNetPacket>(NetConst.POOL_UDP_NET_PACKET);
                    break;
            }

            return packet;
        }

		protected void CreateEvent(NetEventType type){
            socketMgr.CreateEvent (type);
		}

		protected void CreateEvent(NetEventType type, int errCode){
            socketMgr.CreateEvent (type, errCode);
		}

		protected void CreateEvent(NetEventType type, byte[] datas){
            socketMgr.CreateEvent (type, datas);
		}

		public abstract void SendTo (byte[] bytes);

        public virtual void ProcessPacket(NetPacket packet){
            
        }
	}
}
