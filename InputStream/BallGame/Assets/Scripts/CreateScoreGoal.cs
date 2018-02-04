using UnityEngine;
using System.Collections;

public class CreateScoreGoal : MonoBehaviour {
    //分数平面预设物体
    public GameObject scorePlane;
    //音乐播放器
    public AudioClip sound;
    private AudioSource audio;
    //足球游戏对象
    private Transform ball_transform;
    private Rigidbody ball_rigidbody;
    void Start()
    {
        GameObject footBall = GameObject.FindWithTag("ball");
        ball_transform = footBall.GetComponent<Transform>();
        ball_rigidbody = footBall.GetComponent<Rigidbody>();
        audio=GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(Staticer.STATUS!=Staticer.NORMAL)
        {
            return;
        }
        if(collider.gameObject.tag=="ball")
        {               
            //获取球的位置
            Vector2 ballPosition = new Vector2(ball_transform.position.x, ball_transform.transform.position.y);      
            //判断碰撞体
            //计算球与靶心距离的平方
            float lenghtToCenterPow = Mathf.Pow(ballPosition.x - Staticer.goalX, 2) + Mathf.Pow(ballPosition.y - Staticer.goalY, 2);
            //3环
            if(lenghtToCenterPow<=0.058f)
            {
                Staticer.STATUS = Staticer.THREEPOINT;
            }
            //2环
            else if (lenghtToCenterPow <= 0.23f)
            {
                Staticer.STATUS = Staticer.TWOPOINT;
            }
            //1环
            else if (lenghtToCenterPow < -0.051f)
            {
                Staticer.STATUS = Staticer.ONEPOINT;
            }
            else
            {
                //射中状态
                Staticer.STATUS = Staticer.GOAL;
            }
            //实例化分数
            GameObject.Instantiate(scorePlane, collider.transform.position, new Quaternion(-0.7f, 0, 0, 0.7f));
            //播放音乐
            audio.volume = Staticer.valueMusic;
            audio.PlayOneShot(sound);
            //使球不会乱飞
            ball_rigidbody.velocity = new Vector3(0, 0, 1);
        }
    }

}










