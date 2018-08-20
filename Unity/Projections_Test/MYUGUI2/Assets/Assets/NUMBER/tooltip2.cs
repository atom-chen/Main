using UnityEngine;
using System.Collections;

public class tooltip2 : MonoBehaviour {
    void OnGUI()
    {                                   
        //在自定义区域绘制一个Box，Box中的内容为Box，且提示信息为this box has a tooltip
        GUI.Box(new Rect(Screen.width / 20, Screen.height / 10, Screen.width * 3 / 5,
            Screen.height * 3 / 5), new GUIContent("Box", "this box has a tooltip"));
        //在自定义区域绘制一个名为No tooltip here的按钮
        GUI.Button(new Rect(Screen.width / 10, Screen.height / 3, Screen.width / 2,
            Screen.height / 10), "No tooltip here");
        //在自定义区域绘制一个内容为I have a tooltip的按钮，且提示信息为The button overrides the box
        GUI.Button(new Rect(Screen.width / 10, Screen.height / 2, Screen.width / 2,
            Screen.height / 10), new GUIContent("I have a tooltip", "The button overrides the box"));
        //在自定义区域绘制一个标签，标签显示的内容为GUI.tooltip提供的信息
        GUI.Label(new Rect(Screen.width / 10, Screen.height / 5, Screen.width * 2 / 5,
            Screen.height / 10), GUI.tooltip);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
