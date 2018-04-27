using UnityEngine;
using System.Collections;

public class depth : MonoBehaviour {
    //depth：层级关系
    public static int guiDepth = 0;                                             //声明一个初始值为0的静态整型变量guiDepth
    void OnGUI()
    {                                                               
        GUI.depth = guiDepth;                                                   //将GUI.depth设置为guiDepth
        if (GUI.RepeatButton(new Rect(Screen.width / 10, Screen.height / 10,    //绘制一个名为GoBack的RepeatButton
            Screen.width / 5, Screen.height / 5), "GoHome"))
        {                   //若持续按下按钮GoBack，guiDepth变量置为1
            depth.guiDepth = 0;
            depth2.guiDepth = 1;                                                //将guiDepth的值置为0
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
