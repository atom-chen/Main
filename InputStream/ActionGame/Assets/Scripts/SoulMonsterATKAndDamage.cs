using UnityEngine;
using System.Collections;

//处理Monst的攻击 数值计算
public class SoulMonsterATKAndDamage : ATKAndDamage{
    private Transform player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }
    public void MonAttack()
    {
        //攻击距离判断
        if(Vector3.Distance(transform.position,player.position)<attackDistance)
        {
            //减少生命值
            player.GetComponent<ATKAndDamage>().TakeDamage(normalAtack);
        }
    }
}
