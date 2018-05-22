using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInWait : MonoBehaviour {
  public AudioSource m_EngineSource;
  public EllipsoidParticleEmitter m_LeftEmitter;
  public EllipsoidParticleEmitter m_RightEmitter;


  float m_MoveVertical = 0;
  float m_MoveHozizontal = 0;

  float m_Speed = 0;
  void Start()
  {
    m_EngineSource.pitch = 0.5f;
  }
	void Update () 
  {
    m_MoveVertical = 1;
    //PC端输入
#if UNITY_EDITOR
    m_MoveHozizontal = Input.GetAxis("Horizontal");
#endif

    //移动端输入
#if (UNITY_ios || UNITY_ANDROID)
   m_MoveHozizontal = Input.acceleration.x;
#endif

    Forward();
    Turn();
	}

  private void Forward()
  {
    if(m_MoveVertical>=0 && !m_EngineSource.isPlaying)
    {
      m_EngineSource.Play();
    }
  }

  private void Turn()
  {
    if(m_MoveHozizontal>0)
    {
      m_RightEmitter.emit = true;
    }
    else if(m_MoveHozizontal<0)
    {
      m_LeftEmitter.emit = true;
    }
    else
    {
      m_RightEmitter.emit = false;
      m_LeftEmitter.emit = false;
    }
  }
}
