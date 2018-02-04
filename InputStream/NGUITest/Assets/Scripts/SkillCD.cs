using UnityEngine;
using System.Collections;

public class SkillCD : MonoBehaviour {
    public float coldTime = 2.0f;
	//是否可以使用
	private bool isCanRelease=false;
	public GameObject skillSpriteMask;
    //技能的遮罩sprite
    private UISprite cdMask;
	void Awake()
	{
		cdMask = skillSpriteMask.GetComponent<UISprite> ();
	}
	// Update is called once per frame
	void Update () {
		//如果当前处于冷却
		if (!isCanRelease) {
		    //减少冷却时间
            //fillAmount-=(1f / coldTime) * Time.deltaTime;
           cdMask.fillAmount -= (1f/ coldTime) * Time.deltaTime;
			if(cdMask.fillAmount<=0.05)
			{
				isCanRelease=true;
				cdMask.fillAmount=0;
			}
		}
		else if(Input.GetKeyDown(KeyCode.A)&&isCanRelease){
		   //放技能
		//1、释放技能，创建粒子系统，显示技能特效
			//2、UI上显示技能冷却效果
			isCanRelease=false;
			cdMask.fillAmount=1;

		}


	}
}
