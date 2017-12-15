using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Debug = UnityEngine.Debug;

public class TLockFreeStack : MonoBehaviour {

    private const int topValue = 1000;

    private LockFreeStack<int> stack = new LockFreeStack<int>();
    private Stopwatch sw;

    private bool[] poppedValues = new bool[topValue];
	
	void Start () {
        sw = Stopwatch.StartNew();

        Thread reporter = new Thread(new ThreadStart(PushExecute));
        reporter.IsBackground = true;
        reporter.Start();

        Thread reporter1 = new Thread(new ThreadStart(PopExecute));
        reporter1.IsBackground = true;
        reporter1.Start();
	}

    void PushExecute()
    {
        for (int i = 0; i < topValue; i++)
        {
            stack.Push(i + 1);
        }

        Debug.Log("EnqueueExecute done!");
    }

    void PopExecute()
    {
        int poppedInt;

        while(true){
            poppedInt = stack.Pop();
            if (poppedInt != 0)
            {
                if (poppedValues[poppedInt - 1])
                    Debug.LogFormat("{0} has been popped before!", poppedInt);

                poppedValues[poppedInt - 1] = true;
            }

            if (IsOver())
                break;
        };

        Debug.Log("DequeueExecute done! count:" + stack.count + " time:" + sw.Elapsed.Milliseconds + "ms");
    }

    private bool IsOver()
    {
        for (int i = 0; i < topValue; i++)
        {
            if (!poppedValues[i])
            {
                return false;
            }
        }

        return true;
    }
	
}
