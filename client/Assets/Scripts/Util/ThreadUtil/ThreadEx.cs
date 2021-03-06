﻿//
// Class:	NetThread.cs
// Date:	2017/11/13 23:33
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
using System.Threading;

namespace TG.ThreadX {
    public class ThreadEx {
        private Thread thread;

        private bool isRunning;
		public bool IsRunning {
			get { return isRunning; }
		}

        private string name;
        private int sleepTime;
        private Action callBack;

        public ThreadEx(string name, int sleepTime, Action callBack) {
            this.name = name;
            this.sleepTime = sleepTime;
            this.callBack = callBack;
        }

        public void Start() {
            if (isRunning) {
                return;
            }

            isRunning = true;

            thread = new Thread(ThreadLogic);
            thread.Name = name;
            thread.IsBackground = true;
			thread.Start();
        }

        public void Stop() {
            if (!isRunning) {
                return;
            }

            thread.Join();
            isRunning = false;
        }

        private void ThreadLogic() {
            while (isRunning) {
                callBack();

				if(sleepTime > 0)
                	Thread.Sleep(sleepTime);
            }
        }
    }
}