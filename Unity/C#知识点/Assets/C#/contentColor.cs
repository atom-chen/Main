using UnityEngine;
using System.Collections;

public class contentColor : MonoBehaviour {
//实现改变所有文本颜色
//contemtColor：对图形用户界面组件中的文本进行着色
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
        //设置文字颜色
        GUI.contentColor = Color.red;
        //画一个Button
        GUI.Button(new Rect(width / 10, height / 10, width / 5, width / 5), "A Button");
    }
}
