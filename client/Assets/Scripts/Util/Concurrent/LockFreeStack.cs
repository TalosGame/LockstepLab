//
// Class:	LockFreeStack.cs
// Date:	12/14/2017 2:47:29 PM
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
using System.Threading;

public class LockFreeStack<T>
{
    public LinkNode<T> head;

    public int count;

    public LockFreeStack()
    {
        head = new LinkNode<T>();
		head.item = default(T);
    }

    public void Push(T item)
    {
        LinkNode<T> newNode = new LinkNode<T>();
        newNode.item = item;

        do{
            newNode.next = head.next;
        } while (!SyncMethods.CAS<LinkNode<T>>(ref head.next, newNode.next, newNode));

        Interlocked.Increment(ref count);
    }

    public T Pop()
    {
        LinkNode<T> node;
        T ret = default(T);

        do{
            node = head.next;
            if (node == null){
                return ret;
            }

        } while (!SyncMethods.CAS<LinkNode<T>>(ref head.next, node, node.next));

        Interlocked.Decrement(ref count);
        return node.item;
    }
}

