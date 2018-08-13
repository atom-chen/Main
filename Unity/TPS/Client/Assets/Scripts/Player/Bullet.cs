using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private static float damage=20;//子弹伤害
    private static float speed=2000;//子弹速度
    Transform trans;
    private Vector3 m_Dir;

    private Rigidbody rb;

	void OnEnable () 
    {
        if(trans==null)
        {
            trans = transform;
        }
        m_Dir = trans.forward.normalized;
        if (rb == null)
        {
            rb = this.GetComponent<Rigidbody>();
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(speed * m_Dir);
	}

    //void Update()
    //{
    //    trans.position += speed * m_Dir * Time.deltaTime;
    //}
    

}
