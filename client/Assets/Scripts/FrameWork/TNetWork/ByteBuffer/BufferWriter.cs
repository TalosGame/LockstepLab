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
	public partial class ByteBuffer {

        public unsafe void WriteInt(int val) {
            byte* ptrVal = (byte*)&val;
            fixed (byte* ptrDst = buffer) {
#if BIGENDIAN
			    *(ptrDst + WriteIndex + 3) = *ptrVal;
			    *(ptrDst + WriteIndex + 2) = *(ptrVal + 1);
			    *(ptrDst + WriteIndex + 1) = *(ptrVal + 2);
			    *(ptrDst + writeIndex) = *(ptrVal + 3);
#else
                *(ptrDst + WriteIndex) = *ptrVal;
                *(ptrDst + WriteIndex + 1) = *(ptrVal + 1);
                *(ptrDst + WriteIndex + 2) = *(ptrVal + 2);
                *(ptrDst + WriteIndex + 3) = *(ptrVal + 3);
#endif
            }

			WriteIndex += 4;
        }

        public unsafe void WriteShort(short val) {
            byte* ptrVal = (byte*)&val;
            fixed (byte* ptrDst = buffer) {
#if BIGENDIAN
			    *(ptrDst + WriteIndex + 1) = *ptrVal;
			    *(ptrDst + WriteIndex) = *(ptrVal + 1);
#else
                *(ptrDst + WriteIndex) = *ptrVal;
                *(ptrDst + WriteIndex + 1) = *(ptrVal + 1);
#endif
            }
			
			WriteIndex += 2;
        }

		public unsafe void WriteShort(int offset, short val){
			byte* ptrVal = (byte*)&val;
			WriteIndex = offset;

			fixed (byte* ptrDst = buffer) {
#if BIGENDIAN
				*(ptrDst + WriteIndex + 1) = *ptrVal;
				*(ptrDst + WriteIndex) = *(ptrVal + 1);
#else
				*(ptrDst + WriteIndex) = *ptrVal;
				*(ptrDst + WriteIndex + 1) = *(ptrVal + 1);
#endif
			}

			WriteIndex += 2;
		}

		public unsafe void WriteUShort(ushort val){
			byte* ptrVal = (byte*)&val;

			fixed (byte* ptrDst = buffer) {
#if BIGENDIAN
				*(ptrDst + WriteIndex + 1) = *ptrVal;
				*(ptrDst + WriteIndex) = *(ptrVal + 1);
#else
				*(ptrDst + WriteIndex) = *ptrVal;
				*(ptrDst + WriteIndex + 1) = *(ptrVal + 1);
#endif
			}

			WriteIndex += 2;
		}

		public unsafe void WriteUShort(int offset, ushort val){
			byte* ptrVal = (byte*)&val;
			WriteIndex = offset;

			fixed (byte* ptrDst = buffer) {
#if BIGENDIAN
				*(ptrDst + WriteIndex + 1) = *ptrVal;
				*(ptrDst + WriteIndex) = *(ptrVal + 1);
#else
				*(ptrDst + WriteIndex) = *ptrVal;
				*(ptrDst + WriteIndex + 1) = *(ptrVal + 1);
#endif
			}

			WriteIndex += 2;
		}

        public unsafe void WriteByte(byte val) {
            byte* ptrVal = (byte*)&val;
            fixed (byte* ptrDst = buffer) {
                *(ptrDst + WriteIndex) = *ptrVal;
            }
			WriteIndex++;
        }

        public void WriteBytes(int offset, byte[] bytes){
            WriteIndex = offset;
            WriteBytes(bytes, 0, bytes.Length);
        }

		public void WriteBytes(byte[] bytes) {
            WriteBytes(bytes, 0, bytes.Length);
        }

        public void WriteBytes(byte[] bytes, int offset, int size) {
            if (bytes == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > bytes.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (size < 0 || size + offset > bytes.Length)
                throw new ArgumentOutOfRangeException("size");

            Buffer.BlockCopy(bytes, offset, buffer, WriteIndex, size);
            WriteIndex += size;
        }
    }
}

