using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
  public WheelCollider m_Wheel;
  private Transform m_WheelTrans;//自己
  private Transform m_Trans;//父物体
  private Transform m_GranpTrans;//父物体的父物体

  void Start()
  {
    if(m_Wheel==null)
    {
      m_Wheel = this.GetComponent<WheelCollider>();
    }
    m_WheelTrans = this.transform;
    m_Trans = m_WheelTrans.parent;
    m_GranpTrans = m_Trans.parent;
  }
  void Update()
  {

    RotateWheel();
    //HangSystem();
  } 
  /// <summary>
  /// 车轮转动的视觉效果
  /// </summary>
  protected void RotateWheel()
  {
    m_Trans.Rotate(m_Wheel.rpm * 6 * Time.deltaTime * Vector3.right);   //车轮转动

    Vector3 localEulerAngles = m_GranpTrans.localEulerAngles;
    localEulerAngles.y = m_Wheel.steerAngle / 90;
    m_GranpTrans.localEulerAngles = localEulerAngles;
  }

  /// <summary>
  /// 悬挂系统模拟
  /// </summary>
  protected void HangSystem()
  {
    RaycastHit hit;
    //轮胎与地面碰撞
    if (Physics.Raycast(m_WheelTrans.position, Vector3.down, out hit, m_Wheel.radius + m_Wheel.suspensionDistance))
    {
      m_GranpTrans.position = hit.point + Vector3.up * m_Wheel.radius;
    }
    //车子在空中
    else
    {
      m_GranpTrans.position = m_WheelTrans.position - m_WheelTrans.up * m_Wheel.suspensionDistance;

    }
  }
}
