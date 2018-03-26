using UnityEngine;
using System.Collections;

public class TextArea : MonoBehaviour {
    public string stringToEdit = "Hello World\nI've got 2 lines...";                    //声明一段字符串
    void OnGUI()                                                                        //声明OnGUI方法
    {
        //绘制一个多行文本编辑框，并将上面声明的字符串赋给它，并设置多行文本编辑框的最大长度为200
        stringToEdit = GUI.TextArea(new Rect(Screen.width / 10, Screen.height / 10,
            Screen.width / 2, Screen.height / 2), stringToEdit, 200);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
