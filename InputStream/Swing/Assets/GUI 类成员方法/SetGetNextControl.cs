using UnityEngine;
using System.Collections;
//SetNextControlName：给下一步控制设置事件名字
//GetNameOfFousedControl：得到当前控制焦点的名字
//组合使用：通过得到当前控制焦点的名字来执行下一步的事件
public class SetGetNextControl : MonoBehaviour {
    public string login = "username";                                       //声明一个内容为username的字符串login
    public string login2 = "no action here";                                //声明一个内容为no action here的字符串login2
    void OnGUI()                                                            //声明OnGUI方法
    {
        //设置下一步控制事件的名字为user
        GUI.SetNextControlName("user");
        //绘制一个单行文本编辑框
        login = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 3, Screen.height / 10), login);
        //绘制一个单行文本编辑框
        login2 = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 3, Screen.width / 3, Screen.height / 10), login2);
       
        if (Event.current.Equals(Event.KeyboardEvent("return")) &&      //判断当前事件是否为键盘事件return
            GUI.GetNameOfFocusedControl() == "user")                     //并且得到的当前事件名字是否为user
            Debug.Log("Login");                                         //打印提示信息Login
        //在自定义的矩形区域绘制一个按钮
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Login"))
            Debug.Log("Login");                                         //打印提示信息

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
