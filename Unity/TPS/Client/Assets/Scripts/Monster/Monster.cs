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

public delegate void OnMonstDie(Monster monst);

public class Monster : MonoBehaviour
{
    private Transform monsterTrans;
    private Transform playerTrans;
    private Transform footTrans;
    private NavMeshAgent nvAgent;
    private Animator m_Animator;

    private float m_AttackDistance = 2.0f;
    private float m_traceDistance = 10.0f;

    bool m_bIsInit = false;

    private float m_HP = 100;
    public event OnMonstDie WhenMonstDie;
    //Need to init
    Collider coll;
    Collider[] coInChild;
    public MonstStats m_State = MonstStats.idel;
    public bool m_IsDie = false;
    void Start()
    {
        if(m_bIsInit)
        {
            return;
        }
        monsterTrans = this.transform;
        nvAgent = this.GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        footTrans = transform.Find("FootPos");
        if (playerObj != null)
        {
            playerTrans = playerObj.transform;
        }
        m_Animator = this.GetComponent<Animator>();
        coll = this.GetComponent<Collider>();
        coInChild = this.GetComponentsInChildren<Collider>();
        Player.m_OnPlayerDie += OnPlayerDie;
    }

    void OnEnable()
    {
        if(!m_bIsInit)
        {
            Start();
            m_bIsInit = true;
        }
        StopAllCoroutines();
        StartCoroutine(CheckMonstState());
        StartCoroutine(MonsterAction());
    }
    void OnDisable()
    {
        Player.m_OnPlayerDie -= OnPlayerDie;
    }

    IEnumerator CheckMonstState()
    {
        while (!m_IsDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(monsterTrans.position, playerTrans.position);
            if (m_HP <= 0)
            {
                m_State = MonstStats.die;
            }
            else if (dist <= m_AttackDistance)
            {
                m_State = MonstStats.attack;
            }
            else if (dist <= m_traceDistance)
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
        while (!m_IsDie)
        {
            switch (m_State)
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
        if(nvAgent.isOnNavMesh)
        {
            nvAgent.Stop();
        }
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
        if(!nvAgent.isStopped)
        {
            nvAgent.Stop();
        }
        m_Animator.SetBool("IsAttack", true);
    }
    void OnDie()
    {
        if (WhenMonstDie != null)
        {
            WhenMonstDie(this);
        }
        StopAllCoroutines();
        m_IsDie = true;
        if (nvAgent.isOnNavMesh)
        {
            nvAgent.Stop();
        }
        m_Animator.SetTrigger("IsDie");
        coll.enabled = false;
        foreach (Collider co in coInChild)
        {
            co.enabled = false;
        }
        Player.m_OnPlayerDie -= OnPlayerDie;
        StartCoroutine(GCMonst());
        //Destroy(this.gameObject, 3.0f);
    }

    IEnumerator GCMonst()
    {
        yield return new WaitForSeconds(3.00f);
        //初始化各种变量
        coll.enabled = true;
        m_State = MonstStats.idel;
        m_HP = 100;
        foreach (Collider co in coInChild)
        {
            co.enabled = true;
        }
        m_IsDie = false;
        MonstPool.Instance.GCMonst(this);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            BulletPool.Instance.GCBullet(other.gameObject);
            OnDamage(other.transform.position);
        }
    }

    /// <summary>
    /// 被攻击时调用
    /// </summary>
    /// <param name="beAttackPos">被击中的位置</param>
    public void OnDamage(Vector3 beAttackPos)
    {
        m_Animator.SetTrigger("IsHit");
        ParticalPool.Instance.PlayBloodDecal(footTrans.position);
        ParticalPool.Instance.PlayBloodEffect(beAttackPos);
        m_HP -= 50;
    }
    void OnPlayerDie()
    {
        StopAllCoroutines();
        if (nvAgent.isOnNavMesh)
        {
            nvAgent.Stop();
        }
        m_Animator.SetTrigger("IsPlayerDie");
    }
}
