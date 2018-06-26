using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {
    public List<Texture> m_Textures = new List<Texture>();
    public GameObject expEffect;
    private Transform tr;
    private int hitCount = 0;
    void Start()
    {
        tr = this.transform;

        int index = Random.Range(0, m_Textures.Count);
        MeshRenderer render = this.GetComponentInChildren<MeshRenderer>();
        if(render!=null)
        {
            render.material.mainTexture = m_Textures[index];
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.transform.tag=="Bullet")
        {
            BulletPool.Instance.GCBullet(coll.collider.gameObject);
            
            if(++hitCount>=3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject.Instantiate(expEffect, tr.position, Quaternion.identity);

        //获取半径10.0f内的Collider对象
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);
        
        foreach(Collider item in colls)
        {
            Rigidbody rbody = item.GetComponent<Rigidbody>();
            if(rbody!=null)
            {
                rbody.mass = 1;
                rbody.AddExplosionForce(1000.0f, tr.position, 10, 300);
            }
        }
        Destroy(gameObject, 5.0f);
    }
}
