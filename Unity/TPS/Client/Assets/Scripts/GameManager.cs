using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏管理器：控制游戏的功能集合
/// </summary>
public class GameManager : MonoBehaviour 
{
    private static GameManager _Instance;
    public static GameManager Ins
    {
        get
        {
            return _Instance;
        }
    }

    private List<Transform> m_MonstSpawnPoints=new List<Transform>();


    private float m_SpawnTimer = 2.0f;//生成CD
    private int m_MonsterMaxCount = 5;//最大数量
    private bool m_GameIsOver = false;
    private int m_MonstNowCount=0;//当前怪兽数量
   
    void Awake()
    {
        _Instance = this;
    }
	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(gameObject);
        GameObject spawnPointRoot = GameObject.Find("MonsterSpawnRoot");
        if(spawnPointRoot!=null)
        {
            Transform rootTrans=spawnPointRoot.transform;
            for(int i=0;i<rootTrans.childCount;i++)
            {
                m_MonstSpawnPoints.Add(rootTrans.GetChild(i));
            }
        }
        if(m_MonstSpawnPoints.Count>0)
        {
            StartCoroutine(SpawnMonsters());
        }
        Player.m_OnPlayerDie += GameOver;
	}
    IEnumerator SpawnMonsters()
    {
        while(!m_GameIsOver)
        {
            if(m_GameIsOver)
            {
                yield break;
            }
            yield return new WaitForSeconds(m_SpawnTimer);
            if(m_MonstNowCount<m_MonsterMaxCount)
            {
                int index = Random.Range(0, m_MonstSpawnPoints.Count);
                Monster monst = MonstPool.Instance.GetMonster();
                if(monst!=null)
                {
                    monst.transform.position = m_MonstSpawnPoints[index].position;
                    m_MonstNowCount++;
                }
            }
        }
    }
	

    public void OnMonstDie(Monster monst)
    {
        m_MonstNowCount--;
    }

    public void GameOver()
    {
        m_GameIsOver = true;
    }
}
