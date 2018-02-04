using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private Animator animator;
    private CharacterController cc;
    public float speed = 4;
    float h;
    float v;
    
	void Start () {
        h = 0; v = 0;
        cc = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        h= Input.GetAxis("Horizontal");//获取a d键的输入（如果有）
        v = Input.GetAxis("Vertical");//获取w s键的输入（如果有）

        //按键取值以虚拟杆为主
        //每帧读取Joystick类中 h和v的值（-1~1）
        if(Joystick.h!=0 || Joystick.v!=0)
        {
            h = Joystick.h;
            v = Joystick.v;
        }
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            animator.SetBool("Walk", true);
            //如果处于跑步动画中 则移动
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerRun"))
            {
                //获取移动方向
                Vector3 targetDir = new Vector3(h, 0, v);
                //面朝移动方向
                transform.LookAt(targetDir + transform.position);
                //移动
                cc.SimpleMove(transform.forward * speed);
            }
        }
        else
        {
            //置为站立状态
            animator.SetBool("Walk", false);
        }
	}
}