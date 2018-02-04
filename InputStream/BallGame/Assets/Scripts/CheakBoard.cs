using UnityEngine;
using System.Collections;

public class CheakBoard : MonoBehaviour {
    public GameObject ball;
    public Animator player;
    //球的刚体组件
    public Rigidbody ballRigidbody;


	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("ball");
        ballRigidbody = ball.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(ball==null)
        {
            ball = GameObject.FindGameObjectWithTag("ball");
            if(ballRigidbody==null)
            {
                ballRigidbody=ball.GetComponent<Rigidbody>();
            }
        }
        //如果已经碰撞
        if(Staticer.isCrash)
        {
            //防止二次碰撞
            Staticer.isCrash = false;
            //球到网的时间
            float t = Staticer.distance / ballRigidbody.velocity.z;
            //储存球到球门时的时间
            Staticer.timeToGoal = t;
            //到达点的坐标
            float x = Staticer.crashPoint.x + ballRigidbody.velocity.x * t;
            float y = Staticer.crashPoint.y + ballRigidbody.velocity.y * t - 0.5f * 9.8f * t * t;
            //防止落入地下
            y = (y < 0) ? 0 : y;
            //储存落点坐标
            Staticer.endPoint = new Vector3(x, y, Staticer.endZ);
            //设置为已经储存
            Staticer.isSave = true;
            //判断进球位置
            if(y>Staticer.goalStartY+Staticer.goalHeight/2 && y<Staticer.goalEndY)
            {
                //判断落点区域
                if(x>=Staticer.goalStartX && x<=Staticer.goalStartX+Staticer.goalWidth/3)
                {
                    Staticer.isPlay = true;
                    Staticer.animationName = "leftUpCorner";
                }
                else if(x>Staticer.goalStartX+Staticer.goalWidth/3 && x<=Staticer.goalStartX+Staticer.goalWidth/2)
                {
                    Staticer.isPlay = true;
                    Staticer.animationName = "leftUp";
                }
                else if(x>Staticer.goalStartX+Staticer.goalWidth/2 && x<=Staticer.goalStartX+Staticer.goalWidth/3*2)
                {
                    Staticer.isPlay = true;
                    Staticer.animationName = "rightUp";
                }
                else if (x > Staticer.goalStartX + Staticer.goalWidth / 3 * 2 && x <= Staticer.goalEndX)
                {
                    Staticer.isPlay = true;
                    Staticer.animationName = "rightUpCorner";
                }
                else if (y < Staticer.goalStartY + Staticer.goalHeight / 2)
                {
                    if (x >= Staticer.goalStartX && x <= Staticer.goalStartX + Staticer.goalWidth / 5 * 2)
                    {
                        Staticer.isPlay = true;
                        Staticer.animationName = "leftDown";
                    }
                    else if (x > Staticer.goalStartX + Staticer.goalWidth / 5 * 2 && x <= Staticer.goalStartX + Staticer.goalWidth / 5 * 3)
                    {
                        Staticer.isPlay = true;
                        Staticer.animationName = "down";
                    }
                    else if (x > Staticer.goalStartX + Staticer.goalWidth / 5 * 3 && x <= Staticer.goalEndX)
                    {
                        Staticer.isPlay = true;
                        Staticer.animationName = "rightDown";
                    }
                }
            }
        }
	}
    //如果进入触发器
    void OnTriggerEnter()
    {
        if(!Staticer.isCrash && ball!=null)
        {
            Staticer.isCrash = true;
            //记录球的位置
            Staticer.crashPoint = ball.transform.position;
        }
    }
}
