using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

  public WheelCollider m_FLWheel;
  public WheelCollider m_FRWheel;
  private float m_MotorTorque = 800;
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
  private Vector3 m_SkidDir;

  private float m_MaxSpeed=250;
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
      return m_AvgSpeed/2;
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
      m_MotorTorque = 200;
      transform.Translate(m_SkidDir/3);
      if(!m_Skid.isPlaying)
      {
        m_Skid.Play();
      }
    }
    else
    {
      m_SkidDir = Vector3.zero;
      m_MotorTorque = 800;
      if (m_Skid.isPlaying)
      {
        m_Skid.Stop();
      }
    }
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
      Vector3 rote = Vector3.Normalize((Input.GetAxis("Horizontal") * transform.right));
      //漂移偏移角(惯性方向和漂移方向的夹角)
      float angle = Mathf.Acos(Vector3.Dot(rote, Vector3.Normalize(transform.forward)));
      //拿到对原本方向的偏移
      //m_SkidDir = Quaternion.Euler(;

      Debug.Log("按下漂移");
    }
    else
    {
      Debug.Log("松开漂移");
    }
  }

  
  
}
