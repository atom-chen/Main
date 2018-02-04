using UnityEngine;
using System.Collections;

public class Label : MonoBehaviour {
    //Label：标签
    public Texture2D textureToDisplay;                              //声明一个纹理图片
    void OnGUI()                                                    //声明OnGUI方法
    {
        GUI.Label(new Rect(Screen.width / 10, Screen.height / 10,    //绘制一个文本表情
            Screen.width / 5, Screen.height / 10), "Hello World!");

        GUI.Label(new Rect(Screen.width / 10, Screen.height / 3,     //绘制一个纹理图片
            textureToDisplay.width, textureToDisplay.height), textureToDisplay);

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
