//
// Class:	BufferPool.cs
// Date:	2017/12/10 20:25
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
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace TG.Net {
	public class ByteBuffer : IDisposable {

        public void WriteInt(int val) {
			Write(GetWriteIdx(), val);
			writeIndex += 4;
        }

        public void WriteShort(short val) {
			Write(GetWriteIdx(), val);
			writeIndex += 2;
        }

        public void WriteByte(byte val) {
			buffer[writeIndex] = val;
			writeIndex++;
        }

		public void WriteBytes(byte[] bytes) {
			Buffer.BlockCopy (bytes, 0, buffer, writeIndex, bytes.Length);
			writeIndex += bytes.Length;
        }

//        public void WriteBytes(byte[] bytes, int offset, int count) {
//            if (bytes == null)
//                throw new ArgumentNullException("data");
//            if (offset < 0 || offset > bytes.Length)
//                throw new ArgumentOutOfRangeException("offset");
//            if (count < 0 || count + offset > bytes.Length)
//                throw new ArgumentOutOfRangeException("count");
//
//            Write(new ArraySegment<byte>(bytes, offset, count));
//        }

//        private void Write(byte* srcPtr, int size) {
//            CheckBuffer(size);
//
//            int writeNum = 0;
//            do {
//                Position pos = CountWritePos();
//                ArraySegment<byte> curSegment = buffers[pos.index];
//
//                int canWrite = size - writeNum;
//                int leftBytes = curSegment.Count - pos.offset;
//                canWrite = canWrite > leftBytes ? leftBytes : canWrite;
//
//                fixed (byte* ptrDst = curSegment.Array) {
//                    int idx = 0;
//                    while (idx < canWrite) {
//                        int dstOffset = curSegment.Offset + pos.offset;
//                        int ptrOffset = writeNum + idx;
//                        *(ptrDst + dstOffset + ptrOffset) = *(srcPtr + ptrOffset);
//                        idx++;
//                    }
//                }
//
//                writeNum += canWrite;
//                writeIndex += canWrite;
//
//            } while (writeNum < size);
//        }
//
//        private void Write(ArraySegment<byte> segment) {
//            CheckBuffer(segment.Count);
//
//            int writeNum = 0;
//            do {
//                Position pos = CountWritePos();
//                ArraySegment<byte> curSegment = buffers[pos.index];
//
//                int canWrite = segment.Count - writeNum;
//                int leftBytes = curSegment.Count - pos.offset;
//                canWrite = canWrite > leftBytes ? leftBytes : canWrite;
//
//                Buffer.BlockCopy(segment.Array, segment.Offset + writeNum, curSegment.Array, curSegment.Offset + pos.offset, canWrite);
//
//                writeNum += canWrite;
//                writeIndex += canWrite;
//            } while (writeNum < segment.Count);
//        }

		private void Write(int offset, int data)
		{
#if BIGENDIAN
			buffer[offset + 3] = (byte)(data);
			buffer[offset + 2] = (byte)(data >> 8);
			buffer[offset + 1] = (byte)(data >> 16);
			buffer[offset    ] = (byte)(data >> 24);
#else
			buffer[offset] = (byte)(data);
			buffer[offset + 1] = (byte)(data >> 8);
			buffer[offset + 2] = (byte)(data >> 16);
			buffer[offset + 3] = (byte)(data >> 24);
#endif
		}

		public void Write(int offset, short data)
		{
#if BIGENDIAN
			buffer[offset + 1] = (byte)(data);
			buffer[offset    ] = (byte)(data >> 8);
			#else
			buffer[offset] = (byte)(data);
			buffer[offset + 1] = (byte)(data >> 8);
#endif
		}
    }
}

