using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TG.Net;

public class ByteBufferEditor : MonoBehaviour {

	[MenuItem("TG/Make Buffer config")]
	public static void BuildBufferConfig()
	{
		BufferConfig config = new BufferConfig ();
		config.AddBuffer (new BufferInfo (256, 32));
		config.AddBuffer (new BufferInfo (512, 64));
		config.AddBuffer (new BufferInfo (1024, 8));
		config.AddBuffer (new BufferInfo (4096, 2));
		string json = JsonUtility.ToJson (config);
		Debug.Log (json);

		byte[] bytes = System.Text.Encoding.UTF8.GetBytes (json);
		MLFileUtil.SaveFile (Application.dataPath + "/Meta/Config", "ByteBuffer.json", bytes);

		AssetDatabase.Refresh ();
	}
}
