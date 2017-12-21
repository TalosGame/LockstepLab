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
    public partial class ByteBuffer : IDisposable {

        public int ReadInt() {
            int size = sizeof(int);

            IntPtr nPtr = Marshal.AllocHGlobal(size);

//             int readNum = 0;
//             do
//             {
//             
//             }

            Marshal.FreeHGlobal(nPtr);

//             Position pos = CountReadPos();
//             ArraySegment<byte> curSegment = buffers[pos.index];
//             int ret = BitConverter.ToInt32(curSegment.Array, pos.offset);
            return 0;
        }

        public short ReadShort() { 
            return 0;
        }

        public byte ReadByte() {
            return 0;
        }

        public byte[] ReadBytes(int size) {



            return null;
        }
    }
}

