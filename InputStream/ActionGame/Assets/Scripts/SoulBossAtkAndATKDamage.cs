using UnityEngine;
using System.Collections;

public class SoulBossAtkAndATKDamage : ATKAndDamage {
    private Transform player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }

    public void Attack1()
    {
        //攻击距离判断
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //减少生命值
            player.GetComponent<ATKAndDamage>().TakeDamage(normalAtack);
        }
    }
    public void Attack2()
    {
        //攻击距离判断
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //减少生命值
            player.GetComponent<ATKAndDamage>().TakeDamage(normalAtack);
        }
    }
}
