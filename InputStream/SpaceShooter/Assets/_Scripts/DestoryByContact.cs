using UnityEngine;
using System.Collections;

public class DestoryByContact : MonoBehaviour {
    public GameObject explosion;
    public GameObject playerExplosion;
    //杀一个加几分
    public int scoreValue=10;
    //脚本控制类的对象
    public GameController gameController;
    void Start()
    {
        //获取脚本控制类
        gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
    }
    //处理敌人和物体的碰撞
    void OnTriggerEnter(Collider other)
    {
        if(other.tag==Tags.Boundary)
        {
            return;
        }
        //初始化爆炸特效
        GameObject.Instantiate(explosion, transform.position, transform.rotation);
        if(other.tag==Tags.Bolt)
        {
            //加分
            gameController.AddScore(scoreValue);
        }
        //如果爆炸的是Player和小行星
        if(other.tag==Tags.Player)
        {
            GameObject.Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            //跳出游戏结束
            gameController.GameOver();
        }
        //销毁碰撞体，然后销毁自身
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
