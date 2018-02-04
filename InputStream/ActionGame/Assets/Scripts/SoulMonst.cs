using UnityEngine;
using System.Collections;

public class SoulMonst : MonoBehaviour
{
    private Animator animator;
    private Transform player;//玩家位置
    private CharacterController cc;

    public float speed = 3;//移动速度
    public float attackDistance = 1;//攻击距离和寻路距离
    public float AttackTime = 3;//攻击CD
    private float attackTimer = 0;//计时器
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;//拿到玩家位置的引用
        cc = this.GetComponent<CharacterController>();//获取自己的角色控制器
        animator = this.GetComponent<Animator>();//获取自己的动画控制器
        attackTimer = AttackTime;//初始化为可以攻击
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position;
        targetPos .y= this.transform.position.y;
        //print(this.name + "看到了玩家 在" + targetPos);
        transform.LookAt(targetPos);//Monsr朝向玩家位置
        
        float distance = Vector3.Distance(targetPos, transform.position);//获取玩家与Monst当前位置距离
        //如果距离足够
        if (distance <= attackDistance)
        {
            attackTimer += Time.deltaTime;//时间累计
            if (attackTimer > AttackTime)
            {
                //播放攻击动画
               animator.SetTrigger("Attack");
               attackTimer = 0;
            }
            //如果时间不够，还不能进行攻击
            else
            {
                //站立
                animator.SetBool("Walk", false);
            }
        }
        //如果距离不够
        else
        {
            animator.SetBool("Walk", true);
            //重置攻击CD
            attackTimer = AttackTime;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("MonRun"))//跟踪
            {
                //跑向玩家
                cc.SimpleMove(this.transform.forward * speed);    
            }
            
        }


    }
}
