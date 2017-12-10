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
using System.Text;
using UnityEngine;

namespace TG.Net
{
	public class SocketManager
	{
        private SocketBase socket;

		private MLPoolManager poolMgr;
		private NetEventListener listener;

		private Queue<NetEvent> eventsQueue = new Queue<NetEvent>();

		public void Init(NetEventListener listener){
			InitPools ();

			this.listener = listener;
		}

		private void InitPools(){
			poolMgr = MLPoolManager.Instance;
			poolMgr.CreatePool<MLObjectPool<NetEvent>, NetEvent>(preloadAmount:50, isLimit:false);
			poolMgr.CreatePool<MLObjectPool<NetPackage>, NetPackage>(preloadAmount:50, isLimit:false);
		}

		public void CreateSocket(TNetType type){
			switch (type) {
			case TNetType.TCP:
				socket = new TCPSocket(this, listener);
				break;
			case TNetType.UDP:
				socket = new UDPSocket(this, listener);
				break;
			case TNetType.ROUDP:
				socket = new RUDPSocket (this, listener);
				break;
            default:
                Debug.LogError("Unknown socket type. Create error!");
                break;
			}
		}

        public void Connect(string host, int port){
            if (socket == null){
                return;
            }

            socket.Connect(host, port);
        }

		public void SendMessage(string str){
			byte[] bytes = UTF8Encoding.Default.GetBytes (str);
			socket.SendTo (bytes);
		}

		#region net event
		public void CreateEvent(NetEventType type){

			NetEvent evt = MLPoolManager.Instance.Spawn<NetEvent> (NetConst.POOL_NET_EVENT);
			evt.Type = type;

			lock (eventsQueue)
			{
				eventsQueue.Enqueue(evt);
			}
		}

		public void PollEvents(){

			while (eventsQueue.Count > 0){
				
				NetEvent evt;
				lock (eventsQueue)
				{
					evt = eventsQueue.Dequeue();
				}
				ProcessEvent(evt);
			}
		}

		private void ProcessEvent(NetEvent evt){
			switch (evt.Type) {
			case NetEventType.Connect:
				
				break;
			}
		}
		#endregion
	}
}

