﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

  public WheelCollider m_FLWheel;
  public WheelCollider m_FRWheel;
  private float m_MotorTorque = 400;
  private float m_SteerAngle = 100;

  public Transform m_CenterOfMass;//质心

  float m_Speed = 0;//速度

  float m_MoveVertical = 0;
  float m_MoveHozizontal = 0;

  float k = -1f;
  float b=100;
  public float Speed
  {
    get
    {
      return m_Speed;
    }
  }
  void Start()
  {
    this.GetComponent<Rigidbody>().centerOfMass = m_CenterOfMass.localPosition;
  }
  void Update()
  {
    m_MoveVertical = Input.GetAxis("Vertical")*m_MotorTorque;
    m_MoveHozizontal = Input.GetAxis("Horizontal")*m_SteerAngle;

    OnRunForward();
    OnTurn();

    m_Speed = ((m_FLWheel.rpm) * (m_FLWheel.radius * 2 * Mathf.PI) * 60 / 1000 + (m_FRWheel.rpm) * (m_FRWheel.radius * 2 * Mathf.PI) * 60 / 1000)/2.0f;
    if (m_Speed <= 90)
    {
      m_SteerAngle = k * m_Speed + b;
    }
    else
    {
      m_SteerAngle = 10;
    }
  }

  void OnRunForward()
  {
    m_FLWheel.motorTorque = m_MoveVertical;
    m_FRWheel.motorTorque = m_MoveVertical;
  }

  void OnTurn()
  {
    m_FLWheel.steerAngle = m_MoveHozizontal;
    m_FRWheel.steerAngle = m_MoveHozizontal;
  }



  //public Transform m_CenterOfMass;//质心
  //private float m_Throttle = 0;//油门大小
  //private float m_Steer = 0;//方向盘转向大小
  //private Rigidbody m_RB;

  //private float m_TopSpeed = 160;//赛车最高时速
  //private const float m_MinTurn = 10;//赛车转弯时最小转向系数
  //private const float m_MaxTurn = 15;//赛车最大转向系数

  //void Start()
  //{
  //  m_RB = this.GetComponent<Rigidbody>();
  //  if (m_CenterOfMass != null)
  //  {
  //    m_RB.centerOfMass = m_CenterOfMass.localPosition;//显示指定质心位置
  //  }
  //  m_TopSpeed = Utils.MilePerHour2MeterPerSecond(m_TopSpeed);
  //}

  //void Update()
  //{
  //  m_Throttle = Input.GetAxis("Vertical");
  //  m_Steer = Input.GetAxis("Horizontal");
  //}

  ////根据速度大小算出转向系数
  //float SpeedToTurn(float speed)
  //{
  //  float threshold = m_TopSpeed / 2;//根据速度计算转向系数的阈值
  //  if (speed > threshold)
  //  {
  //    return m_MinTurn;//较高速度时使用较小的转向系数
  //  }
  //  float speedIndex = 1 - (speed / threshold);
  //  return m_MinTurn + speedIndex * (m_MaxTurn - m_MinTurn);//速度越小，转向系数越大
  //}

  ////赛车转向控制
  //void ApplySteering(Vector3 rv)
  //{
  //  float turnRadius = (float)(3.0 / Mathf.Sin((90 - (m_Steer * 30)) * Mathf.Deg2Rad));//计算转弯时的半径
  //  float minMaxTurn = SpeedToTurn(m_RB.velocity.magnitude);//根据速度拿到转向系数
  //  float turnSpeed = Mathf.Clamp(rv.z / turnRadius, -minMaxTurn / 10, minMaxTurn / 10);//计算转弯时的速度
  //  transform.RotateAround(transform.position + transform.right * turnRadius * m_Steer, transform.up, turnSpeed * Mathf.Rad2Deg * Time.deltaTime * m_Steer);//当方向键向右时,steer为正->赛车右转 反之左转
  //}
  ////对刚体对象施加力
  //void ApplyThrottle()
  //{
  //  m_RB.AddForce(transform.forward * 20 * m_Throttle * Time.deltaTime * m_RB.mass);
  //}

  //void FixedUpdate()
  //{
  //  Vector3 rv = transform.InverseTransformDirection(m_RB.velocity);//拿到car在局部坐标系的运动速度
  //  ApplySteering(rv);
  //  ApplyThrottle();
  //}
}
