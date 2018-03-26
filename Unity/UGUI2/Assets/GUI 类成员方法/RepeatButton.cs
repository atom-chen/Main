using UnityEngine;
using System.Collections;

public class RepeatButton : MonoBehaviour {
    public Texture btnTexture;                                                              //声明一个纹理图片
    void OnGUI()                                                                            //声明OnGUI方法
    {
        if (GUI.RepeatButton(new Rect(Screen.width / 10, Screen.height / 3,                  //绘制一个文本RepeatButton    
           Screen.width / 5, Screen.height / 10), "Click"))
            Debug.Log("Clicked the button with text");                                      //若持续按下按钮，则打印提示信息
       
        if (!btnTexture)                                                                    //判断是否存在纹理图片
        {
            Debug.LogError("Please assign a texture on the inspector");                     //若不存在，则打印提示信息
            return;
        }
        if (GUI.RepeatButton(new Rect(Screen.width / 10, Screen.height / 10,                //绘制一个纹理图片RepeatButton
            Screen.width / 10, Screen.width / 10), btnTexture))
            Debug.Log("Clicked the button with an image");                                  //若持续按下按钮，则打印提示信息

       

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
