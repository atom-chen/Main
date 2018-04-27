using UnityEngine;
using System.Collections;
//FocusControl：用于在当前焦点处通过键盘输入值
public class FocusControl : MonoBehaviour {
    public string username = "username";                                    //声明一个内容为username的字符串username
    public string pwd = "a pwd";                                            //声明一个内容为a pwd的字符串pwd
    void OnGUI()
    {   
        //给下一步控制事件命名MyTextField                                                
        GUI.SetNextControlName("MyTextField"); 
                             
        //绘制一个单行文本编辑框，并将字符串username的内容赋给它
        username = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 3, Screen.height / 10), username);
        //绘制一个单行文本编辑框，并将字符串pwd的内容赋给它
        pwd = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 4, Screen.width / 3, Screen.height / 10), pwd);
        
        //绘制一个名字为Move Focus的按钮，并判定按钮是否被按下
        if (GUI.Button(new Rect(Screen.width / 10, Screen.height * 2 / 5, Screen.width / 6, Screen.height / 10), "Move Focus"))
            GUI.FocusControl("MyTextField");
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
