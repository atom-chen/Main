using UnityEngine;
using System.Collections;                                                   //导入系统类

public class GUIBox : MonoBehaviour {
    void OnGUI(){                                                           //声明OnGUI方法
            //在屏幕的自定义范围内绘制一个内容为This is a title的Box控件
        GUI.Box(new Rect(Screen.width / 5, Screen.height / 5, Screen.width / 2, Screen.height / 2), "This is a title");    
    }
}
