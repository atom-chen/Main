using UnityEngine;
using System.Collections;

public class PasswordField : MonoBehaviour {
    public string passwordToEdit = "My Password";                   //声明一个字符串
    void OnGUI()                                                    //声明OnGUI方法
    {
        //绘制一个密码编辑框，并设置用*号来屏蔽密码，且设置密码编辑框的最大长度为25
        passwordToEdit = GUI.PasswordField(new Rect(Screen.width / 10, Screen.height / 10,
            Screen.width / 2, Screen.height / 10), passwordToEdit, "*"[0], 25);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
