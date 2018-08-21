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
        NORMAL
    }

	void Start () 
	{
		
        //if(animator.layerCount >= 2)
        //    animator.SetLayerWeight(1, 1);
	}
		
	void Update () 
	{
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animator  && _Type == TYPE.NORMAL)
		{
            //if (stateInfo.IsName("Base Layer.Run"))
            //{
            //    if (Input.GetKeyDown(KeyCode.Space)) animator.SetBool("Jump", true);                
            //}
            //else
            //{
            //    animator.SetBool("Jump", false);                
            //}

			if(Input.GetKeyDown(KeyCode.Z) && animator.layerCount >= 2)
			{
				animator.SetTrigger("Hi");
			}
			animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h,DirectionDampTime ,Time.deltaTime);	
		}
        else if (animator && _Type == TYPE.SIMPLE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Jump");

            if (Input.GetKeyDown(KeyCode.Z) && animator.layerCount >= 2)
            {
                animator.SetTrigger("Hi");
            }
            animator.SetFloat("Speed", v*10);
            animator.SetFloat("Dir", h);
            if (v > 0)
            {
                this.transform.position += transform.forward * Time.deltaTime * DirectionDampTime * v;
            }
            Debug.Log(v);

        }
	}
}
