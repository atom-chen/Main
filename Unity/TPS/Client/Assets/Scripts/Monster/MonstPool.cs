using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstPool : MonoBehaviour {
    private static MonstPool _Instance;
    public static MonstPool Instance
    {
        get
        {
            return _Instance;
        }
    }

    private Transform m_Transform;
    private GameObject m_MonstPrefab;
    private const int MonstMax=10;
    private int index = 0;
    private Monster[] m_ArrMonster = new Monster[MonstMax];

    void Awake()
    {
        _Instance = this;
        m_MonstPrefab = Resources.Load("Prefabs/monster") as GameObject;
        m_Transform = transform;
    }

	void Start () 
    {
        for(int i=0;i<MonstMax;i++)
        {
            GameObject obj = Instantiate(m_MonstPrefab,m_Transform);
            Monster monst = obj.GetComponent<Monster>();
            if (monst != null)
            {
                monst.WhenMonstDie += GameManager.Ins.OnMonstDie;
                m_ArrMonster[i] = monst;
            }
            obj.SetActive(false);
        }
	}

    void OnDestroy()
    {
        _Instance = null;
    }

    public Monster GetMonster()
    {
        for(int i=0;i<MonstMax;i++)
        {
            if(!m_ArrMonster[i].isActiveAndEnabled)
            {
                m_ArrMonster[i].gameObject.SetActive(true);
                return m_ArrMonster[i];
            }
        }
        return null;
    }

    public void GCMonst(Monster monst)
    {
        monst.gameObject.SetActive(false);
        monst.transform.SetPositionAndRotation(m_Transform.position, Quaternion.identity);
    }

}
