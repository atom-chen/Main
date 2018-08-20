using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {
    public Texture btnTexture;                                                  //声明一个2D纹理图片    
    void OnGUI()
    {                                                               //声明OnGUI方法
        if (!btnTexture)
        {                                                       //判断是否存在纹理图片
            Debug.LogError("Please assign a texture on the inspector");         //若不存在，打印提示消息
            return;
        }
        //创建一个纹理按钮，并进行是否执行按钮操作的判定
        if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 10, Screen.width / 10), btnTexture))
       
            Debug.Log("Clicked the button with an image");                      //若单击按钮，则打印提示信息
        
        //创建一个文本按钮，并进行是否执行按钮操作的判定
        if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 3, Screen.width / 5, Screen.height / 10), "Click"))
           
            Debug.Log("Clicked the button with text");                          //若单击按钮，则打印提示信息

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
