using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TG.Net;
using System.Threading;

public class TestAddress : MonoBehaviour, NetEventListener 
{
	private ThreadEx thread;

    LockFreeQueue<int> queue;
    private int topValue = 10000;

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


// 		ConcurrentQueue<int> queue = new ConcurrentQueue<int> (3);
// 		queue.Enqueue (10);
// 		queue.Enqueue (20);
// 		queue.Enqueue (30);
// 		queue.Enqueue (40);
// 
// 		int num = queue.Dequeue ();
//         num = queue.Dequeue();
//         num = queue.Dequeue();
//         num = queue.Dequeue();
// 		Debug.Log ("num:" + num);

        queue = new LockFreeQueue<int>();
//         queue.Enqueue(1);
//         queue.Enqueue(2);
// 
//         int num = queue.Dequeue();
//         num = queue.Dequeue();
//         num = queue.Dequeue();

//        queue.Enqueue(1);

        Thread reporter = new Thread(new ThreadStart(EnqueueExecute));
        reporter.IsBackground = true;
        reporter.Start();

        Thread reporter1 = new Thread(new ThreadStart(DequeueExecute));
        reporter1.IsBackground = true;
        reporter1.Start();

		UDebug.logEnable = true;
		UDebug.Log (LogType.warning, "test error log");
	}


    void EnqueueExecute()
    {
        for (int i = 1; i <= topValue; i++)
        {
            queue.Enqueue(i);
        }

        Debug.Log("EnqueueExecute done!");
    }

    void DequeueExecute()
    {
        bool[] poppedValues = new bool[topValue + 1];
        int poppedInt;

        do
        {
            poppedInt = queue.Dequeue();
            if (poppedInt != 0)
            {
                if (poppedValues[poppedInt])
                    Debug.LogFormat("{0} has been popped before!", poppedInt);

                poppedValues[poppedInt] = true;
            }
        } while (poppedInt != topValue);

        Debug.Log("DequeueExecute done!");
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
