using UnityEngine;
using System.Collections;
//实现设置全局GUI颜色（背景、文本……）
//color：整体颜色
public class color : MonoBehaviour {

    public float width = 0;//屏幕宽
    public float height = 0;//屏幕高
    // Use this for initialization
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        //设置颜色
        GUI.color = Color.blue;
        //画一个标签
        GUI.Label(new Rect(width / 10, height / 10, width / 5, height / 10), "Hello,World!");
        //画一个盒子
        GUI.Box(new Rect(width / 10, height / 5, width / 5, height / 10), "A Box");
        //画一个按钮
        GUI.Button(new Rect(width / 10, height / 2, width / 5, height / 10), "A Button");
    }
}
