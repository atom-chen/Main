using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 敌人对象池
 */ 
public class EnemyPool : MonoBehaviour {
    public Enemy_01[] m_Enemy_01;
    private int m_NextIndex = 0;


    public float m_RandRange = 6;

    private float m_CD = 3;
    private float m_Delta = 0;

    private static EnemyPool _Instance;
    public static EnemyPool Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
    }
    void OnEnable()
    {
        for(int i=0;i<m_Enemy_01.Length;i++)
        {
            m_Enemy_01[i].gameObject.SetActive(false);
        }
        ReSetCD();
    }

    void Update()
    {
        m_Delta += Time.deltaTime;
        if(m_Delta>=m_CD)
        {
            m_Delta = 0;
            if(m_CD>0.5f)
            {
                m_CD -= 0.25f;
            }
            CreateEnemy();
        }
    }
	
    public void CreateEnemy()
    {
        if (m_NextIndex >= m_Enemy_01.Length)
        {
            m_NextIndex = 0;
        }
        if (!m_Enemy_01[m_NextIndex].gameObject.activeInHierarchy)
        {
            m_Enemy_01[m_NextIndex].transform.position = this.transform.position + new Vector3(Random.Range(-m_RandRange, m_RandRange), 0, 0);
            m_Enemy_01[m_NextIndex].gameObject.SetActive(true);
        }
        m_NextIndex++;
    }

    public void ReSetCD()
    {
        m_CD = 3;
    }



    
}
