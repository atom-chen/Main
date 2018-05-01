using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
  private WheelCollider m_Wheel;
  private Transform m_Trans;
  private Transform m_GranpTrans;

  void Start()
  {
    m_Wheel = this.GetComponent<WheelCollider>();
    m_Trans = this.GetComponent<Transform>().parent;
    m_GranpTrans = m_Trans.parent;
  }
  void Update()
  {
    m_Trans.Rotate(m_Wheel.rpm * 6 * Time.deltaTime * Vector3.right);   //车轮转动

    Vector3 localEulerAngles = m_GranpTrans.localEulerAngles;
    localEulerAngles.y = m_Wheel.steerAngle/3;
    m_GranpTrans.localEulerAngles = localEulerAngles;
  } 
}
