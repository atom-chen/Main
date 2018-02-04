using UnityEngine;
using System.Collections;

public class TextField : MonoBehaviour {
    public string stringToEdit = "账号";                                             //声明一个字符串
    void OnGUI()                                                                            //声明OnGUI方法 
    {
        stringToEdit = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 10,         //绘制一个单行文本编辑框
            Screen.width / 3, Screen.height / 10), stringToEdit, 25);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
