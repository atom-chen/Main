using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {
    public GameObject m_BulletPrefab;
    private float m_WaitTimer=2.0f;
    private Transform[] m_Bullets = new Transform[m_Size];
    private int m_NowIndex=0;
    private const int m_Size=20;

    private Transform m_PoolTranform;
    private static BulletPool _Instance;
    public static BulletPool Instance
    {
        get
        {
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
        m_PoolTranform = this.transform;
    }
    void OnDestroy()
    {
        _Instance = null;
    }
    void Start()
    {
        //if(m_BulletPrefab!=null)
        //{
        //    for(int i=0;i<m_Size;i++)
        //    {
        //        GameObject obj = GameObject.Instantiate(m_BulletPrefab);
        //        m_Bullets[i] = obj.transform;
        //        m_Bullets[i].parent = m_PoolTranform;
        //    }
        //}
    }
    
    public Transform GetBullet()
    {
        //int ret = m_NowIndex;
        //if(m_Bullets[ret].gameObject.activeInHierarchy)
        //{
        //    m_Bullets[ret].gameObject.SetActive(false);
        //}
        //if(m_NowIndex>=m_Size-1)
        //{
        //    m_NowIndex = 0;
        //}
        //else
        //{
        //    m_NowIndex++;
        //}
        //StartCoroutine(gcBullet(m_Bullets[ret]));
        //return m_Bullets[ret];
        GameObject obj = GameObject.Instantiate(m_BulletPrefab);
        obj.transform.parent=m_PoolTranform;
        StartCoroutine(gcBullet(obj));
        return obj.transform;
    }
    public void GCBullet(GameObject bullet)
    {
        if (bullet!=null && bullet.activeInHierarchy && bullet.tag == "Bullet")
        {
            Destroy(bullet);
            //bullet.parent = m_PoolTranform;
            //bullet.gameObject.SetActive(false);
        }
    }

    IEnumerator gcBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(m_WaitTimer);
        if(bullet!=null)
        {
            GCBullet(bullet);
        }
    }
}
