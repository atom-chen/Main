using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 特效对象池
 */ 
public class ExpolsionPool : MonoBehaviour
{
    public Transform m_PlayerExplision;

    private int m_NextIndex = 0;
    public ParticleSystem[] m_Expolsion;

    private static ExpolsionPool _Instance;
    private float m_WaitTime=1.5f;
    public static ExpolsionPool Instance
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
    public void GetExpolsion(Vector3 pos)
    {
        if (m_NextIndex >= m_Expolsion.Length)
        {
            m_NextIndex = 0;
        }
        if (!m_Expolsion[m_NextIndex].gameObject.activeInHierarchy)
        {
            m_Expolsion[m_NextIndex].transform.position = pos;
            m_Expolsion[m_NextIndex].gameObject.SetActive(true);
            StartCoroutine(ActiveFalse(m_NextIndex++));
        }
    }

    IEnumerator ActiveFalse(int index)
    {
        yield return new WaitForSeconds(m_WaitTime);
        m_Expolsion[index].gameObject.SetActive(false);
    }

    public void CreatePlayerExplision(Vector3 position)
    {
        m_PlayerExplision.position = position;
        m_PlayerExplision.gameObject.SetActive(true);
        StartCoroutine(OnEnableExplision(2));
    }
    IEnumerator OnEnableExplision(float time)
    {
        yield return new WaitForSeconds(time);
        m_PlayerExplision.gameObject.SetActive(false);
    }
}