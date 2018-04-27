using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *敌人1号脚本 
 * 
 */
public class Enemy_01 : MonoBehaviour {

    private float m_Velocity = 3;//移动速度
    private float m_Tumble = 10;//角速度
    public Rigidbody m_RB;
	void Start () {
        m_RB.angularVelocity = Random.insideUnitSphere * m_Tumble;
        m_RB.velocity = new Vector3(0, 0, -m_Velocity);
	}
    void OnEnable()
    {

    }
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag==Tags.TAG_BOLT)
        {
            //扔回对象池
            ExpolsionPool.Instance.GetExpolsion(this.transform.position);
            this.transform.position = EnemyPool.Instance.transform.position;
            GameManager.Instance.KillEnemy(GameManager.Instance.GetCd());
            this.gameObject.SetActive(false);
        }
        else if(other.tag==Tags.TAG_PLAYER)
        {
            ExpolsionPool.Instance.CreatePlayerExplision(other.transform.position);
            this.transform.position = EnemyPool.Instance.transform.position;
            this.gameObject.SetActive(false);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.TAG_BOUND)
        {
            GameManager.Instance.EnemyOK();
            this.transform.position = EnemyPool.Instance.transform.position;
            this.gameObject.SetActive(false);
        }
    }
}
