using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
    //Box：图形化盒子
    void OnGUI()
    {                                                           //声明OnGUI方法
        //在屏幕的自定义范围内绘制一个内容为This is a title的Box控件
        GUI.Box(new Rect(Screen.width / 5, Screen.height / 5, Screen.width / 2, Screen.height / 2), "This is a title");
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
