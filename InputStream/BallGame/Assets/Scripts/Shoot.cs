using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
    //触点
    Vector2 touchPos;
    //足球位置
    Vector2 ballPos;
    //发起标志位
    private bool shootFlag = false;
    //手指落下的位置
    Vector2 beginPos;
    //是否开始计时
    private bool jishi=false;
    //手指操纵时间
    private float touchTime = 0;
    //屏幕高度的1/4
    private float D = Mathf.Pow(Screen.height / 10, 2);
    //足球游戏对象
    public GameObject footBall;
    //足球的刚体组件
    private Rigidbody ballRigibody;
    //摄像机游戏对象
    public GameObject MyCamera;
    //重置标志位
    private bool resetFlag = false;
    //重置时间
    private float resetTime = 0;
    //守门员游戏对象
    public GameObject goalie;
    //各类游戏对象Prafab
    public GameObject ballPrefab;
    public GameObject goaliePrefab;

    //起始点坐标
    private float starX = 0;
    private float starY = 0;
    //移动距离
    private float moveX;
    //触摸次数
    private int touchCount;

	// Use this for initialization
	void Start () {
        resetFlag = false;
        touchTime = 0;
        ballPos = new Vector2(GetComponent<Camera>().WorldToViewportPoint(footBall.transform.position).x * Screen.width,
            GetComponent<Camera>().WorldToViewportPoint(footBall.transform.position).y * Screen.height);//球在屏幕的位置
        shootFlag = false;
        Staticer.animationName = null;
        BoxCollider b = goalie.GetComponentInChildren<BoxCollider>();
        b.size = new Vector3(0.2f, 0.1f, 0.1f);
        touchCount = 0;
        jishi = false;//计时标志位
	}
	
	// Update is called once per frame
	void Update () {
        //判断是否摸到球
        foreach(Touch t in Input.touches)
        {
            touchPos = t.position;
            //如果刚刚开始触摸
            if(t.phase==TouchPhase.Began)
            {
                touchCount = 0;
                //如果没有摸到球（x坐标差的平方加上y坐标差的平方之和大于60000）
                if(Mathf.Pow(touchPos.x-ballPos.x,2)+Mathf.Pow(touchPos.y-ballPos.y,2)>60000)
                {
                    shootFlag = false;
                }
                else
                {
                    //可以发射
                    beginPos = touchPos;
                    shootFlag = true;
                    jishi = true;
                }
            }
            //如果处于触摸中
            if(t.phase==TouchPhase.Moved)
            {
                //如果可以发射
                if(shootFlag)
                {
                    //如果可以计时
                    if(jishi)
                    {
                        //记录手指触摸时间
                        touchTime += Time.deltaTime;
                    }
                    //计算滑动距离
                    float L = Mathf.Pow(t.position.x - beginPos.x, 2) + Mathf.Pow(t.position.y - beginPos.y, 2);
                    {
                        //如果滑动距离大于屏幕高度的1/4
                        if(L>D)
                        {
                            jishi = false;
                            //将滑动距离限制为D，计算其滑动速度
                            float V = D / touchTime;
                            //如果滑动速度超过限制
                            if(V>100000)
                            {
                                //可以发球
                                shootFlag = true;
                                //得到滑动分量
                                float dX = t.position.x - beginPos.x;
                                float dY = t.position.y - beginPos.y;
                                //计算飞行时间，并将其限制在15-17之间
                                float T = 14 / Mathf.Clamp((V - 100000) / 100000 + 13, 15, 17);
                                //计算落点
                                Vector2 endPoint = GetendPoint(dX, dY);
                                //计算x,y,z分速度
                                float Vx = Mathf.Clamp((endPoint.x - Staticer.ballPos2.x) / T, -3, 3);
                                float Vy = Mathf.Clamp((endPoint.y - Staticer.ballPos2.y+1/2*9.8f*T*T) / T+2.5f, 4, 6);
                                float Vz = Mathf.Clamp((V - 100000) / 100000 + 13, 15, 17);
                                //修正射门高度
                                if(Vx>2.5f || Vy<5f)
                                {
                                    if(Random.Range(0,10)<9)
                                    {
                                        Vy = Random.Range(4.5f, 5.5f);
                                    }
                                }
                                //发球
                                ballRigibody.velocity = MyCamera.transform.InverseTransformDirection(new Vector3(Vx, Vy, Vz));
                                //关闭射门标志位
                                shootFlag = false;
                                //开始重置标志位
                                resetFlag = true;

                                //如果射门开始
                                if(resetFlag)
                                {
                                    resetTime += Time.deltaTime;
                                    //等待2秒重置
                                    if(resetTime>=2)
                                    {
                                        Staticer.animationName = null;
                                        resetTime = 0;
                                        resetFlag = false;
                                        //回收
                                        Destroy(footBall);
                                        Destroy(this.goalie);
                                        //摆放摄像机
                                        MyCamera.transform.position = Staticer.cameraPosMid;
                                        MyCamera.transform.rotation = Quaternion.Euler(Staticer.cameraRotMid);
                                        //实例化球
                                        footBall = (GameObject)GameObject.Instantiate(ballPrefab, Staticer.ballPosMid, Quaternion.identity);
                                        //实例化守门员
                                        goalie =(GameObject) GameObject.Instantiate(goaliePrefab, Staticer.playerPos, Quaternion.identity);
                                        //重置守门员坐标
                                        Staticer.endPoint = Staticer.playerPos;
                                        //自动重新发球
                                        Start();
                                    }
                                }
                               

                            }
                        }
                    }
                }
            }

        }
	}
    public Vector2 GetendPoint(float dX,float dY)
    {
        return new Vector2(dX / 80, dY / 80);
    }
}
