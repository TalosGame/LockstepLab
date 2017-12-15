using System;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class InterlockedTest : MonoBehaviour
{
    //记录线程数  
    private static int threadCount = 0;
    private static System.Random rnd = new System.Random();

    void Start()
    {
        int num = 0;
        Interlocked.Increment(ref num);
        Debug.Log(num);
        Debug.Log(Interlocked.Decrement(ref num));
        Debug.Log(num);
        Debug.Log(Interlocked.Exchange(ref num, 10));
        Debug.Log(num);
        Debug.Log(Interlocked.CompareExchange(ref num, 100, 10));
        Debug.Log(num);

        //         //创建RptThread线程，每隔一秒通报有几个线程存在  
        //         Thread reporter = new Thread(new ThreadStart(RptThread));
        //         //设置为后台线程  
        //         reporter.IsBackground = true;
        //         reporter.Start();
        // 
        //         //创建50个RndThreadFunc线程  
        //         Thread[] rndThreads = new Thread[50];
        //         for (int i = 0; i < 50; i++)
        //         {
        //             rndThreads[i] = new Thread(new ThreadStart(RndThreadFunc));
        //             rndThreads[i].Start();
        //            }
    }

    void RndThreadFunc()
    {
        //新建线程，线程数加一  
        Interlocked.Increment(ref threadCount);

        try
        {
            //什么事也不干， 只随机停留1~12秒  
            int sleepTime = rnd.Next(1000, 12000);
            Thread.Sleep(sleepTime);
        }
        finally
        {
            //线程结束，线程数减一  
            Interlocked.Decrement(ref threadCount);
        }
    }

    void RptThread()
    {
        while (true)
        {
            int threadNumber = 0;
            //获取当前线程数  
            threadNumber = Interlocked.Exchange(ref threadCount, threadCount);
            Debug.LogFormat("{0} Thread(s) alive ", threadNumber);

            if (threadNumber == 0)
            {
                break;
            }

            //休眠1秒  
            Thread.Sleep(1000);
        }
    }  


}
