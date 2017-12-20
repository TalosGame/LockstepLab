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
    public unsafe partial class ByteBuffer : IDisposable {

        public void WriteInt(int val) {
            byte* ptrVal = (byte*)&val;
            Write(ptrVal, sizeof(int));
        }

        public void WriteShort(short val) {
            byte* ptrVal = (byte*)&val;
            Write(ptrVal, sizeof(short));
        }

        public void WriteByte(byte val) {
            byte* ptrVal = (byte*)&val;
            Write(ptrVal, sizeof(byte));
        }

        public void WriteBytes(byte[] bytes) {
            WriteBytes(bytes, 0, bytes.Length);
        }

        public void WriteBytes(byte[] bytes, int offset, int count) {
            if (bytes == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > bytes.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0 || count + offset > bytes.Length)
                throw new ArgumentOutOfRangeException("count");

            Write(new ArraySegment<byte>(bytes, offset, count));
        }

        private void Write(byte* srcPtr, int size) {
            CheckBuffer(size);

            int writeNum = 0;
            do {
                Position pos = CountWritePos();
                ArraySegment<byte> curSegment = buffers[pos.index];

                int canWrite = size - writeNum;
                int leftBytes = curSegment.Count - pos.offset;
                canWrite = canWrite > leftBytes ? leftBytes : canWrite;

                fixed (byte* ptrDst = curSegment.Array) {
                    int idx = 0;
                    while (idx < canWrite) {
                        int dstOffset = curSegment.Offset + pos.offset;
                        int ptrOffset = writeNum + idx;
                        *(ptrDst + dstOffset + ptrOffset) = *(srcPtr + ptrOffset);
                        idx++;
                    }
                }

                writeNum += canWrite;
                writeIndex += canWrite;

            } while (writeNum < size);
        }

        private void Write(ArraySegment<byte> segment) {
            CheckBuffer(segment.Count);

            int writeNum = 0;
            do {
                Position pos = CountWritePos();
                ArraySegment<byte> curSegment = buffers[pos.index];

                int canWrite = segment.Count - writeNum;
                int leftBytes = curSegment.Count - pos.offset;
                canWrite = canWrite > leftBytes ? leftBytes : canWrite;

                Buffer.BlockCopy(segment.Array, segment.Offset + writeNum, curSegment.Array, curSegment.Offset + pos.offset, canWrite);

                writeNum += canWrite;
                writeIndex += canWrite;
            } while (writeNum < segment.Count);
        }

        /// <summary>
        /// TODO: Can make less gc alloc here?
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            byte[] ret = new byte[writeIndex];
            Position pos = CountWritePos();

            int offset = 0;
            int writeNum = writeIndex;
            for (int i = 0; i < pos.index + 1; i++) {
                ArraySegment<byte> curSegment = buffers[i];

                int canWrite = writeNum < curSegment.Count ? writeNum : curSegment.Count;
                Buffer.BlockCopy(curSegment.Array, curSegment.Offset, ret, offset, canWrite);

                // left write bytes
                writeNum -= canWrite;
                offset += canWrite;
            }

            return ret;
        }
    }
}

