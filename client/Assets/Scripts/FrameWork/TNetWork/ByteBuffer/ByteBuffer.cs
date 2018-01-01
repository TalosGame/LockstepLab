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
		private int chunkSize;

		private byte[] buffer;
		public byte[] Buffers{
			get{
				return buffer;
			}
		}

        private int writeIndex;
        public int WriteIndex {
            get { return writeIndex; }
            set { writeIndex = value; }
        }

        private int readIndex;
        public int ReadIndex {
            get { return readIndex; }
            set { readIndex = value; }
        }

		public int Length{
			get{
				if (buffer == null) {
					return 0;
				}

				return buffer.Length;
			}
		}

        private bool dispose;

		public void Alloc(int size){
			if (size <= 0) {
				throw new ArgumentException("size");
			}

			this.WriteIndex = 0;
			this.ReadIndex = 0;
			this.dispose = false;

			this.buffer = BufferPool.Instance.GetBuffer (size);
			this.chunkSize = this.buffer.Length;
		}

		public void Copy(byte[] bytes){
			if (bytes == null || bytes.Length <= 0) {
				throw new ArgumentException("bytes");
			}

			this.ReadIndex = 0;
			this.dispose = false;

			this.buffer = BufferPool.Instance.GetBuffer(bytes.Length);
			this.chunkSize = this.buffer.Length;

			WriteBytes(bytes);
		}

        public void Copy(byte[] bytes, int offset, int size){
            if (bytes == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > bytes.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (size < 0 || size + offset > bytes.Length)
                throw new ArgumentOutOfRangeException("size");

            this.ReadIndex = 0;
            this.dispose = false;

            this.buffer = BufferPool.Instance.GetBuffer(size);
            this.chunkSize = this.buffer.Length;

            WriteBytes(bytes, offset, size);
        }

        public void Clear(){
            this.WriteIndex = 0;
            this.ReadIndex = 0;
        }

        public void Release() {
			BufferPool.Instance.ReturnBuffer(buffer);
            dispose = true;
			buffer = null;
        }

		private void CheckBuffer() {
			if (WriteIndex >= chunkSize) {
				throw new Exception("Write buffer exception!");
			}

			if (ReadIndex > WriteIndex) {
				throw new Exception("Read buffer exception!");
			}
        }
    }
}

