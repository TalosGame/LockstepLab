//
// Class:	BufferReader.cs
// Date:	12/20/2017 5:27:09 PM
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
using System.Runtime.InteropServices;

namespace TG.Net {
    public partial class ByteBuffer {

        public int AvailableBytes {
            get { return chunkSize - ReadIndex; }
        }

        public int ReadInt() {
            int ret = BitConverter.ToInt32(buffer, ReadIndex);
            ReadIndex += 4;
            return ret;
        }

        public short ReadShort() {
            short ret = BitConverter.ToInt16(buffer, ReadIndex);
            ReadIndex += 2;
            return ret;
        }

		public ushort ReadUShort() {
			ushort ret = BitConverter.ToUInt16(buffer, ReadIndex);
			ReadIndex += 2;
			return ret;
		}

        public byte ReadByte() {
            return buffer[ReadIndex++];
        }

        public byte[] ReadBytes() {
            byte[] ret = new byte[AvailableBytes];
            Buffer.BlockCopy(buffer, ReadIndex, ret, 0, AvailableBytes);
            ReadIndex = chunkSize;
            return ret;
        }

        public byte[] ReadBytes(int size){
            if (size < 0 || size > AvailableBytes)
                throw new ArgumentOutOfRangeException("size");

            byte[] ret = new byte[size];
            Buffer.BlockCopy(buffer, ReadIndex, ret, 0, size);
            ReadIndex += size;
            return ret;
        }

        public void GetRemainingBytes(byte[] destination) {
            Buffer.BlockCopy(buffer, ReadIndex, destination, 0, AvailableBytes);
            ReadIndex = chunkSize;
        }

        public void GetBytes(byte[] destination, int lenght) {
            Buffer.BlockCopy(buffer, ReadIndex, destination, 0, lenght);
            ReadIndex += lenght;
        }
    }
}

