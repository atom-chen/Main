using UnityEngine;
using System.Collections;
//实现空格切换skin变量
//skin:皮肤风格
public class skin : MonoBehaviour {

    //声明
    public GUISkin[] gskin;
    public int skin_index = 0;//皮肤数组下标
    public float width = 0;//屏幕宽
    public float height = 0;//屏幕高
	// Use this for initialization
	void Start () 
    {
        width=Screen.width;
        height=Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        //事件：如果点击了空格键
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //改变皮肤下标
            skin_index++;
            //如果+1后越界 则清零
            if(skin_index>=gskin.Length)
            {
                skin_index = 0;
            }
        }
	}

    //实现OnGUI函数：当渲染和处理GUI事件时调用
    void OnGUI()
    {
        //获取当前数组下标对应皮肤
        GUI.skin = gskin[skin_index];
        //创建按钮
        if (GUI.Button(new Rect(0, 0, width / 10, height / 10), "a button"))
        {
            //如果点击了按钮，打印信息
            Debug.Log("Button被点击了");
        }
        //创建标签
        GUI.Label(new Rect(0, height * 3 / 10, width / 10, height / 10), "a lable");
        
    }
}
