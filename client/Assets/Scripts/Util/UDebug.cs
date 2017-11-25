//
// Class:	UDebug.cs
// Date:	2017/11/23 18:19
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

using UnityEngine;
using System;

public enum LogType
{
	info,	 // 信息
	warning, // 警告
	error,	 // 错误
}

public sealed class UDebug
{
	private static string[] DEBUG_COLORS = new string[]{
		"FFFFFF",
		"FFFF00",
		"FF0000",
	};

	public static bool logEnable {
		get{ 
			return Debug.logger.logEnabled;
		}

		set{ 
			Debug.logger.logEnabled = value;
		}
	}

	public static void Log(LogType type, string log){

		if (!logEnable) {
			return;
		}

		string color = DEBUG_COLORS [(int)type];

		//format
		string str = "<color=#" + color + ">" +
			"<b>" + log + "</b>" 
			+ "</color>";

		Debug.Log(log);
	}
}


