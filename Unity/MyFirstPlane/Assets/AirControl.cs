using UnityEngine;
using System.Collections;



public class AirControl : MonoBehaviour {
    //定义变量
    private Transform m_transform;//建立一个Transform实例，用于后面存放Transform组件的调用
    public float speed = 600f;//飞机速度
    private float rotationz = 0.00f;//绕z轴旋转角度
    public float rotateSpeed_AxisZ = 45f;//绕z轴的旋转速度，
    public float rotateSpeed_AxisY = 20f;//绕y轴的旋转速度
    private Vector2 touchPosition;//触屏点的位置
    private float screenWeight;//屏幕宽度
    
	// Use this for initialization
	void Start () 
    {
        //初始化我们定义的属性
        m_transform = this.transform;//把这个对象的transform属性交给我们定义的Transform对象
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;//关闭重力影响
        screenWeight = Screen.width; //获取屏幕宽度
	}
	
	// Update is called once per frame
	void Update () 
    {
        //控制飞机基本逻辑
        //1、让飞机动起来 Translate：构造一个新的向量，代表飞机的坐标。和坦克一样，每帧更新一次x,y,z。
        m_transform.Translate(new Vector3(0, 0, speed *Time.deltaTime));
        //2、找到飞机身上的"propeller"标签（螺旋桨），使它绕着y轴旋转
        GameObject.Find("propeller").transform.Rotate(new Vector3(0, 1000f * Time.deltaTime, 0));
        //enlerAngles：物体旋转的欧拉角
        //获取飞机绕z轴的旋转量 
        rotationz = this.transform.eulerAngles.z;
        
        //输入类：Input
        //当发生触屏事件时
        if(Input.touchCount>0)
        {
            //遍历所有触点
            for(int i=0;i<Input.touchCount;i++)
            {
                //触点类：Touch
                //实例化当前触点：通过对象数组touches[下标]
                Touch touch = Input.touches[i];
                //判断触摸：通过touch.phase的值来判断
                //phase==TouchPhase.Stationary:手指在屏幕上，但没有滑动
                //phase==TouchPhase.Mover：手指在屏幕上发生了滑动
                //Ended：手指离开屏幕
                if(touch.phase==TouchPhase.Stationary || touch.phase==TouchPhase.Moved)
                {
                    //获取当前触点坐标
                    touchPosition = touch.position;
                    //如果触摸点在屏幕左半边
                    if(touchPosition.x<screenWeight/2)
                    {
                        //执行飞机左转（每秒30度）
                        m_transform.Rotate(new Vector3(0, -Time.deltaTime * 30, 0));
                    }//如果触摸点在屏幕右半边
                    else if(touchPosition.x>screenWeight/2)
                    {
                        m_transform.Rotate(new Vector3(0, Time.deltaTime * 30, 0));
                    }
                    //如果手指离开屏幕
                    else if(touch.phase==TouchPhase.Ended)
                    {
                        //恢复平衡
                        this.BackToBlance();
                    }
                }
            }
        }
	}
    private void BackToBlance()
    {
        //如果当前处于右倾
        if(rotationz<=180)
        {
            //如果右倾角度较小：制造略微晃动的感觉
            if(rotationz-0<=2)
            {
                m_transform.Rotate(0, 0, Time.deltaTime * -1);
            }//如果倾角较大
            else
            {
                m_transform.Rotate(0, 0, Time.deltaTime * -40);
            }
        }
        //如果当前处于左倾
        else if(rotationz>180)
        {
            //如果左倾角度较小：制造略微晃动的感觉
            if (360-rotationz  <= 2)
            {
                m_transform.Rotate(0, 0, Time.deltaTime * 1);
            }//如果倾角较大
            else
            {
                m_transform.Rotate(0, 0, Time.deltaTime * 40);
            }
        }
    }
}
