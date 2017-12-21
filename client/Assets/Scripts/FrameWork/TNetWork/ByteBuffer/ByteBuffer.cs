//
// Class:	ByteBuffer.cs
// Date:	12/20/2017 5:16:23 PM
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
    public partial class ByteBuffer {
		private readonly int chunkSize;

		private byte[] buffer;
		public byte[] Buffers{
			get{
				return buffer;
			}
		}

        private int writeIndex;
        private int readIndex;
        private bool dispose;

		public ByteBuffer(int size){

			if (size <= 0) {
				throw new ArgumentException("size");
			}

			this.chunkSize = size;
			this.writeIndex = 0;
			this.readIndex = 0;
			this.dispose = false;

			this.buffer = BufferPool.Instance.GetBuffer (size);
		}

//        public ByteBuffer(byte[] bytes) {
//            if (bytes == null || bytes.Length <= 0) {
//                throw new ArgumentException("bytes");
//            }
//
//            if (pool == null) {
//                throw new ArgumentException("pool");
//            }
//
//            // Create first default buffer
//            this.buffers = new List<ArraySegment<byte>>(pool.ObtainSegments(1));
//            this.bufferSize = buffers[0].Count;
//            this.pool = pool;
//            this.readIndex = 0;
//            this.dispose = false;
//
//            CheckBuffer(bytes.Length);
//
//            WriteBytes(bytes);
//        }

        public void Dispose() {
			BufferPool.Instance.ReturnBuffer(buffer);
            dispose = true;
			buffer = null;
        }

		public int GetWriteIdx(){
			return writeIndex % chunkSize;
		}

		public int GetReadIdx(){
			return readIndex % chunkSize;
		}

		private void CheckBuffer() {
			if (writeIndex >= chunkSize) {
				throw new Exception("Write buffer exception!");
			}

			if (readIndex > writeIndex) {
				throw new Exception("Read buffer exception!");
			}
        }
    }
}

