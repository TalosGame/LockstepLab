//
// MLObjectPool.cs
//
// Author:
//       wangquan <wangquancomi@gmail.com>
//       QQ: 408310416
// Desc:
//      1.通用对象池
//      2.为了兼顾所有自定义的对象，集合类里定义是object.
//        从池里拿和取对象有装箱和拆箱操作
//
// Copyright (c) 2017 
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

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MLObjectPool<T> : MLPoolBase<T> where T : class
{
	public override void Init (T poolItem, int preloadAmount = 10, Transform parent = null, bool isLimit = true)
	{
		Type t = typeof(T);
		this.itemName = t.Name;

		this.preloadAmount = preloadAmount;
		this.limitAmount = this.preloadAmount << 1;
		this.limitInstances = isLimit;
	}

	public override void CreatePoolItems()
	{
		for (int i = 0; i < preloadAmount; i++)
		{
			T item = SpawnNewItem();
			if (item == null)
				break;

			freeObjects.Push(item);
		}
    }

	public override T SpawnNewItem()
    {
		T item = Activator.CreateInstance<T>();
		if (item == null)
		{
            Debug.LogError("Create instance error!");
			return default(T);
		}

        return item;
    }

	public override bool Despawn(T item)
    {
		if (!usedObjects.Contains(item))
		{
			return false;
		}

		usedObjects.Remove(item);

		// recycle used object
        freeObjects.Push(item);
		return true;
    }
}
