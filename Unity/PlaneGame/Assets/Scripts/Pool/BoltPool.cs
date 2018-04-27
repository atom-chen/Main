using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 子弹对象池
 */ 
public class BoltPool : MonoBehaviour {
    public Bolt[] m_Bolts;
    private int m_NextIndex = 0;
    public Transform m_Player;

    private static BoltPool _Instance;
    public static BoltPool Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance=this;
    }
    void OnEnable()
    {
        for(int i=0;i<m_Bolts.Length;i++)
        {
            m_Bolts[i].transform.parent = this.transform;
            m_Bolts[i].gameObject.SetActive(false);
        }
    }
    public void GetBolt()
    {
        if(this.transform.childCount>0)
        {
            if(m_NextIndex>=m_Bolts.Length)
            {
                m_NextIndex = 0;
            }
            m_Bolts[m_NextIndex].gameObject.SetActive(true);
            m_Bolts[m_NextIndex++].SetParent(m_Player);
        }
    }


    
}
