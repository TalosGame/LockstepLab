﻿//
// Class:	BufferPool.cs
// Date:	12/20/2017 7:55:36 PM
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

    public class BufferPool1 {

        //class 
        private int _chunkNum;
        public int ChunkNum {
            get { return _chunkNum; }
            set { _chunkNum = value; }
        }

        private readonly int chunkSize;
        public byte[][] buffers;

        public BufferPool1(int chunkNum, int chunkSize)
        {
            this._chunkNum = chunkNum;
            this.chunkSize = chunkSize;
            this.buffers = new byte[chunkNum][];

            for (int i = 0; i < chunkNum; i++) {
                this.buffers[i] = new byte[chunkSize];
            }
        }

        public byte[] GetBuffer(int size) {
            for (int i = 0; i < buffers.Length; i++) { 
                
            }

            return null;
        }

        public void ReturnBuffer(byte[] bytes) {
            
        }
    }
}


