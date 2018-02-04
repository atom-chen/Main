using UnityEngine;
using System.Collections;

public class enabled : MonoBehaviour {
 
    //enabled变量：启用
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
        //总开关
        GUI.enabled = allOptions;
        //在自定义区域画两个子开关
        extends1 = 
            GUI.Toggle(new Rect(width / 10, height / 10, width / 5, height / 10), extends1, "Extended Option1");
        extends2 =
            GUI.Toggle(new Rect(width / 10, height / 5, width / 5, height / 10), extends2, "Extended Option2");
        //画一个按钮 判断是否被按下
        if(GUI.Button(new Rect(0,height*3/10,width/5,height/10),"ok"))
        {
            Debug.Log("ok");
        }

    }
    /*
    void OnGUI()
    {
        GUI.Button(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), new GUIContent("Click me", "This is the tooltip"));
        GUI.Label(new Rect(Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 10), GUI.tooltip);
    }
     */
	
	// Update is called once per frame
	void Update () {
	
	}
}
