using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    private const float m_MaxTorque = 1200;
    private const float m_SkidTorque = 400;

    public WheelCollider m_FLWheel;
    public WheelCollider m_FRWheel;
    public WheelCollider m_BLWheel;
    public WheelCollider m_BRWheel;

    public Light m_LeftLight;
    public Light m_RightLight;
    public Light m_LeftSkidLight;
    public Light m_RightSkidLight;

    private float m_MotorTorque = 1200;
    private float m_SteerAngle = 30;

    public Transform m_CenterOfMass;//质心

    public ParticleEmitter m_LeftSmoke;
    public ParticleEmitter m_RightSmoke;
    public AudioSource m_Skid;
    public AudioSource m_Driver;
    public AudioSource m_Boom;

    public GameObject m_SkidMask;//划痕

    float m_Speed = 0;//速度
    private LinkList m_SpeedList = new LinkList();
    float m_AvgSpeed = 0;

    float m_MoveVertical = 0;//向前
    float m_MoveHozizontal = 0;//转向
    float input = 0;

    float k = -1.25f;
    float b = 100;

    private bool m_IsSkid = false;//是否漂移

    private float m_MaxSpeed = 250;
    public float MaxSpeed
    {
        get
        {
            return m_MaxSpeed;
        }
    }
    private float m_MinSpeed = 30;

    private bool m_IsShowDowm = false;//是否刹车
    public float Speed
    {
        get
        {
            return m_AvgSpeed / 2;
        }
    }
    void Start()
    {
        if (m_FLWheel == null)
        {
            Transform obj = transform.Find("Wheel/WheelFL/DiscBrakeFL/WheelCollider");
            if (obj != null)
            {
                m_FLWheel = GetComponent<WheelCollider>();
            }
        }
        if (m_FRWheel == null)
        {
            Transform obj = transform.Find("Wheel/WheelFR/WheelCollider");
            if (obj != null)
            {
                m_FRWheel = GetComponent<WheelCollider>();
            }
        }
        if (m_BLWheel == null)
        {
            Transform obj = transform.Find("Wheel/WheelRL/WheelCollider");
            if (obj != null)
            {
                m_FRWheel = GetComponent<WheelCollider>();
            }
        }
        if (m_BRWheel == null)
        {
            Transform obj = transform.Find("Wheel/WheelRR/WheelCollider");
            if (obj != null)
            {
                m_FRWheel = GetComponent<WheelCollider>();
            }
        }


        this.GetComponent<Rigidbody>().centerOfMass = m_CenterOfMass.localPosition;
        m_LeftLight.enabled = false;
        m_RightLight.enabled = false;
        m_LeftSkidLight.enabled = false;
        m_RightSkidLight.enabled = false;
    }
    public void SetInput()
    {

    }
    void Update()
    {
        m_Speed = (m_FLWheel.rpm) * (m_FLWheel.radius * 2 * Mathf.PI) * 60 / 1000;
        m_SpeedList.Add(m_Speed);
        m_AvgSpeed = m_SpeedList.GetSpeed();

        //PC端输入
#if UNITY_EDITOR || WINDOWS
        input = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");
        m_MoveHozizontal = input * m_SteerAngle;
        //m_MoveVertical = (1 - input) < 0.5f ? 0.5f * m_MotorTorque : (1 - input) * m_MotorTorque;
        m_MoveVertical = forward * m_MotorTorque;

#endif
        //移动端输入
#if (UNITY_ios || UNITY_ANDROID)
   input = Input.acceleration.x;
   m_MoveHozizontal = input*m_SteerAngle;
   m_MoveVertical = (1 - input) * m_MotorTorque;
#endif
        m_MoveHozizontal = Joystick.h * m_SteerAngle;
        m_MoveVertical = Joystick.v * m_MotorTorque;
        OnRunForward();

    }

    void FixedUpdate()
    {
        OnTurn();
    }

    void OnRunForward()
    {
        m_Driver.pitch = 0.5f + (m_AvgSpeed / m_MaxSpeed);
        if (!m_Driver.isPlaying)
        {
            m_Driver.Play();
        }
        //倒车检测
        HeadBackTest();
        //按下刹车键
        if (m_IsShowDowm)
        {
            m_FLWheel.motorTorque = 0;
            m_FRWheel.motorTorque = 0;
            m_FLWheel.brakeTorque = 400;
            m_FRWheel.brakeTorque = 400;
        }
        //是否需要限速
        else if ((m_Speed > m_MaxSpeed && m_MoveVertical > 0) || (m_Speed < -m_MinSpeed && m_MoveVertical < 0))
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
        //限制旋转
        if (m_Speed <= 30)
        {
            m_SteerAngle = 60;
        }
        else if (m_Speed <= 80)
        {
            m_SteerAngle = 20;
        }
        else
        {
            m_SteerAngle = 5;
        }
        if (!m_IsSkid)
        {
            m_FLWheel.steerAngle = m_MoveHozizontal;
            m_FRWheel.steerAngle = m_MoveHozizontal;
        }
        OnSkid();

    }

    //按下/松开刹车
    public void OnClickShotDownCar()
    {
        Debug.Log("按下刹车");
        m_IsShowDowm = !m_IsShowDowm;
    }

    //按下/松开漂移
    public void OnClickSkid()
    {
        m_IsSkid = !m_IsSkid;
    }

    /// <summary>
    /// 漂移中持续计算方向
    /// </summary>
    private void OnSkid()
    {
        //漂移处理
        if (m_IsSkid)
        {
            float dir = input;
            //要处理的方向
            Vector3 rote = dir * (transform.right);
            //前方向
            Vector3 forward = transform.forward;
            //rote = Vector3.Slerp(forward, rote, (m_MaxSpeed/Speed)*Time.deltaTime);
            //漂移偏移角(惯性方向和漂移方向的夹角)
            float angle = Mathf.Acos(Vector3.Dot(Vector3.Normalize(rote), Vector3.Normalize(forward)));
            ////拿到对原本方向的偏移
            transform.Rotate(0, input * 3 * m_Speed / MaxSpeed, 0);


            //漂移灯亮
            if (dir > 0)
            {
                m_RightLight.enabled = true;
                m_LeftLight.enabled = false;
            }
            else
            {
                m_RightLight.enabled = false;
                m_LeftLight.enabled = true;
            }
            m_MotorTorque = m_SkidTorque;
            if (!m_Skid.isPlaying)
            {
                m_Skid.Play();
            }
            SkidEffect();
        }
        else
        {
            m_MotorTorque = m_MaxTorque;
            if (m_Skid.isPlaying)
            {
                m_Skid.Stop();
            }
            m_RightLight.enabled = false;
            m_LeftLight.enabled = false;
            m_LeftSmoke.emit = false;
            m_RightSmoke.emit = false;
        }

    }

    //倒车检测
    private void HeadBackTest()
    {
        //亮灯
        if (Speed < 0)
        {
            m_LeftSkidLight.enabled = true;
            m_RightSkidLight.enabled = true;
        }
        else
        {
            m_LeftSkidLight.enabled = false;
            m_RightSkidLight.enabled = false;
        }

    }


    private Vector3 m_PreLeftSkidPos = Vector3.zero;
    private Vector3 m_PreRightSkidPos = Vector3.zero;
    /// <summary>
    /// 播放漂移特效
    /// </summary>
    private void SkidEffect()
    {
        //1 烟雾
        m_LeftSmoke.emit = true;
        m_RightSmoke.emit = true;

        //2 划痕
        WheelHit hit;
        //检测车轮是否与地面接触
        if (m_BLWheel.GetGroundHit(out hit))
        {
            if (m_PreLeftSkidPos != Vector3.zero)
            {
                Vector3 pos = hit.point;
                pos.y += 0.05f;

                Quaternion rotation = Quaternion.LookRotation(hit.point - m_PreLeftSkidPos);//当前点-上一个点，形成一个方向

                GameObject.Instantiate(m_SkidMask, pos, rotation);//在正确的方向处理划痕
            }
            m_PreLeftSkidPos = hit.point;
        }
        else
        {
            m_PreLeftSkidPos = Vector3.zero;
        }
        //右轮
        if (m_BRWheel.GetGroundHit(out hit))
        {
            if (m_PreRightSkidPos != Vector3.zero)
            {
                Vector3 pos = hit.point;
                pos.y += 0.05f;

                Quaternion rotation = Quaternion.LookRotation(hit.point - m_PreRightSkidPos);//当前点-上一个点，形成一个方向
                GameObject.Instantiate(m_SkidMask, pos, rotation);
            }
            m_PreLeftSkidPos = hit.point;
        }
        else
        {
            m_PreRightSkidPos = Vector3.zero;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall")
        {
            m_Boom.volume = m_Speed / 280;
            m_Boom.Play();
        }
    }
}
