//
// Class:	NetEndPoint.cs
// Date:	2017/11/13 17:10
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
using System.Net;
using System.Net.Sockets;

namespace TNetWork.Net
{
	public sealed class NetIPEndPoint
	{
		private IPEndPoint endPoint;
		public IPEndPoint EndPoint
		{
			set{
				endPoint = value;
			}

			get{
				return endPoint;
			}
		}

		public string Host 
		{
			get{
				return EndPoint.Address.ToString ();
			}
		}

		public int Port
		{
			get{
				return EndPoint.Port;
			}
		}

		private TNetType netType = TNetType.Unknown;
		public TNetType NetType
		{
			set{
				netType = value;
			}

			get{ 
				return netType;
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is NetIPEndPoint))
			{
				return false;
			}

			NetIPEndPoint option = obj as NetIPEndPoint;
			if (NetType != option.NetType) {
				return false;
			}

			if (!EndPoint.Equals (option.EndPoint)) {
				return false;
			}

			return true;
		}

		public override string ToString()
		{
			return EndPoint.ToString();
		}

		public override int GetHashCode()
		{
			return EndPoint.GetHashCode();
		}

		public void SetIPEndPoint(string ip, int port)
		{
			IPAddress ipAdress = null;
			bool isValidIP = IPAddress.TryParse(ip, out ipAdress);
			if (isValidIP)
			{
				EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
				return;
			}

			IPHostEntry hostEntry = Dns.GetHostEntry(ip);
			EndPoint = new IPEndPoint(hostEntry.AddressList[0], port);
		}

		public AddressFamily GetAddressFamily()
		{
			if (EndPoint == null) {
				return AddressFamily.Unknown;
			}

			IPAddress[] address = Dns.GetHostAddresses(Host);
			if (address [0].AddressFamily == AddressFamily.InterNetworkV6) 
			{
				return AddressFamily.InterNetworkV6;
			}

			if (address [0].AddressFamily == AddressFamily.InterNetwork) 
			{
				return AddressFamily.InterNetwork;
			}

			return AddressFamily.Unknown;
		}
	}
}

