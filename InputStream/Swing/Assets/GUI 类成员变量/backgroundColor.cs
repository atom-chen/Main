using UnityEngine;
using System.Collections;
//实现改变GUI渲染的所有背景颜色
//background：用于设置总体上图形用户界面的背景颜色
public class backgroundColor : MonoBehaviour {

    public float width = 0;//屏幕宽
    public float height = 0;//屏幕高
    // Use this for initialization
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnGUI()
    {
        //设置背景颜色
        GUI.backgroundColor = Color.cyan;
        //画一个按钮
        GUI.Button(new Rect(width / 10, height / 10, width / 5, height / 5),"A Button");
    }
}
