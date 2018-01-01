using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TG.Net;

public class TBufferPool : MonoBehaviour {

	Dictionary<int, string> dic;

	void Start () {
//		TextAsset asset = Resources.Load<TextAsset>("Meta/Config/ByteBuffer");
//		string json = asset.text;
//
//		BufferConfig config = JsonUtility.FromJson<BufferConfig> (json);
//		BufferPool.Instance.CreateSegments (config);
//
//		ByteBuffer byteBuf = new ByteBuffer(100);
//		byteBuf.WriteInt(500);
//		byteBuf.WriteShort(500);
//		byteBuf.WriteByte(1);
//
//        var ret = byteBuf.ReadInt();
//        ret = byteBuf.ReadShort();
//        ret = byteBuf.ReadByte();
//
//        byteBuf.Dispose ();

//		byte[] data = new byte[2];
//		for(int i = 1; i < data.Length; i++){
//			data[i] = 0xF;
//		}
//
//		short sequence = BitConverter.ToInt16 (data, 1);

//		ushort head = 0xffff;
//		head = (ushort)(head & 0x7fff);
//		int j = 0;
//		j++;

//		UDPNetPacket packet = new UDPNetPacket (20);
//		packet.IsFragment = true;
//		packet.Property = PacketProperty.AckReliable;
//		packet.Sequence = 1000;
//
//		bool IsFragment = packet.IsFragment;
//		PacketProperty property = packet.Property;
//		int sequence = packet.Sequence;

		ByteBuffer buffer = new ByteBuffer ();
		buffer.Alloc (128);

		buffer.WriteUShort (65535);

		var ret = buffer.ReadUShort ();

		int i = 0;
		i++;
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
