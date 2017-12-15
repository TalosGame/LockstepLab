using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Debug = UnityEngine.Debug;

public class TLockFreeQueue : MonoBehaviour {

    private const int topValue = 1000;

    private LockFreeQueue<int> queue = new LockFreeQueue<int>();
    private Stopwatch sw;

    private bool []poppedValues = new bool[topValue];

	void Start () {
        sw = Stopwatch.StartNew();

        Thread reporter = new Thread(new ThreadStart(EnqueueExecute));
        reporter.IsBackground = true;
        reporter.Start();

        Thread reporter1 = new Thread(new ThreadStart(DequeueExecute));
        reporter1.IsBackground = true;
        reporter1.Start();
	}

    void EnqueueExecute(){

        for (int i = 0; i < topValue; i++){
            queue.Enqueue(i + 1);
        }

        Debug.Log("EnqueueExecute done!");
    }

    void DequeueExecute(){

        int poppedInt;

        while(true){
            poppedInt = queue.Dequeue();
            if (poppedInt != 0){
                if (poppedValues[poppedInt - 1])
                    Debug.LogFormat("{0} has been popped before!", poppedInt);

                poppedValues[poppedInt - 1] = true;
            }

            if (IsOver())
                break;
        }

        Debug.Log("DequeueExecute done! count:" + queue.count + " time:" + sw.Elapsed.Milliseconds + "ms");
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
