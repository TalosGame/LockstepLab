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

namespace TG.Net {
    public enum PacketProperty : byte {
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

    public abstract class NetPacket {
		public ByteBuffer buffer = new ByteBuffer();

        public void Init(byte[] datas, int offset, int size){
            this.buffer.Copy(datas, offset, size);
        }

		public void Init(PacketProperty property, int size){
			this.buffer.Alloc(size);
			this.Property = property;
		}

        public void Init(PacketProperty property, byte[] datas){
            int headSize = GetHeadSize(property);
            this.buffer.Alloc(headSize + datas.Length);
            this.Property = property;
            this.buffer.WriteBytes(headSize, datas);
        }

        public void Close() {
			buffer.Release ();
        }

		public byte[] RawData{
			get{ 
				return buffer.Buffers;
			}
		}

		public int Length{
			get{
				return buffer.Length;
			}
		}

		public static int GetHeadSize(PacketProperty property) {
			return IsSequenced(property) ? NetConst.SEQUENCE_HEAD_SIZE : NetConst.HEAD_SIZE;
		}

		private static bool IsSequenced(PacketProperty property) {
			return property == PacketProperty.ReliableOrdered ||
				property == PacketProperty.Reliable ||
				property == PacketProperty.Sequenced ||
				property == PacketProperty.Ping ||
				property == PacketProperty.Pong ||
				property == PacketProperty.AckReliable ||
				property == PacketProperty.AckReliableOrdered;
		}

		public PacketProperty Property {
			get { return (PacketProperty)(RawData[0] & 0x3F); }
			set {
				RawData[0] = (byte)((RawData[0] & 0xC0) | ((byte)value & 0x3F));
			}
		}

        public long Token{
            get{ 
                if (!WriteToken){
                    return -1;
                }

                return BitConverter.ToInt64(RawData, 1); 
            }

            set{
                WriteToken = true;
                buffer.WriteLong(1, value);
            }
        }

        private bool WriteToken{
            get {
                return (RawData[0] & 0x40) != 0;
            }

            set {
                if (value) {
                    RawData[0] |= 0x40;
                    return;
                }

                RawData[0] &= 0xBF;
            }
        }

        protected int TokenLen{
            get{ 
                if (WriteToken){
                    return NetConst.SOCKET_TOKEN_SIZE;
                }

                return 0;
            }
        }

		public void WriteBytes(byte[] bytes, int offset, int size){
			buffer.WriteBytes (bytes, offset, size);
		}
    }

    /// <summary>
    /// 0 Fragment & Token & Property
    /// 8 or 0 Token
    /// 2 data or ack Sequence Number
    /// 2 FragmentId
    /// 2 FragmentPart
    /// 2 FragmentsTotal
    /// </summary>
    public class UDPNetPacket : NetPacket {
        public bool IsFragment {
            get {
				return (RawData[0] & 0x80) != 0;
            }

            set {
                if (value) {
					RawData[0] |= 0x80;
                    return;
                }

				RawData[0] &= 0x7F;
            }
        }

        public ushort Sequence {
            get { return (ushort)(BitConverter.ToUInt16(RawData, 1 + TokenLen)); }
            set {
                buffer.WriteUShort(1 + TokenLen, value);
            }
        }

        public ushort FragmentId {
            get { return BitConverter.ToUInt16(RawData, 3 + TokenLen); }
            set { buffer.WriteUShort(3 + TokenLen, value); }
        }

        public ushort FragmentPart {
            get { return BitConverter.ToUInt16(RawData, 5 + TokenLen); }
            set { buffer.WriteUShort(5 + TokenLen, value); }
        }

        public ushort FragmentsTotal {
            get { return BitConverter.ToUInt16(RawData, 7 + TokenLen); }
            set { buffer.WriteUShort(7 + TokenLen, value); }
        }
    }
}
