using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {

    private bool IsPress;//判断是否按下虚拟杆
    private Transform button;
    public static float h = 0;
    public static float v = 0;
    void Awake()
    {
        button = transform.Find("jy");
    }
    void OnPress(bool isPress)
    {
        this.IsPress = isPress;
        if (isPress == false)
        {
            button.localPosition = Vector3.zero;
            h = 0;
            v = 0;
        }

    }
    void Update()
    {
        if (IsPress)
        {
            Vector2 touchPos = UICamera.lastTouchPosition;//获得以左下角为原点的触碰点向量
            touchPos -= new Vector2(91, 91);//向量代换，将其替换为中间Button为原点的坐标系

            float distance = Vector2.Distance(Vector2.zero, touchPos);//计算Button坐标系中，原点和触碰点的距离
            //如果越界
            if (distance > 73)
            {
                touchPos = touchPos.normalized * 73;
            }
            //更改button位置
            button.localPosition = touchPos;
            //更新位置参数
            h = touchPos.x / 73;
            v = touchPos.y / 73;
        }
    }

}
