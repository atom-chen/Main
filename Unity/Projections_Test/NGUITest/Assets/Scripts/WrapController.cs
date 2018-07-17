using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapController : MonoBehaviour {

    public UIWrapContent m_WrapContent;
    private string[] m_Data = new string[100];
    void Start()
    {
        m_WrapContent.onInitializeItem = OnUpdateWrap;
        for (int i = 0; i < m_Data.Length; i++)
        {
            m_Data[i] = i.ToString();
        }
    }

    void OnUpdateWrap(GameObject obj,int wrapIndex,int dataIndex)
    {
        Debug.Log(obj.name + "    " + wrapIndex + "     " + dataIndex);
        EmailItem emailItem = obj.GetComponent<EmailItem>();
        if(emailItem!=null)
        {
            emailItem.InitEmailItem("1", dataIndex.ToString(), DateTime.Now.ToString());
        }
    }
}
