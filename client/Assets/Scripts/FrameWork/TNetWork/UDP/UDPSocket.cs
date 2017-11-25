﻿//
// Class:	UDPSocket.cs
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

using UnityEngine;
using System;
using System.Net.Sockets;

namespace TNetWork.Net
{
	public class UDPSocket : SocketBase
	{
		public UDPSocket(SocketManager socketMgr, NetEventListener listener) : base(socketMgr, listener){
			NetType = TNetType.Udp;
		}

		protected override void Connect (){
			this.socket = new Socket (IPEndPoint.GetAddressFamily(), SocketType.Dgram, ProtocolType.Udp);
		}

		public override void SendTo (byte[] bytes, NetIPEndPoint remoteEndPoint){
			try{
				
				int ret = this.socket.SendTo (bytes, remoteEndPoint.EndPoint);
				Debug.Log("Send packet to {0}, result: {1}", remoteEndPoint.EndPoint, ret);

			}catch(SocketException ex){



			}
		}
	}
}

