//
// Class:	NetPackage.cs
// Date:	12/6/2017 11:33:50 AM
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
	public enum PacketProperty : byte
	{
		Unreliable,             //0
		Reliable,               //1
		Sequenced,              //2
		ReliableOrdered,        //3
		AckReliable,            //4
		AckReliableOrdered,     //5
		Ping,                   //6
		Pong,                   //7
		ConnectRequest,         //8
		ConnectAccept,          //9
		Disconnect,             //10
		UnconnectedMessage,     //11
		NatIntroductionRequest, //12
		NatIntroduction,        //13
		NatPunchMessage,        //14
		MtuCheck,               //15
		MtuOk,                  //16
		DiscoveryRequest,       //17
		DiscoveryResponse,      //18
		Merged                  //19
	}

	public abstract class NetPacket
    {
        public byte[] body;

		public virtual int GetHeadSize(){
			return 0;
		}
    }

	public class UDPNetPacket : NetPacket
	{
		/// <summary>
		/// 0 Fragment & Property
		/// 1 data or ack Sequence Number
		/// 2 
		/// </summary>
		private byte[] head;

		public UDPNetPacket(int size){
			head = new byte[size];
		}

		public bool IsFragment
		{
			get{ 
				return (head [0] & 0x80) != 0;
			}

			set{ 
				if (value) {
					head [0] |= 0x80;
					return;
				}

				head [0] &= 0x7F;
			}
		}

		public PacketProperty Property
		{
			get { return (PacketProperty)(head[0] & 0x7F); }
			set {
				head [0] = (byte)((head [0] & 0x80) | ((byte)value & 0x7F));
			}
		}

		public ushort Sequence
		{
			get { return (ushort)(BitConverter.ToUInt16(head, 1)); }
			set {
				GetBytes (head, 1, value);
			}
		}

		public ushort FragmentId
		{
			get { return BitConverter.ToUInt16(head, 3); }
			set { GetBytes(head, 3, value); }
		}

		public ushort FragmentPart
		{
			get { return BitConverter.ToUInt16(head, 5); }
			set { GetBytes(head, 5, value); }
		}

		public ushort FragmentsTotal
		{
			get { return BitConverter.ToUInt16(head, 7); }
			set { GetBytes(head, 7, value); }
		}

		public static void WriteLittleEndian(byte[] buffer, int offset, short data)
		{
#if BIGENDIAN
			buffer[offset + 1] = (byte)(data);
			buffer[offset    ] = (byte)(data >> 8);
#else
			buffer[offset] = (byte)(data);
			buffer[offset + 1] = (byte)(data >> 8);
#endif
		}

		public static void GetBytes(byte[] bytes, int startIndex, short value)
		{
			WriteLittleEndian(bytes, startIndex, value);
		}

		public static void GetBytes(byte[] bytes, int startIndex, ushort value)
		{
			WriteLittleEndian(bytes, startIndex, (short)value);
		}
	}

}



