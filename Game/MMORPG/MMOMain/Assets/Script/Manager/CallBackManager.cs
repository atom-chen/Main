using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CallBackManager : MonoBehaviour
{
    public static CallBackManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    private static CallBackManager _Instance;


    public delegate void NotReturn();
    public static NotReturn OneMinCallBack;//一分钟回调一次

    public static NotReturn OneSecondCallBack;//一秒回调一次

    void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        StartCoroutine(SecondLoop());
        StartCoroutine(MinLoop());
    }


    //全局
    IEnumerator SecondLoop()
    {
        while (true)
        {
            if (OneSecondCallBack != null)
            {
                foreach (NotReturn item in OneSecondCallBack.GetInvocationList())
                {
                    item();
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator MinLoop()
    {
        while (true)
        {
            if (OneMinCallBack != null)
            {
                foreach (NotReturn item in OneMinCallBack.GetInvocationList())
                {
                    item();
                }
            }
            yield return new WaitForSeconds(60);
        }
    }
}
