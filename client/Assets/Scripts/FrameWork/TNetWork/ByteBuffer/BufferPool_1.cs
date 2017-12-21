//
// Class:	NetDataWriter.cs
// Date:	2017/12/10 14:36
// Author: 	Miller
// Email:	wangquan <wangquancomi@gmail.com>
// QQ:		408310416
// Desc:
//
// 模拟Netty ByteBuf
// 0 <= readerIndex <= writerIndex <= capacity。
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
using System.Threading;
/*
namespace TG.Net
{
	public class BufferPool
	{
		private static BufferPool _defaultPool;

		private readonly int chunkNum;
		private readonly int chunkSize;
		private readonly int segmentSize;
		private readonly bool autoAlloc;

		private LockFreeStack<ArraySegment<byte>> buffers = new LockFreeStack<ArraySegment<byte>> ();

		public static BufferPool DefaultPool{
			get{
				if(_defaultPool == null)
					_defaultPool = new BufferPool (1, 1024, 1);
				
				return _defaultPool;
			}
		}

		public BufferPool(int chunkNum, int chunkSize, int allocNum)
			: this(chunkNum, chunkSize, 1, true){
		}

		public BufferPool(int chunkNum, int chunkSize, int initNum, bool autoAlloc){
			if (chunkNum <= 0) {
				throw new ArgumentException ("segmentSize");
			}

			if (chunkSize <= 0) {
				throw new ArgumentException ("chunkSize");
			}

			if (initNum < 0) {
				throw new ArgumentException ("size");
			}

			this.chunkNum = chunkNum;
			this.chunkSize = chunkSize;
			this.segmentSize = this.chunkNum * this.chunkSize;

			this.autoAlloc = true;
			Thread.MemoryBarrier ();

			for (int i = 0; i < initNum; i++) {
				CreateSegment (true);
			}

			this.autoAlloc = autoAlloc;
		}

		private void CreateSegment(bool forceCreate){
			if (!this.autoAlloc) {
				return;
			}

			// 非强制的情况，如果数据段已小于池里总数的一半就创建
			if (!forceCreate && buffers.count > chunkNum / 2) {
				return;
			}

			byte[] bytes = new byte[segmentSize];
			for (int i = 0; i < chunkNum; i++) {
				ArraySegment<byte> buffer = new ArraySegment<byte> (bytes, i * chunkSize, chunkSize);
				buffers.Push (buffer);
			}
		}

		public ArraySegment<byte> ObtainSegment(){
			ArraySegment<byte> ret = buffers.Pop ();
			if (ret != default(ArraySegment<byte>)) {
				return ret;
			}

			CreateSegment (false);
			return buffers.Pop ();
		}

		public ArraySegment<byte>[] ObtainSegments(int num){
			var ret = new ArraySegment<byte>[num];
			int count = 0;

			while (count < num) {
				ArraySegment<byte> segment = ObtainSegment ();
				if (segment == default(ArraySegment<byte>)) {
					break;
				}
				
				ret [count++] = segment;
			}

			if (count == num) {
				return ret;
			}

			RecycleSegments (ret);
			return ret;
		}

		public void RecycleSegment(ArraySegment<byte> segment){
			CheckBuffer (segment);
			buffers.Push (segment);
		}

		public void RecycleSegments(ArraySegment<byte> []segments){
			if (segments == null) {
				throw new Exception("Attempt to checking invalid buffer");
			}

			for(int i = 0; i < segments.Length; i++){
				ArraySegment<byte> segment = segments[i];
				if (segment == default(ArraySegment<byte>)) {
					continue;
				}

				RecycleSegment (segment);
			}
		}

		public void RecycleSegments(List<ArraySegment<byte>> segments){
			if (segments == null) {
				throw new Exception("Attempt to checking invalid buffer");
			}

			for(int i = 0; i < segments.Count; i++){
				ArraySegment<byte> segment = segments[i];
				if (segment == default(ArraySegment<byte>)) {
					continue;
				}

				RecycleSegment (segment);
			}
		}

		private void CheckBuffer(ArraySegment<byte> segment){
			if (segment.Array == null || segment.Count == 0 || segment.Array.Length < segment.Offset + segment.Count)
				throw new Exception("Attempt to checking invalid buffer");
			if (segment.Count != chunkSize) 
				throw new ArgumentException("Buffer was not of the same chunk size as the buffer manager", "buffer");
		}
	}
}
*/

/*
class BufferInfo
	{
		public int bufferSize;
		public int count;
		public int capacity;
		public byte[][] buffers;

		public BufferInfo(int bufferSize_, int capacity_)
		{
			count = 0;
			bufferSize = bufferSize_;
			capacity = capacity_;
			buffers = new byte[capacity][];
		}

		public byte[] Get()
		{
			lock(this)
			{
				if (count == 0) return new byte[bufferSize];
				var p = buffers[--count];
				buffers[count] = null;
				return p;
			}
		}

		public void Put(byte[] p)
		{
			lock(this)
			{
				if (count >= capacity) return;
				buffers[count++] = p;
			}
		}
	}

	static BufferInfo[] bufferInfos =
	{
		new BufferInfo(256, 32),
		new BufferInfo(1024, 8),
		new BufferInfo(4096, 2)
	};

	public static byte[] GetBuffer(int size)
	{
		int c = bufferInfos.Length;
		for(int i = 0; i < c; ++ i)
		{
			var info = bufferInfos[i];
			if(size <= info.bufferSize)
			{
				return info.Get();
			}
		}
		return new byte[size];
	}

	public static void PutBuffer(byte[] p)
	{
		int size = p.Length;
		int c = bufferInfos.Length;
		for (int i = 0; i < c; ++i)
		{
			var info = bufferInfos[i];
			if (size == info.bufferSize)
			{
				info.Put(p);
				break;
			}
		}
	}
}
 */

