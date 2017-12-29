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
		private const string RECEIVE_THREAD_NAME = "ReceiveMsgThread";

		protected Socket socket;
		protected OnMessageReceived messageReceived;

		private SocketManager socketMgr;

		private ThreadEx netThread;
		private const int DefaultUpdateTime = 15;

		protected NetIPEndPoint _ipEndPoint = new NetIPEndPoint();
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

		public SocketBase(SocketManager socketMgr, OnMessageReceived messageReceived) {
			this.socketMgr = socketMgr;
			this.messageReceived = messageReceived;
		}

        public void Init() {
            OnInit();

            netThread = new ThreadEx(RECEIVE_THREAD_NAME, -1, ReceiveMessageLogic);
            netThread.Start();
        }

        protected virtual void OnInit() { 
        
        }

		public void Connect (string host, int port){
            _ipEndPoint.SetIPEndPoint(host, port);
			OnConnect ();

			CreateEvent (NetEventType.Connect);
		}

		protected virtual void OnConnect(){}

		public void Close(){
			if (netThread.IsRunning) {
				netThread.Stop ();
			}

			if (socket != null) {
				socket.Close ();
				socket = null;
			}

			OnClose ();
		}

		protected virtual void OnClose(){}

		protected void CreateEvent(NetEventType type){
			socketMgr.CreateEvent (type);
		}

		private void ReceiveMessageLogic(){
			if (netThread == null || !netThread.IsRunning) {
				return;
			}

			ReceiveMessage ();
		}

		protected abstract void ReceiveMessage ();

		public abstract void SendTo (byte[] bytes);
	}
}
