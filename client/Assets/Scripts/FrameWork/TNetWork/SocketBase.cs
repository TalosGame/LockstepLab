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
using TG.ThreadX;

namespace TG.Net
{
	public abstract class SocketBase
	{
		protected Socket socket;

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

		public SocketBase(SocketManager socketMgr, NetEventListener listener) {
			this.socketMgr = socketMgr;
		}

		public void Connect (string host, int port){
            _ipEndPoint.SetIPEndPoint(host, port);
			Connect ();

			CreateEvent (NetEventType.Connect);

//			netThread = new ThreadEx ("Process update logic", DefaultUpdateTime, ProcessLogic);
//			netThread.Start ();
		}

		protected virtual void Connect(){}

		protected void CreateEvent(NetEventType type){
			socketMgr.CreateEvent (type);
		}

		private void ProcessLogic(){
			OnProcessLogic ();
		}

		protected virtual void OnProcessLogic(){}

		public abstract void SendTo (byte[] bytes);
	}
}
