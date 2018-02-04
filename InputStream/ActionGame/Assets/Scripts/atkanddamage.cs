using UnityEngine;
using System.Collections;

public class ATKAndDamage : MonoBehaviour {
    public float hp = 100;
    public float normalAtack = 50;
    public float attackDistance = 1;
    protected Animator animator;

    protected void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    
    public virtual void TakeDamage(float damage)
    {
        if(hp>0)
        {
            hp -= damage;    
        }
        if (animator == null)
        {
            animator = this.GetComponent<Animator>();
        }
        //处理动画
        if(hp>0)
        {        
            //播放受伤动画
            if (this.tag == Tags.soulBoss || this.tag == Tags.soulMonst)
            {
                animator.SetTrigger("Damage");
            }
        }
        else
        {
            animator.SetBool("Dead", true);

            //如果死亡的是Monst或者Boss
            if (this.tag == Tags.soulBoss || this.tag == Tags.soulMonst)
            {
                Destroy(this.gameObject, 1);
                SpawnManager._instance.enemyList.Remove(this.gameObject);
                this.GetComponent<CharacterController>().enabled = false;//关闭它们的角色控制器
                //生成掉落物品
                SpawnAward();
            }
        }
        //播放受打击的特效
        if (this.tag == Tags.soulBoss)//如果是Boss被打
        {
            //实例化Boss的被打特效
            GameObject.Instantiate(Resources.Load("HitBoss"), this.transform.position + Vector3.up, this.transform.rotation);

        }
        else if (this.tag == Tags.soulMonst)
        {
            GameObject.Instantiate(Resources.Load("HitMonster"), this.transform.position + Vector3.up, this.transform.rotation);
        }
    }

    void SpawnAward()
    {
          //生成奖励物品
           int index = Random.Range(0, 2);
            if(index==0)
            {
                //生成双刃剑球
                GameObject.Instantiate(Resources.Load("Item-DualSword"), transform.position + Vector3.up, Quaternion.identity);
            }
            else
            {
                //生成枪
                GameObject.Instantiate(Resources.Load("Item-Gun"), transform.position + Vector3.up, Quaternion.identity);
            }
    }
}
