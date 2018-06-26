using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalPool : MonoBehaviour {
    private static ParticalPool _Instance;
    public static ParticalPool Instance
    {
        get
        {
            return _Instance;
        }
    }


    private const int FlareCount = 20;
    private int m_FlareNowIndex = 0;
    private const int BloodEffectCount = 20;
    private int m_BloodNowIndex = 0;
    private GameObject[] m_Flares = new GameObject[FlareCount];
    private GameObject[] m_BloodEffects = new GameObject[BloodEffectCount];
    private GameObject[] m_BloodDecal = new GameObject[BloodEffectCount];
    public GameObject m_FlarePrefab;
    public GameObject m_BloodEffectPrefab;
    public GameObject m_BloodDecalPrefab;

    private float m_DeltaTime=1.0f;

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        if(m_FlarePrefab!=null)
        {
            for(int i=0;i<FlareCount;i++)
            {
                GameObject obj = GameObject.Instantiate(m_FlarePrefab);
                obj.transform.SetParent(this.transform);
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(false);
                m_Flares[i] = obj;
            }
        }
        if (m_BloodEffectPrefab != null && m_BloodDecalPrefab!=null)
        {
            for (int i = 0; i < BloodEffectCount; i++)
            {
                GameObject obj = GameObject.Instantiate(m_BloodEffectPrefab);
                obj.transform.SetParent(this.transform);
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(false);
                m_BloodEffects[i] = obj;

                obj = GameObject.Instantiate(m_BloodDecalPrefab);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                m_BloodDecal[i] = obj;
            }
        }
    }
    void OnDestroy()
    {
        _Instance = null;
    }

    public void PlayBoomPartical(Vector3 pos)
    {
        int ret = m_FlareNowIndex;
        if(m_FlareNowIndex>=FlareCount-1)
        {
            m_FlareNowIndex = 0;
        }
        else
        {
            m_FlareNowIndex++;
        }
        m_Flares[ret].transform.position = pos;
        m_Flares[ret].SetActive(true);
        StartCoroutine(GCPartical(m_Flares,ret));
    }

    IEnumerator GCPartical(GameObject[] array,int index,float time=1.0f)
    {
        yield return new WaitForSeconds(time);
        array[index].SetActive(false);
    }

    public void PlayBloodEffect(Vector3 pos)
    {
        int ret = m_BloodNowIndex;
        if (m_BloodNowIndex >= BloodEffectCount - 1)
        {
            m_BloodNowIndex = 0;
        }
        else
        {
            m_BloodNowIndex++;
        }
        m_BloodEffects[ret].transform.position = pos;
        m_BloodEffects[ret].SetActive(true);
        StartCoroutine(GCPartical(m_BloodEffects, ret,0.2f));
    }

    public void PlayBloodDecal(Vector3 pos)
    {
        int ret = m_BloodNowIndex;
        m_BloodDecal[ret].transform.position = pos;
        float scale = Random.Range(1.5f, 3.5f);
        m_BloodDecal[ret].transform.localScale = Vector3.one * scale;
        m_BloodDecal[ret].transform.rotation = Quaternion.Euler(90, 0, Random.Range(0, 360));
        m_BloodDecal[ret].SetActive(true);
        StartCoroutine(GCPartical(m_BloodDecal, ret,5.0f));
    }
}
