using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour {
  public Driver m_Driver;
  private AudioSource m_Audio;

  float m_CD = 0.5f;
  float m_Timer = 0;
  void Start()
  {
    m_Audio = this.GetComponent<AudioSource>();
  }

  void Update()
  {
    m_Timer += Time.deltaTime;
    if (m_Timer >= m_CD)
    {
      m_Timer = 0;
      float pitch = m_Driver.Speed / m_Driver.MaxSpeed;
      m_Audio.pitch = 0.5f+pitch;
    }

  }
}
