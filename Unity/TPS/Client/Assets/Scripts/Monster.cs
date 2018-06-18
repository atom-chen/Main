using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {
    private Transform monsterTrans;
    private Transform playerTrans;
    private NavMeshAgent nvAgent;
    void Start()
    {
        monsterTrans = this.transform;
        nvAgent = this.GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj!=null)
        {
            playerTrans = playerObj.transform;
            nvAgent.destination = playerTrans.position;
        }

    }

    void Update()
    {
        nvAgent.destination = playerTrans.position;
    }
}
