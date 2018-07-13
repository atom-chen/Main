using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    private static GameManager _Instance;


    public delegate void NotReturn();
    public NotReturn m_OneMinCallBack;//一分钟回调一次
    public NotReturn OneMinCallBack
    {
        get { return m_OneMinCallBack; }
        set { m_OneMinCallBack = value; }
    }
    public NotReturn m_OneSecondCallBack;//一秒回调一次
    public NotReturn OneSecondCallBack
    {
        get { return m_OneSecondCallBack; }
        set { m_OneSecondCallBack = value; }
    }

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

    void Update()
    {

    }

    //全局
    IEnumerator SecondLoop()
    {
        while (true)
        {
            if (m_OneSecondCallBack != null)
            {
                foreach (NotReturn item in m_OneSecondCallBack.GetInvocationList())
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
            if (m_OneMinCallBack != null)
            {
                foreach (NotReturn item in m_OneMinCallBack.GetInvocationList())
                {
                    item();
                }
            }
            yield return new WaitForSeconds(60);
        }
    }
}
