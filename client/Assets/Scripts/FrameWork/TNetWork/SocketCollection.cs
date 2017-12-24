//
// Class:	SocketCollection.cs
// Date:	2017/12/23 18:34
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

namespace TG.Net
{
	public class SocketCollection
	{
		private Dictionary<NetIPEndPoint, SocketBase> socketDic;
		private List<SocketBase> socketArrays;
		private int count;

		public SocketCollection(){
			socketDic = new Dictionary<NetIPEndPoint, SocketBase> ();
			socketArrays = new List<SocketBase> ();
			count = 0;
		}

		public void Clear(){
			socketDic.Clear ();
			socketArrays.Clear ();
		}

		public bool TryGetValue(NetIPEndPoint endPoint, out SocketBase peer){
			return socketDic.TryGetValue(endPoint, out peer);
		}

		public void Add(SocketBase socket){
//			socketArrays.Add(socket);
//			socketDic.Add(endPoint, socket);
//			count++;
		}

		public bool ContainsAddress(NetIPEndPoint endPoint){
			SocketBase ret = null;
			if (socketDic.TryGetValue (endPoint, out ret)) {
				return true;
			}

			return false;
		}

		public void RemoveAt(int idx){
			NetIPEndPoint endPoint = socketArrays [idx].IPEndPoint;
			socketDic.Remove(endPoint);

			socketArrays[idx] = socketArrays[count - 1];
			socketArrays[count - 1] = null;
			count--;
		}

		public void Remove(NetIPEndPoint endPoint){
			for (int i = 0; i < count; i++){
				NetIPEndPoint ipEndPoint = socketArrays [i].IPEndPoint;
				if (ipEndPoint.Equals(endPoint)){
					RemoveAt(i);
					break;
				}
			}
		}
	}
}

