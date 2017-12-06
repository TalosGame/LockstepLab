using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNetWork.Net;

public class TestAddress : MonoBehaviour, NetEventListener 
{
	void Start () 
	{
		//SocketBase sbase = new SocketBase();
		//Debug.Log("Address Family:" + sbase.GetAddressFamily ("app.laiwanhb.com"));

		SocketManager socketMgr = new SocketManager();
		socketMgr.SetEventListener (this);

// 		socketMgr.CreateSocket (TNetType.Tcp);
//         socketMgr.Connect("127.0.0.1", 1000);

		socketMgr.CreateSocket (TNetType.Tcp);
        socketMgr.Connect("127.0.0.1", 1000);

		int i = 0;
		i++;

		UDebug.logEnable = true;
		UDebug.Log (LogType.warning, "test error log");
	}

	public void ReciveMessage (NetIPEndPoint peer, byte[] a)
	{
		
	}
}
