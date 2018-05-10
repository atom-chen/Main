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

  private bool m_IsSkid = false;//是否漂移
  private Vector3 m_RotateDir;//转弯角
  private Vector3 m_Inertia;//惯性方向

  private float m_MaxSpeed=130;
  public float MaxSpeed
  {
    get
    {
      return m_MaxSpeed;
    }
  }
  private float m_MinSpeed=30;

  private bool m_IsShowDowm = false;//是否刹车
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
    m_Driver.pitch = 0.5f + (m_AvgSpeed / m_MaxSpeed);
    if(!m_Driver.isPlaying)
    {
      m_Driver.Play();
    }
    
    if(m_IsShowDowm)
    {
      m_FLWheel.motorTorque = 0;
      m_FRWheel.motorTorque = 0;
      m_FLWheel.brakeTorque = 400;
      m_FRWheel.brakeTorque = 400;
    }
    //是否需要限速
    else if ((m_Speed>m_MaxSpeed &&m_MoveVertical>0) ||(m_Speed<-m_MinSpeed && m_MoveVertical<0))
    {
      m_FLWheel.motorTorque = 0;
      m_FRWheel.motorTorque = 0;
      m_FLWheel.brakeTorque = 0;
      m_FRWheel.brakeTorque = 0;
    }
    //全速前进
    else
    {
      m_FLWheel.motorTorque = m_MoveVertical;
      m_FRWheel.motorTorque = m_MoveVertical;
      m_FLWheel.brakeTorque = 0;
      m_FRWheel.brakeTorque = 0;
    }
  }

  void OnTurn()
  {
    if(m_Speed<=30)
    {
      m_SteerAngle = 60;
    }
    else if(m_Speed<=80)
    {
      m_SteerAngle = 20;
    }
    else
    {
      m_SteerAngle = 5;
    }
    m_FLWheel.steerAngle = m_MoveHozizontal;
    m_FRWheel.steerAngle = m_MoveHozizontal;

    if(m_IsSkid)
    {
      transform.Translate(m_RotateDir);
      if(!m_Skid.isPlaying)
      {
        m_Skid.Play();
      }
    }
    else
    {
      if (m_Skid.isPlaying)
      {
        m_Skid.Stop();
      }
    }


    ////如果当前处于漂移状态
    //if(m_IsSkin)
    //{
    //  if(Mathf.Abs(m_FLWheel.steerAngle)>10 && m_Speed>10)
    //  {
    //    if(!m_Skid.isPlaying)
    //    {
    //      m_LeftSmoke.emit = true;
    //      m_RightSmoke.emit = true;
    //      m_Skid.Play();
    //    }
    //  }
    //  else
    //  {
    //    if (m_Skid.isPlaying)
    //    {
    //      m_LeftSmoke.emit = false;
    //      m_RightSmoke.emit = false;
    //      m_Skid.Stop();
    //      m_IsSkin = false;
    //    }
    //  }
    //}
    ////位移校测
    //else if(Mathf.Abs(m_FLWheel.steerAngle)>30 && m_Speed>50)
    //{
    //  bool isLeftHitGround=false;
    //  bool isRightHitGround = false;
    //  WheelHit hit;
    //  //左轮检测
    //  if(m_FLWheel.GetGroundHit(out hit))
    //  {
    //    isLeftHitGround = true;
    //    m_LeftSmoke.emit = true;
    //  }
    //  else
    //  {
    //    m_LeftSmoke.emit = false;
    //    isLeftHitGround = false;
    //  }

    //  //右轮检测
    //  if(m_FRWheel.GetGroundHit(out hit))
    //  {
    //    m_RightSmoke.emit = true;
    //    isRightHitGround = true;
    //  }
    //  else
    //  {
    //    m_RightSmoke.emit = false;
    //    isRightHitGround = false;
    //  }

    //  //根据计算的着陆情况判断是否处于漂移
    //  if ((isRightHitGround || isLeftHitGround))
    //  {
    //    m_IsSkin = true;
    //  }
    //  else if ((!isRightHitGround && !isLeftHitGround))
    //  {
    //    m_IsSkin = false;
    //  }
    //  else
    //  {
    //    m_IsSkin = true;
    //  }
    //}
    //else
    //{
    //  m_LeftSmoke.emit = false;
    //  m_RightSmoke.emit = false;
    //  m_IsSkin = false;
    //}
  }

  //按下/松开刹车
  public void ShotDownCar()
  {
    Debug.Log("按下刹车");
    m_IsShowDowm = !m_IsShowDowm;
  }

  //按下/松开漂移
  public void Skid()
  {
    m_IsSkid = !m_IsSkid;
    if(m_IsSkid)
    {
      //要处理的方向
      Vector3 rote =(m_MoveHozizontal / m_SteerAngle *transform.right);
      //漂移偏移角
      float angle = Mathf.Acos(Vector3.Dot(Vector3.Normalize(rote), Vector3.Normalize(transform.forward))*Mathf.Deg2Rad);
      m_RotateDir = rote;
      Debug.Log("按下漂移");
    }
    else
    {
      Debug.Log("松开漂移");
    }
  }

  
  
}
