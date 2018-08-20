using UnityEngine;
using System.Collections;
//通过调用change变量来检测某些数据是否发生改变
//changed:检测任何控件输入数据的值是否发生改变

public class changed : MonoBehaviour {
    public string stringToEdit = "Modifyme";
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
    void OnGui()
    {
        //绘制一个TextField，将输入的数据复制给我们定义的字符串stringToEdit
        stringToEdit = GUI.TextField(new Rect(width / 10, height / 10, width / 4, height / 10), stringToEdit, 25);
        //调用changed，看输入数据是否变化
        if(GUI.changed)
        {
            Debug.Log("检测到TextField内容变化");
        }
    }
}
