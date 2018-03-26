using UnityEngine;
using System.Collections;

public class SoulBoss : MonoBehaviour {
    private Animator animator;
    private Transform player;
    public float attackDistance=4;//攻击距离
    public float speed=2;//移动速度
    private CharacterController cc;
    public float AttackTime = 3;//攻击CD
    private float attackTimer = 0;//计时器
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        cc = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        attackTimer=AttackTime;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos = player.position;
        targetPos.y = this.transform.position.y;//保证y坐标不变
        transform.LookAt(targetPos);//Boss朝向目标点

        float distance = Vector3.Distance(targetPos, transform.position);//获取玩家与Boss当前位置距离
        if(distance<=attackDistance)
        {
            attackTimer+=Time.deltaTime;
            if(attackTimer>=AttackTime)
            {
                //攻击
               if (Random.Range(0, 2)==0)
               {
                   //Attack1攻击
                   animator.SetTrigger("Attack1");
               }
               else
               {
                   animator.SetTrigger("Attack2");
               }
               attackTimer = 0;
            }
            //如果时间不够，还不能进行攻击
            else
            {
                //站立
                animator.SetBool("Walk", false);
            }
        }
        else 
        {
            attackTimer = AttackTime;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BossRun01"))//跟踪
            {
                //跑向玩家
                cc.SimpleMove(this.transform.forward * speed);
                
            }
            animator.SetBool("Walk", true);
        }


	}
}
