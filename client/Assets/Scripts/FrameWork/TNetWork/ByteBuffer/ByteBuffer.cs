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
using System.Collections.Generic;

namespace TG.Net
{
	public class ByteBuffer : IDisposable{

		struct Position{
			public readonly int index;
			public readonly int offset;

			public Position(int index, int offset){
				this.index = index;
				this.offset = offset;
			}
		}

		private readonly BufferPool pool;
		private readonly int bufferSize;

		private List<ArraySegment<byte>> buffers;
		private int lenght;
		private bool dispose;

		public ByteBuffer(BufferPool pool){
			this.pool = pool;
		}

		public ByteBuffer(int bufferCount, BufferPool pool){

			if (bufferCount <= 0) {
				throw new ArgumentException ("bufferCount");
			}

			if(pool == null){
				throw new ArgumentException ("pool");
			}

			this.buffers = new List<ArraySegment<byte>> (pool.ObtainSegments (bufferCount));
			this.bufferSize = buffers [0].Count;
			this.pool = pool;
			this.lenght = 0;
			this.dispose = false;
		}

		public void Dispose(){
			pool.RecycleSegments (buffers);
			dispose = true;
			buffers = null;
		}

		public void WriteInt(int val){
			byte[] bytes = BitConverter.GetBytes (val);
			WriteBytes (bytes);
		}

		public void WriteShort(short val){
			
		}

		public void WriteByte(byte val){
			
		}

		public void WriteBytes(byte[] bytes){
			WriteBytes (bytes, 0, bytes.Length);
		}

		public void WriteBytes(byte[] bytes, int offset, int count){
			if (bytes == null) 
				throw new ArgumentNullException("data");
			if (offset < 0 || offset > bytes.Length) 
				throw new ArgumentOutOfRangeException("offset");
			if (count < 0 || count + offset > bytes.Length) 
				throw new ArgumentOutOfRangeException("count");
			
			Write(new ArraySegment<byte>(bytes, offset, count));
		}

		private void Write(ArraySegment<byte> segment){

			int writeNum = 0;
			do{
				Position pos = CountPostion (lenght);
				CheckBuffer(pos.index);

				ArraySegment<byte> curSegment = buffers[pos.index];

				int canWrite = segment.Count - writeNum;
				int leftBytes = curSegment.Count - curSegment.Offset;
				canWrite = canWrite > leftBytes ? leftBytes : canWrite;

				Buffer.BlockCopy(segment.Array, segment.Offset + writeNum, curSegment.Array, curSegment.Offset + pos.offset, canWrite);

				writeNum += canWrite;
				lenght += canWrite;
			}while(writeNum < segment.Count);
		}

		public void ReadInt(){
			
		}

		public void ReadShort(){
			
		}

		public void ReadByte(){
			
		}

		public void ReadBytes(){
			
		}

		private Position CountPostion(int position){
			Position ret = new Position (position / bufferSize, position % bufferSize);
			return ret;
		}

		private void CheckBuffer(int index){
			if (index <= buffers.Count) {
				return;
			}

			int count = index - buffers.Count;
			ArraySegment<byte>[] segments = pool.ObtainSegments (count);
			for (int i = 0; i < segments.Length; i++) {
				buffers.Add (segments [i]);
			}
		}
	}
}

