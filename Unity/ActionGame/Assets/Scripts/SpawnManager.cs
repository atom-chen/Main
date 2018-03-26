using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {
    public static SpawnManager _instance;
    public EnemySPawn[] monsterSpawnArray;
    public EnemySPawn[] bossSpawnArray;

    public List<GameObject> enemyList = new List<GameObject>();
    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(Spawn());
	}
	
    IEnumerator Spawn()
    {
        //第一波敌人的生成
        foreach(EnemySPawn s in monsterSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }
        while(enemyList.Count>0)
        {
            yield return new WaitForSeconds(0.2f);
        }
        //第二波敌人的产生
        foreach (EnemySPawn s in monsterSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }
        yield return new WaitForSeconds(1f);
        foreach (EnemySPawn s in monsterSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }

        while (enemyList.Count > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        //第三波敌人的产生
        foreach (EnemySPawn s in monsterSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }
        yield return new WaitForSeconds(1f);
        foreach (EnemySPawn s in monsterSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }
        yield return new WaitForSeconds(1f);

        foreach(EnemySPawn s in bossSpawnArray)
        {
            enemyList.Add(s.Spawn());
        }
    }
}
