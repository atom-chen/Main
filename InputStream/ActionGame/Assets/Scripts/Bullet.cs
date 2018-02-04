using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float speed = 10;
    public float attack=100;
	// Use this for initialization
	void Start () {
        //自动销毁
        Destroy(this.gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
        //向前运动
        transform.Translate(Vector3.forward * speed*Time.deltaTime);  
	}
    //触发检测
    void OnTriggerEnter(Collider col)
    {
        if(col.tag==Tags.soulBoss || col.tag==Tags.soulMonst)
        {
            col.GetComponent<ATKAndDamage>().TakeDamage(attack);
        }
    }
}
