using UnityEngine;
using System.Collections;

//负责初始化怪物出生点的基类
public class EnemySPawn : MonoBehaviour {
    public GameObject prefab;
    public GameObject  Spawn()
    {
        return (GameObject)GameObject.Instantiate(prefab, transform.position, transform.rotation);
    }
}
