using UnityEngine;
using System.Collections;

public class tooltip3 : MonoBehaviour {
    public string lastTooltip = " ";                                //声明一个名为lastTooltip的空字符串
    void OnGUI()
    {                                                   
        GUILayout.Button(new GUIContent("Play Game", "Button1"));   //通过GUI的布局管理器绘制一个按钮Button1                                                                    
        GUILayout.Button(new GUIContent("Quit", "Button2"));        //通过GUI的布局管理器绘制一个按钮Button2
        
        if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip)
        {
            //对当前事件进行判定
            if (lastTooltip != "")                                  //若lastTooltip不为空，则发送消息
                SendMessage(lastTooltip + "OnMouseOut", SendMessageOptions.DontRequireReceiver);
            if (GUI.tooltip != "")                                  //若lastTooltip为空，则发送消息
                SendMessage(GUI.tooltip + "OnMouseOver", SendMessageOptions.DontRequireReceiver);
            lastTooltip = GUI.tooltip;                              //将lastTooltip的值置为GUI.tooltip
        }
    }
    void Button1OnMouseOver()
    {                                      
        Debug.Log("Play game got focus");                          
    }
    void Button2OnMouseOut()
    {                                       
        Debug.Log("Quit lost focus");                              
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
