using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

// 		byte[] bytes = new byte[]{ 1, 2, 3, 4, 5 };

		//			{ 256, 32 },
		//			{ 512, 64 },
		//			{ 1024, 8 },
		//			{ 4096, 2 }

		List<BufferConfig> configs = new List<BufferConfig> ();
		configs.Add (new BufferConfig (256, 32));
		configs.Add (new BufferConfig (512, 64));
		configs.Add (new BufferConfig (1024, 8));
		configs.Add (new BufferConfig (4096, 2));
		BufferPool.Instance.CreateSegments (configs);

		ByteBuffer byteBuf = new ByteBuffer(100);
		byteBuf.WriteInt(500);
		byteBuf.WriteShort(500);
		byteBuf.WriteByte(1);

		byte []bytes = byteBuf.Buffers;
		byteBuf.Dispose ();

        //int ret = byteBuf.ReadInt();
        //byte []bytes = byteBuf.ToBytes();
        int i = 0;
        i++;

// 		int j = 0;
// 		j++;
// 
//         int maxArrayLength = 1024 * 1024;
//         const int MinimumArrayLength = 0x10, MaximumArrayLength = 0x40000000;
//         if (maxArrayLength > MaximumArrayLength)
//         {
//             maxArrayLength = MaximumArrayLength;
//         }
//         else if (maxArrayLength < MinimumArrayLength)
//         {
//             maxArrayLength = MinimumArrayLength;
//         }
// 
//         int ret = SelectBucketIndex(maxArrayLength);
//         Debug.Log("bucket index==" + ret);


		//var segments = new ArraySegment<byte>[2];
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

    internal static int SelectBucketIndex(int bufferSize)
    {
        Debug.LogAssertion(bufferSize > 0);

        uint bitsRemaining = ((uint)bufferSize - 1) >> 4;

        int poolIndex = 0;
        if (bitsRemaining > 0xFFFF) { bitsRemaining >>= 16; poolIndex = 16; }
        if (bitsRemaining > 0xFF) { bitsRemaining >>= 8; poolIndex += 8; }
        if (bitsRemaining > 0xF) { bitsRemaining >>= 4; poolIndex += 4; }
        if (bitsRemaining > 0x3) { bitsRemaining >>= 2; poolIndex += 2; }
        if (bitsRemaining > 0x1) { bitsRemaining >>= 1; poolIndex += 1; }

        return poolIndex + (int)bitsRemaining;
    }
}
