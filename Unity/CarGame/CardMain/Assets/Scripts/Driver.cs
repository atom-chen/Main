using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

  public WheelCollider m_FLWheel;
  public WheelCollider m_FRWheel;
  private float m_MotorTorque = 400;
  private float m_SteerAngle = 30;

  public Transform m_CenterOfMass;//质心

  public ParticleEmitter m_LeftSmoke;
  public ParticleEmitter m_RightSmoke;
  public AudioSource m_Skid;
  public AudioSource m_Driver;

  float m_Speed = 0;//速度
  private LinkList m_SpeedList=new LinkList();
  float m_AvgSpeed = 0;

  float m_MoveVertical = 0;
  float m_MoveHozizontal = 0;

  float k = -1.25f;
  float b=100;

  private bool m_IsSkin = false;

  private float m_MaxSpeed=130;
  public float MaxSpeed
  {
    get
    {
      return m_MaxSpeed;
    }
  }
  private float m_MinSpeed=30;

  private bool m_IsShowDowm = false;
  public float Speed
  {
    get
    {
      return m_AvgSpeed;
    }
  }
  void Start()
  {
    this.GetComponent<Rigidbody>().centerOfMass = m_CenterOfMass.localPosition;
  }
  void Update()
  {
    m_Speed = (m_FLWheel.rpm) * (m_FLWheel.radius * 2 * Mathf.PI) * 60 / 1000;
    m_SpeedList.Add(m_Speed);
    m_AvgSpeed = m_SpeedList.GetSpeed();
    m_MoveVertical = Input.GetAxis("Vertical")*m_MotorTorque;//向前速度
    m_MoveHozizontal = Input.GetAxis("Horizontal")*m_SteerAngle;//转向
    OnRunForward();
    OnTurn();

  }

  void OnRunForward()
  {
    //刹车检测
    if((m_Speed>0 && m_MoveVertical<0) || (m_Speed<0 && m_MoveVertical>0))
    {
      Debug.Log("玩家在刹车");
      m_IsShowDowm = true;
    }
    else
    {
      Debug.Log("玩家在前进");
      m_IsShowDowm = false;
    }
    m_Driver.pitch = 0.5f + (m_AvgSpeed / m_MaxSpeed);
    if(!m_Driver.isPlaying)
    {
      m_Driver.Play();
    }
    //速度检测
    if ((m_Speed>m_MaxSpeed &&m_MoveVertical>0) ||(m_Speed<-m_MinSpeed && m_MoveVertical<0))
    {
      Debug.Log("玩家被限速");
      m_FLWheel.motorTorque = 0;
      m_FRWheel.motorTorque = 0;
    }

    else
    {
      Debug.Log("玩家在全速前进");
      m_FLWheel.motorTorque = m_MoveVertical;
      m_FRWheel.motorTorque = m_MoveVertical;
    }

    if (m_IsShowDowm)
    {
      m_FLWheel.motorTorque = 0;
      m_FRWheel.motorTorque = 0;
      m_FLWheel.brakeTorque = 40000;
      m_FRWheel.brakeTorque = 40000;
    }
    else
    {
      m_FLWheel.brakeTorque = 0;
      m_FRWheel.brakeTorque = 0;
    }
  }

  void OnTurn()
  {
    if(m_Speed<=30)
    {
      m_SteerAngle = 80;
    }
    else
    {
      m_SteerAngle = 30;
    }
    m_FLWheel.steerAngle = m_MoveHozizontal;
    m_FRWheel.steerAngle = m_MoveHozizontal;

    //如果当前处于漂移状态
    if(m_IsSkin)
    {
      if(Mathf.Abs(m_FLWheel.steerAngle)>10 && m_Speed>10)
      {
        if(!m_Skid.isPlaying)
        {
          m_LeftSmoke.emit = true;
          m_RightSmoke.emit = true;
          m_Skid.Play();
        }
      }
      else
      {
        if (m_Skid.isPlaying)
        {
          m_LeftSmoke.emit = false;
          m_RightSmoke.emit = false;
          m_Skid.Stop();
          m_IsSkin = false;
        }
      }
    }
    //位移校测
    else if(Mathf.Abs(m_FLWheel.steerAngle)>30 && m_Speed>50)
    {
      bool isLeftHitGround=false;
      bool isRightHitGround = false;
      WheelHit hit;
      //左轮检测
      if(m_FLWheel.GetGroundHit(out hit))
      {
        isLeftHitGround = true;
        m_LeftSmoke.emit = true;
      }
      else
      {
        m_LeftSmoke.emit = false;
        isLeftHitGround = false;
      }

      //右轮检测
      if(m_FRWheel.GetGroundHit(out hit))
      {
        m_RightSmoke.emit = true;
        isRightHitGround = true;
      }
      else
      {
        m_RightSmoke.emit = false;
        isRightHitGround = false;
      }

      //根据计算的着陆情况判断是否处于漂移
      if ((isRightHitGround || isLeftHitGround))
      {
        m_IsSkin = true;
      }
      else if ((!isRightHitGround && !isLeftHitGround))
      {
        m_IsSkin = false;
      }
      else
      {
        m_IsSkin = true;
      }
    }
    else
    {
      m_LeftSmoke.emit = false;
      m_RightSmoke.emit = false;
      m_IsSkin = false;
    }
  }

  public void ShotDownCar()
  {
    //减速
    m_FLWheel.motorTorque = 0;
    m_FRWheel.motorTorque = 0;
    m_IsShowDowm = true;
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
