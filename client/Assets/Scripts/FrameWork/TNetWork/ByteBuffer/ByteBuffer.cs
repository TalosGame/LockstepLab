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
    public unsafe partial class ByteBuffer {
        struct Position {
            public readonly int index;
            public readonly int offset;

            public Position(int index, int offset) {
                this.index = index;
                this.offset = offset;
            }
        }

        private readonly BufferPool pool;
        private readonly int bufferSize;

        private List<ArraySegment<byte>> buffers;

        private int writeIndex;
        private int readIndex;

        private bool dispose;

        public ByteBuffer(BufferPool pool) : this(1, pool) {
        }

        public ByteBuffer(int bufferCount, BufferPool pool) {
            if (bufferCount <= 0) {
                throw new ArgumentException("bufferCount");
            }

            if (pool == null) {
                throw new ArgumentException("pool");
            }

            this.buffers = new List<ArraySegment<byte>>(pool.ObtainSegments(bufferCount));
            this.bufferSize = buffers[0].Count;
            this.pool = pool;
            this.writeIndex = 0;
            this.readIndex = 0;
            this.dispose = false;
        }

        public ByteBuffer(byte[] bytes, BufferPool pool) {
            if (bytes == null || bytes.Length <= 0) {
                throw new ArgumentException("bytes");
            }

            if (pool == null) {
                throw new ArgumentException("pool");
            }

            // Create first default buffer
            this.buffers = new List<ArraySegment<byte>>(pool.ObtainSegments(1));
            this.bufferSize = buffers[0].Count;
            this.pool = pool;
            this.readIndex = 0;
            this.dispose = false;

            CheckBuffer(bytes.Length);

            WriteBytes(bytes);
        }

        public void Dispose() {
            pool.RecycleSegments(buffers);
            dispose = true;
            buffers = null;
        }

        private Position CountWritePos() {
            return CountPosition(writeIndex);
        }

        private Position CountReadPos() {
            return CountPosition(readIndex);
        }

        private Position CountPosition(int index) {
            Position ret = new Position(index / bufferSize, index % bufferSize);
            return ret;
        }

        private void CheckBuffer(int nbNum) {
            int needCnt = (writeIndex + nbNum) / bufferSize;
            if (needCnt < buffers.Count) {
                return;
            }

            int count = needCnt - buffers.Count + 1;
            if (count <= 0) {
                throw new ArgumentOutOfRangeException("Check Buffer Count!");
            }

            ArraySegment<byte>[] segments = pool.ObtainSegments(count);
            for (int i = 0; i < segments.Length; i++) {
                buffers.Add(segments[i]);
            }
        }
    }
}

