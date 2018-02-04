using UnityEngine;
using System.Collections;
/*
 * 奖励物品的脚本
 */ 
public enum AwardType
{
    Gun,DualSword
}
public class AwardItem : MonoBehaviour {
    public AwardType type;
    private Rigidbody rigidbody;
    private bool isStartMove = false;
    public GameObject player;
    public Transform playerTrans;
    float speed = 10f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag==Tags.ground)
        {
            //小球停止运动
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            //拿到碰撞体组件
            SphereCollider col = this.GetComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 2;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag==Tags.player)
        {
            isStartMove = true;
            playerTrans = col.transform;
        }
    }

    void Update()
    {
        if(isStartMove)
        {
            //向主角运动
            this.transform.position = Vector3.Lerp(transform.position, playerTrans.position+Vector3.up, speed * Time.deltaTime);
            //如果进入主角范围，则判定为吃到了
            if(Vector3.Distance(transform.position,playerTrans.position+Vector3.up)<0.5f)
            {
                //吃到了
                player.GetComponent<PlayAWard>().SetAward(this.type);
                Destroy(this.gameObject);
            }
        }
    }
}
