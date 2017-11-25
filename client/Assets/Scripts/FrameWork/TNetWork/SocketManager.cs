//
// Class:	SocketManager.cs
// Date:	2017/11/8 22:33
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
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace TNetWork.Net
{
	public class SocketManager : SingletonBase<SocketManager>
	{
		private Dictionary<NetIPEndPoint, SocketBase> sockets = new Dictionary<NetIPEndPoint, SocketBase>();

		private NetEventListener listener;

		private MLPoolManager poolMgr;

		protected override void Init ()
		{
			InitPools ();
		}

		private void InitPools(){
			poolMgr = MLPoolManager.Instance;
			poolMgr.CreatePool<MLObjectPool<NetEvent>, NetEvent>(preloadAmount:10);
		}

		public void SetEventListener(NetEventListener listener){
			this.listener = listener;
		}

		public SocketBase CreateSocket(TNetType type){
			SocketBase socket = null;
			switch (type) {
			case TNetType.Tcp:
				socket = new TCPSocket(this, listener);
				break;
			case TNetType.Udp:
				socket = new UDPSocket(this, listener);
				break;
			}

			return socket;
		}

		public bool AddSocket(SocketBase socket)
		{
			SocketBase cacheSockt = null;
			NetIPEndPoint option = socket.IPEndPoint;

			if (sockets.TryGetValue (option, out cacheSockt)) {
				Debug.LogError ("Have Same Socket connect address! type:" + cacheSockt.NetType);
				return true;
			}

			sockets.Add (option, socket);
			return false;
		}

		public void PollEvents()
		{

		}

		private void CreateEvent()
		{

		}





	}
}

