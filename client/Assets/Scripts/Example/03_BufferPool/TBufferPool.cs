using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TG.Net;

public class TBufferPool : MonoBehaviour {

	//private LockFreeStack<ArraySegment<byte>> _buffers = new LockFreeStack<ArraySegment<byte>> (); 

	private BufferPool pool;

	void Start () {
//		byte[] bytes = new byte[1024 * 1024];
//
//		for (int i = 0; i < 1; i++){
//			var chunk = new ArraySegment<byte>(bytes, i * 1024, 1024);
//			_buffers.Push(chunk);
//		}
//
//		int cnt = _buffers.count;
//		Debug.Log ("buffer cnt:" + cnt);
//
//		ArraySegment<byte> segment = _buffers.Pop ();
//		segment = _buffers.Pop ();
//
//		if (segment == default(ArraySegment<byte>)) {
//			int i = 0;
//			i++;
//		}

//		var result = GetBuffs ();
//		foreach (ArraySegment<byte> buf in segment) {
//			Debug.Log ("len:" + buf.Array.Length + " offset:" + buf.Offset + " count:" + buf.Count);
//		}

		byte[] bytes = new byte[]{ 1, 2, 3, 4, 5 };

		using(ByteBuffer byteBuf = new ByteBuffer(1, BufferPool.DefaultPool)){
			byteBuf.WriteBytes (bytes);	
		}

		int j = 0;
		j++;

		//var segments = new ArraySegment<byte>[2];
		//segments
	}

//	private IEnumerable<ArraySegment<byte>> GetBuffs()
//	{
//		ArraySegment<byte> chunks = _buffers.Pop();
//		//Debug.Log ("count:" + chunks.Count);
//
//		var result = new ArraySegment<byte>[1];
//		result [0] = chunks;
//
//		return result;
//	}
}
