using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
 * 单个子弹脚本
 * 
 * 
 */ 
public class Bolt : MonoBehaviour {
    public Transform m_Trans;

	void Start () {
        Rigidbody rb =this.GetComponent<Rigidbody>();
        if(rb!=null)
        {
            rb.velocity =new Vector3(0,0,8);
        }
	}

    public void SetParent(Transform parent)
    {
        m_Trans.parent = parent;
        m_Trans.localPosition = new Vector3(0, 0, 0);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Boundary")
        {
           //放回到父物体
           SetParent(BoltPool.Instance.transform);
           this.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.TAG_ENEMY)
        {
            SetParent(BoltPool.Instance.transform);
            this.gameObject.SetActive(false);
        }
    }
}
