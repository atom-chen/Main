using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomParticalPool : MonoBehaviour {
    private static BoomParticalPool _Instance;
    public static BoomParticalPool Instance
    {
        get
        {
            return _Instance;
        }
    }


    private const int Count = 20;
    private GameObject[] m_Flares = new GameObject[Count];
    public GameObject m_FlarePrefab;
    private int m_NowIndex;
    private float m_DeltaTime=1.0f;

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        if(m_FlarePrefab!=null)
        {
            for(int i=0;i<Count;i++)
            {
                GameObject obj = GameObject.Instantiate(m_FlarePrefab);
                obj.transform.SetParent(this.transform);
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(false);
                m_Flares[i] = obj;
            }
        }
    }
    void OnDestroy()
    {
        _Instance = null;
    }

    public void PlayBoomPartical(Vector3 pos)
    {
        int ret = m_NowIndex;
        if(m_NowIndex>=Count-1)
        {
            m_NowIndex = 0;
        }
        else
        {
            m_NowIndex++;
        }
        m_Flares[ret].transform.position = pos;
        m_Flares[ret].SetActive(true);
        StartCoroutine(GCPartical(ret));
    }

    IEnumerator GCPartical(int index)
    {
        yield return new WaitForSeconds(m_DeltaTime);
        m_Flares[index].SetActive(false);
    }
}
