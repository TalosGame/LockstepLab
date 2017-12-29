//
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

namespace TG.Net
{
	#region net delegate
	public delegate void OnMessageReceived(byte[] data, int length, int errorCode, NetIPEndPoint remoteEndPoint);

	delegate void OnConncect();

	delegate void CallBack();
	#endregion

	public enum TNetType
	{
		Unknown = 0,

		/// <summary>
		/// The tcp.
		/// </summary>
		TCP,

		/// <summary>
		/// The Unreliable UDP. Packets can be dropped, duplicated or arrive without order.
		/// </summary>
		UDP,

		/// <summary>
		/// The Reliable & Unorder UDP. All packets will be sent and received, but without order.
		/// </summary>
		RUUDP,

		/// <summary>
		/// Unreliable UDP. Packets can be dropped, but never duplicated and arrive in order.
		/// </summary>
		SUDP,

		/// <summary>
		/// The Reliable and ordered UDP. All packets will be sent and received in order.
		/// </summary>
		ROUDP,
	}

	public enum NetEventType
	{
		Connect = 0,
		DisConnect,
		ReceiveMessage,
		Error,
	}

    public enum UHeadDataType : byte
    {
        Data = 0,
        Ack,
    }

	public sealed class NetConst
	{
		public const string POOL_NET_EVENT = "NetEvent";
		public const string POOL_NET_PACKAGE = "NetPackage";

		// 这里可以根据需求自己调整
		public const int SLIDING_WINDOW = 64;

		// 最合适的mtu大小，避免分片
//		public static readonly int[] POSSIBLE_MTU_SIZE =
//		{
//			1452, // tcp
//			512,  // udp
//		};

        #region udp const
		// TODO udp最大mtu大小 后续根据测试调整
		public const int MAX_UDP_MTU = 512;

        public const int HEAD_SIZE = 1;
		public const int SEQUENCE_HEAD_SIZE = HEAD_SIZE + 2;
		public const int FRAGMENT_HEAD_SIZE = SEQUENCE_HEAD_SIZE + 6;



        #endregion


    }
}

