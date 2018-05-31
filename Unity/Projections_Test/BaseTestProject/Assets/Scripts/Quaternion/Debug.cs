using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour {
  private Vector3 m_Dir;
  private Transform m_Target;

	// Use this for initialization
	void Start () 
  {
    m_Target = GameObject.Find("Sphere").transform;

	}
	
	// Update is called once per frame
	void Update () 
  {
    UnityEngine.Debug.Log("e="+transform.eulerAngles+"  q="+transform.rotation);

    m_Dir = (m_Target.position - transform.position).normalized;
    Quaternion res = Quaternion.LookRotation(m_Dir);//拿到转向dir的角度
    //transform.rotation = res;
    transform.rotation = Quaternion.Lerp(transform.rotation, res, Time.deltaTime);//4元数线性插值
	}
}
