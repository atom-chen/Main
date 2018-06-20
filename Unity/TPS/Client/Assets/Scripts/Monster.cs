using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonstStats
{
    idel,
    trace,
    attack,
    die
}

public class Monster : MonoBehaviour {
    private Transform monsterTrans;
    private Transform playerTrans;
    private Transform footTrans;
    private NavMeshAgent nvAgent;
    private Animator m_Animator;

    private float m_AttackDistance = 2.0f;
    private float m_traceDistance = 10.0f;
    private MonstStats m_State = MonstStats.idel;
    private bool m_IsDie = false;
    void Start()
    {
        monsterTrans = this.transform;
        nvAgent = this.GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        footTrans = transform.Find("FootPos");
        if(playerObj!=null)
        {
            playerTrans = playerObj.transform;
        }
        m_Animator = this.GetComponent<Animator>();
        StartCoroutine(CheckMonstState());
        StartCoroutine(MonsterAction());
    }

    IEnumerator CheckMonstState()
    {
        while (!m_IsDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(monsterTrans.position, playerTrans.position);
            if (dist <= m_AttackDistance)
            {
                m_State = MonstStats.attack;
            }
            else if(dist<=m_traceDistance)
            {
                m_State = MonstStats.trace;
            }
            else
            {
                m_State = MonstStats.idel;
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while(!m_IsDie)
        {
            switch(m_State)
            {
                case MonstStats.idel:
                    OnIdel();
                    break;
                case MonstStats.trace:
                    OnTrace();
                    break;
                case MonstStats.attack:
                    OnAttack();
                    break;
                case MonstStats.die:
                    OnDie();
                    break;
            }
            yield return null;
        }
    }

    void OnIdel()
    {
        nvAgent.Stop();
        m_Animator.SetBool("IsTrace", false);
    }
    void OnTrace()
    {
        m_Animator.SetBool("IsAttack", false);
        m_Animator.SetBool("IsTrace", true);
        nvAgent.destination = playerTrans.position;
        nvAgent.isStopped = false;
    }
    void OnAttack()
    {
        nvAgent.Stop();
        m_Animator.SetBool("IsAttack", true);
    }
    void OnDie()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Bullet")
        {
            BulletPool.Instance.GCBullet(other.transform);
            m_Animator.SetTrigger("IsHit");
            ParticalPool.Instance.PlayBloodDecal(footTrans.position);
            ParticalPool.Instance.PlayBloodEffect(other.transform.position);
        }
    }
}
