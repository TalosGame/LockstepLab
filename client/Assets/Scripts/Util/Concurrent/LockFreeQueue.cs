//
// Class:	LockFreeQueue.cs
// Date:	12/14/2017 2:48:02 PM
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

public class LockFreeQueue<T>
{
    public LinkNode<T> head = null;
    public LinkNode<T> tail;

    public int count;

    public LockFreeQueue()
    {
        head = new LinkNode<T>();
        tail = head;
    }

    public void Enqueue(T item){
        LinkNode<T> oldTail = null;
        LinkNode<T> oldTailNext = null;

        LinkNode<T> newNode = new LinkNode<T>();
        newNode.item = item;

        bool newNodeWasAdded = false;
        while (!newNodeWasAdded) {
            oldTail = tail;
            oldTailNext = oldTail.next;

            if (tail == oldTail)
            {
                if (oldTailNext == null)
                {
                    newNodeWasAdded = SyncMethods.CAS<LinkNode<T>>(ref tail.next, null, newNode);
                }
                else
                {
                    SyncMethods.CAS<LinkNode<T>>(ref tail, oldTail, oldTailNext);
                }
            }
        }

        SyncMethods.CAS<LinkNode<T>>(ref tail, oldTail, newNode);
        Interlocked.Increment(ref count);
    }

    public T Dequeue()
    {
        T ret = default(T);

        LinkNode<T> oldHead = null;
        LinkNode<T> oldTail = null;
        LinkNode<T> oldHeadNext = null;

        bool haveAdvancedHead = false;
        while (!haveAdvancedHead)
        {
            oldHead = head;
            oldTail = tail;
            oldHeadNext = oldHead.next;

            if (oldHead == head)
            {
                if (oldHead == oldTail)
                {
                    if(oldHeadNext == null)
                    {
                        return ret;
                    }

                    SyncMethods.CAS<LinkNode<T>>(ref tail, oldTail, oldHeadNext);
                }
                else
                {
                    ret = oldHeadNext.item;
                    haveAdvancedHead = SyncMethods.CAS<LinkNode<T>>(ref head, oldHead, oldHeadNext);

                    Interlocked.Decrement(ref count);
                }
            }
        }

        return ret;
    }

}

