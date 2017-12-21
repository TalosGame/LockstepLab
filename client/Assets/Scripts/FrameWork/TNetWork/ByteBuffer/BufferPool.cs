//
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
using TG.ThreadX;

namespace TG.Net {

	public struct BufferConfig
	{
		public int chunkSize;
		public int capacity;

		public BufferConfig(int chunkSize, int capacity){
			this.chunkSize = chunkSize;
			this.capacity = capacity;
		}
	}

	public class BufferPool : SingletonBase<BufferPool> {

		private List<BufferSegment> segments = new List<BufferSegment> ();
		private List<int> chunkSizes = new List<int> ();

		class BufferSegment{
			public byte[][] buffers;

			public readonly int chunkSize;
			public readonly int capacity;

			private int chunkNum;
			private SpinLock spinLock = new SpinLock (10);

			public BufferSegment(int capacity, int chunkSize){
				this.capacity = capacity;
				this.chunkSize = chunkSize;
				this.chunkNum = capacity;

				this.buffers = new byte[capacity][];
				for (int i = 0; i < capacity; i++) {
					this.buffers[i] = new byte[chunkSize];
				}
			}

			public byte[] GetBuffer(){
				if (chunkNum < 0) {
					return new byte[chunkSize];
				}

				spinLock.Enter ();
				var p = buffers[--chunkNum];
				buffers[chunkNum] = null;
				spinLock.Exit ();
				return p;
			}

			public void ReturnBuffer(byte []bytes){

				if (chunkNum >= capacity) return;

				spinLock.Enter ();
				buffers[chunkNum++] = bytes;
				spinLock.Exit ();
			}
		}

		public void CreateSegments(List<BufferConfig> configs){
			for (int i = 0; i < configs.Count; i++) {
				BufferConfig config = configs [i];
				CreateSegment (config.capacity, config.chunkSize);
			}
		}

		public void CreateSegment(int chunkSize, int capacity){
			if (chunkSizes.Contains (chunkSize)) {
				return;
			}

			chunkSizes.Add (chunkSize);

			BufferSegment segment = new BufferSegment (chunkSize, capacity);
			segments.Add (segment);
		}

		public byte[] GetBuffer(int size){
			for(int i = 0; i < segments.Count; i++){
				var segment = segments[i];
				if(size <= segment.chunkSize){
					return segment.GetBuffer ();
				}
			}

			// buffer cache miss, keep program running 
			return new byte[size];
		}

		public void ReturnBuffer(byte[] bytes){
			int size = bytes.Length;
			for (int i = 0; i < segments.Count; ++i){
				var segment = segments[i];
				if (size == segment.chunkSize){
					segment.ReturnBuffer (bytes);
					break;
				}
			}
		}
    }
}


