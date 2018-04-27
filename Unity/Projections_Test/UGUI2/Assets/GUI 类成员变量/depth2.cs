using UnityEngine;
using System.Collections;

public class depth2 : MonoBehaviour {
    public static int guiDepth = 0;                                  //声明一个初始值为0的静态整型变量guiDepth
    void OnGUI()
    {                                                               //声明OnGUI方法
        GUI.depth = guiDepth;                                                   //将GUI.depth设置为guiDepth
        if (GUI.RepeatButton(new Rect(Screen.width / 8, Screen.height / 8,    //绘制一个名为GoBack的RepeatButton
            Screen.width / 3, Screen.height / 3), "GoBeijing"))
        {                   //若持续按下按钮GoBack，guiDepth变量置为1
            depth2.guiDepth = 0;
            depth.guiDepth = 1;                                                //将Test10.guiDepth的值置为0
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
