using System.Collections;
using UnityEngine;
using System.Collections.Generic;
//攻击的计算部分

public class PlayerATKAndDamage : ATKAndDamage {
    public float attackB = 80;
    public float attackRange = 100;
    public WeaponGun gun;

    public void AttackA()
    {
        GameObject enemy = null;
        float distance = attackDistance;
        //遍历所有敌人
        foreach(GameObject go in SpawnManager._instance.enemyList)
        {
            float nowdistance = Vector3.Distance(go.transform.position, transform.position);
            //找到距离玩家最近的敌人
            if(nowdistance<distance)
            {
                enemy = go;
                distance = nowdistance;
            }
        }
        if(enemy!=null)
        {
            Vector3 targetPos=enemy.transform.position;
            targetPos.y=transform.position.y;
            //面向敌人
            transform.LookAt(targetPos);

            //处理伤害
            enemy.GetComponent<ATKAndDamage>().TakeDamage(normalAtack);
        }
        //否则不处理伤害
    }
    public void AttackB()
    {
        GameObject enemy = null;
        float distance = attackDistance;
        //遍历所有敌人
        foreach (GameObject go in SpawnManager._instance.enemyList)
        {
            float nowdistance = Vector3.Distance(go.transform.position, transform.position);
            //找到距离玩家最近的敌人
            if (nowdistance < distance)
            {
                enemy = go;
                distance = nowdistance;
            }
        }
        if (enemy != null)
        {
            Vector3 targetPos = enemy.transform.position;
            targetPos.y = transform.position.y;
            //面向敌人
            transform.LookAt(targetPos);
            //处理伤害
            enemy.GetComponent<ATKAndDamage>().TakeDamage(attackB);
        }
    }
    public void AttackRange()
    {
        List<GameObject> enemy = new List<GameObject>();
        //遍历所有敌人
        foreach (GameObject go in SpawnManager._instance.enemyList)
        {
            if (Vector3.Distance(go.transform.position, transform.position) < attackDistance)
            {
                enemy.Add(go);     
            }
        }

        //进行攻击
        foreach(GameObject go in enemy)
        {
            //进行伤害计算（获取该角色的脚本）  
            go.GetComponent<ATKAndDamage>().TakeDamage(attackRange);
        }
    }
    public void AttackGun()
    {
        gun.Shot();    
    }

}
