using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TG.Net;

public class TestAddress : MonoBehaviour, NetEventListener 
{
	private ThreadEx thread;

	void Start () 
	{
		//SocketBase sbase = new SocketBase();
		//Debug.Log("Address Family:" + sbase.GetAddressFamily ("app.laiwanhb.com"));

//		SocketManager socketMgr = new SocketManager();
//		socketMgr.SetEventListener (this);

// 		socketMgr.CreateSocket (TNetType.Tcp);
//         socketMgr.Connect("127.0.0.1", 1000);

//		socketMgr.CreateSocket (TNetType.TCP);
//        socketMgr.Connect("127.0.0.1", 1000);

		SocketManager socketMgr = new SocketManager();
		socketMgr.Init (this);
		socketMgr.CreateSocket (TNetType.ROUDP);
		socketMgr.Connect ("127.0.0.1", 8090);

		socketMgr.SendMessage ("Hello netty!");


		ConcurrentQueue<int> queue = new ConcurrentQueue<int> (100);
		queue.Enqueue (10);
		queue.Enqueue (20);
		queue.Enqueue (30);
		queue.Enqueue (40);

		int num = queue.Dequeue ();
		Debug.Log ("num:" + num);

		UDebug.logEnable = true;
		UDebug.Log (LogType.warning, "test error log");
	}

	public void ReciveMessage (NetIPEndPoint peer, byte[] a)
	{
		
	}

	public void OnConnect()
	{
		
	}

	private int num = 0;

	public void CallBack()
	{
		Debug.Log ("ssss" + num);

		num++;
		if (num >= 10) {
			thread.Stop ();
		}
	}
}
