using UnityEngine;
using System.Collections;

public class IdleRunJump : MonoBehaviour {


	public Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;
    public TYPE _Type;
    public enum TYPE
    {
        SIMPLE,
        NORMAL,
        HARD
    }

	void Start () 
	{
		
        //if(animator.layerCount >= 2)
        //    animator.SetLayerWeight(1, 1);
        if(animator.runtimeAnimatorController.name == "AC1")
        {
            _Type = TYPE.SIMPLE;
        }
        else if(animator.runtimeAnimatorController.name == "AC2")
        {
            _Type = TYPE.NORMAL;
        }
        else
        {
            _Type = TYPE.HARD;
        }
	}
		
	void Update () 
	{
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (animator  && _Type == TYPE.NORMAL)
		{
            if (Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Jump");

            if (Input.GetKeyDown(KeyCode.Z))
            {
                animator.SetTrigger("Hi");
            }
            animator.SetFloat("Speed", v * 10);
            animator.SetFloat("Dir", h);
            if (v > 0)
            {
                this.transform.position += transform.forward * Time.deltaTime * DirectionDampTime * v;
            }
		}
        else if (animator && _Type == TYPE.SIMPLE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Jump");

            if (Input.GetKeyDown(KeyCode.Z))
            {
                animator.SetTrigger("Hi");
            }
            animator.SetFloat("Speed", v*10);
            if (v > 0)
            {
                this.transform.position += transform.forward * Time.deltaTime * DirectionDampTime * v;
            }
        }
        else if(_Type == TYPE.HARD)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //如果当前处于跑步状态
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetKeyDown(KeyCode.Space)) 
                    animator.SetTrigger("Jump");
            }
            //如果有多个Layer
            if (Input.GetKeyDown(KeyCode.Z) && animator.layerCount >= 2)
            {
                animator.SetTrigger("Hi");
            }
            animator.SetFloat("Speed", v * 10);
            animator.SetFloat("Dir", h);
            if (v > 0)
            {
                this.transform.position += transform.forward * Time.deltaTime * DirectionDampTime * v;
            }
        }
	}
}
