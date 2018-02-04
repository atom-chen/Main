using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
    //enabled变量：判断图形用户界面组件是否被启动
    //对enabled的布尔值进行设置，控制图形用户界面组件的启动情况
    public bool allOptions = true;
    public bool extends1 = true;
    public bool extends2 = true;

    public float width = 0;//屏幕宽
    public float height = 0;//屏幕高
    // Use this for initialization
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }
    void OnGUI()
    {
        //绘制一个名为Edit All Options的开关，初始状态为allOptions
        allOptions = GUI.Toggle(new Rect(0, 0, width / 5, height / 10), allOptions, "Edit All Options");
        //将总开关的布尔值赋给enabled组件
        GUI.enabled = allOptions;
        //在自定义区域画两个子开关
        extends1 = 
            GUI.Toggle(new Rect(width / 10, height / 10, width / 5, height / 10), extends1, "Extended Option1");
        extends2 =
            GUI.Toggle(new Rect(width / 10, height / 5, width / 5, height / 10), extends1, "Extended Option2");
        GUI.enabled = true;//将enabled的值设为true
        //画一个按钮 判断是否被按下
        if(GUI.Button(new Rect(0,height*3/10,width/5,height/10),"ok"))
        {
			Debug.Log("ok!");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
