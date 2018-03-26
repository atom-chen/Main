using UnityEngine;
using System.Collections;

public class DrawTexture : MonoBehaviour {
    //DrawTexture：画纹理图
    public Texture aTexture;                                                                //声明一个纹理图片
    void OnGUI()
    {                                                                          //声明OnGUI方法
        GUI.DrawTexture(new Rect(Screen.width / 10, Screen.height / 10, Screen.width / 5,     //绘制一个纹理图片
            Screen.height / 5), aTexture, ScaleMode.ScaleToFit, true, 0.0f);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
