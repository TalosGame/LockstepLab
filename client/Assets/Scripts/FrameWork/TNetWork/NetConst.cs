﻿//
// Class:	NetConst.cs
// Date:	2017/11/8 22:52
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

namespace TNetWork.Net
{
	// delegate
	delegate void OnMessageReceived(byte[] data, int length, int errorCode, NetIPEndPoint remoteEndPoint);

	public enum TNetType
	{
		Unknown = 0,
		Tcp,
		Udp,
	}

	public enum NetEventType
	{
		ConnectSucess = 0,
		ConnectFailure,
	}

    public enum UHeadDataType : byte
    {
        Data = 0,
        Ack,
    }

    public enum UDPOptions
    {

        ReliableOrdered,
    }

	public sealed class NetConst
	{
		public const string POOL_NET_EVENT = "pool_net_event";

        #region udp const
        // 这里可以根据需求自己调整
        public static int SLIDING_WINDOW = 64;



        #endregion


    }
}

