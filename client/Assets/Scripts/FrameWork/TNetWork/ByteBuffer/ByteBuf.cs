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

namespace TG.Net
{
	public class ByteBuf
	{
		private static ByteBuf _defaultBufferManager;

		public byte[] datas;
		protected int capacity;
		protected int writerIndex;
		protected int readerIndex;

		private readonly bool autoResize;

		//private 

		public ByteBuf Default{
			get{ 
				return null;
			}
		}



		public ByteBuf()
		{
			//ArraySegment

			capacity = 64;
			datas = new byte[capacity];
			autoResize = true;
		}

		public ByteBuf(bool autoResize)
		{
			capacity = 64;
			datas = new byte[capacity];
			autoResize = autoResize;
		}

		public ByteBuf(bool autoResize, int initialSize)
		{
			capacity = initialSize;
			datas = new byte[capacity];
			autoResize = autoResize;
		}

		public void ResizeIfNeed(int newSize)
		{
			if (capacity < newSize)
			{
				while (capacity < newSize)
				{
					capacity *= 2;
				}

				Array.Resize(ref datas, capacity);
			}
		}

		public void Reset(int size)
		{
			ResizeIfNeed(size);
			writerIndex = 0;
			readerIndex = 0;
		}


		public void Put(float value)
		{
//			if (autoResize)
//				ResizeIfNeed(_position + 4);
			
//			FastBitConverter.GetBytes(_data, _position, value);
//			_position += 4;
		}

		public void Put(double value)
		{
//			if (_autoResize)
//				ResizeIfNeed(_position + 8);
//			FastBitConverter.GetBytes(_data, _position, value);
//			_position += 8;
		}

		//public void SetBytes()

	}
}

