using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag=="Bullet")
        {
            BoomParticalPool.Instance.PlayBoomPartical(other.transform.position);
            BulletPool.Instance.GCBullet(other.transform);
        }
    }
}
