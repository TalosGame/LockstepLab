using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Threading;
using TG.Net;
using TG.ThreadX;

public class TestAddress : MonoBehaviour, NetEventListener 
{
	private ThreadEx thread;

    LockFreeQueue<int> queue;
    LockFreeStack<int> stack;
    private int topValue = 10000;

    object _mtuMutex = new object();

    Queue<int> queue1 = new Queue<int>();

    Stopwatch sw;

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
        socketMgr.SendMessage ("Hello netty 2222!");

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

//        queue = new LockFreeQueue<int>();
//        stack = new LockFreeStack<int>();

//         queue.Enqueue(1);
//         queue.Enqueue(2);
// 
//         int num = queue.Dequeue();
//         num = queue.Dequeue();
//         num = queue.Dequeue();

//        queue.Enqueue(1);

//        sw = Stopwatch.StartNew();
//
//        Thread reporter = new Thread(new ThreadStart(EnqueueExecute));
//        reporter.IsBackground = true;
//        reporter.Start();
//
//        Thread reporter1 = new Thread(new ThreadStart(DequeueExecute));
//        reporter1.IsBackground = true;
//        reporter1.Start();

//         Thread reporter = new Thread(new ThreadStart(EnqueueExecute1));
//         reporter.IsBackground = true;
//         reporter.Start();
// 
//         Thread reporter1 = new Thread(new ThreadStart(DequeueExecute1));
//         reporter1.IsBackground = true;
//         reporter1.Start();

//		UDebug.logEnable = true;
//		UDebug.Log (LogType.warning, "test error log");
	}

    void EnqueueExecute()
    {
        for (int i = 0; i < topValue; i++)
        {
            queue.Enqueue(i);
        }

        UnityEngine.Debug.Log("EnqueueExecute done!");
    }

    void DequeueExecute()
    {
        bool[] poppedValues = new bool[topValue];
        int poppedInt;

        do
        {
            poppedInt = queue.Dequeue();
            if (poppedInt != 0)
            {
                if (poppedValues[poppedInt])
                    UnityEngine.Debug.LogFormat("{0} has been popped before!", poppedInt);

                poppedValues[poppedInt] = true;
            }
        } while (poppedInt != (topValue - 1));

        UnityEngine.Debug.Log("DequeueExecute done! count:" + queue.count + " time:" + sw.Elapsed.Milliseconds);
    }

    void EnqueueExecute1()
    {
        for (int i = 0; i < topValue; i++)
        {
            lock(_mtuMutex)
            {
                queue1.Enqueue(i);
            }
        }

        UnityEngine.Debug.Log("EnqueueExecute done!");
    }

    void DequeueExecute1()
    {
        bool[] poppedValues = new bool[topValue];
        int poppedInt;

        do
        {
            lock(_mtuMutex)
            {
                poppedInt = queue1.Dequeue();

                if (poppedInt != 0)
                {
                    if (poppedValues[poppedInt])
                        UnityEngine.Debug.LogFormat("{0} has been popped before!", poppedInt);

                    poppedValues[poppedInt] = true;
                }
            }

        } while (poppedInt != (topValue - 1));

        UnityEngine.Debug.Log("DequeueExecute done! count:" + queue.count + " time:" + sw.Elapsed.Milliseconds);
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
        UnityEngine.Debug.Log("ssss" + num);

		num++;
		if (num >= 10) {
			thread.Stop ();
		}
	}
}
