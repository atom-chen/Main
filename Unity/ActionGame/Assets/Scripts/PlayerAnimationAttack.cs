using UnityEngine;
using System.Collections;

public class PlayerAnimationAttack : MonoBehaviour {
    private Animator animator;//得到动画状态机
    private bool isCanAttackB=false;
	// Use this for initialization
	void Start () {
        //得到按钮
        EventDelegate NormalAttackEvent=new EventDelegate(this,"OnNormalAttackClick");
        GameObject.Find("NormalAttack").GetComponent<UIButton>().onClick.Add(NormalAttackEvent);

        //得到按钮
        EventDelegate RangeAttackEvent = new EventDelegate(this, "OnRangeAttackClick");
        GameObject.Find("RangeAttack").GetComponent<UIButton>().onClick.Add(RangeAttackEvent);

        //得到按钮
        EventDelegate RedAttackEvent = new EventDelegate(this, "OnRedAttackClick");
        GameObject redAttack = GameObject.Find("RedAttack");
        redAttack.GetComponent<UIButton>().onClick.Add(RedAttackEvent);
        redAttack.SetActive(false);

        animator = this.GetComponent<Animator>();
	}
	
    //实现委托方法
    public void OnNormalAttackClick()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackA") && isCanAttackB)
        {
            animator.SetTrigger("Attack B");
        }
        else
        {
            animator.SetTrigger("Attack A");
        }
        
    }
    public void OnRangeAttackClick()
    {
        animator.SetTrigger("Attack Range");
    }
    public void OnRedAttackClick()
    {
        animator.SetTrigger("Attack Gun");
       
    }

    public void AttackBEvent1()
    {
        isCanAttackB = true;
    }

    public void AttackBEvent2()
    {
        isCanAttackB = false;
    }
}
