using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPanel : MonoBehaviour {

    float m_Timer = 0;
    float m_UpdateSpeedTimer = 0.5f;

    public UILabel m_SpeedLabel;
    public Driver m_Car;

    public Transform m_Point;
    private float m_PreAngle = 0;
    void Update()
    {
      m_Timer += Time.deltaTime;
      if (m_Timer >= m_UpdateSpeedTimer)
      {
        m_Timer = 0;
        float speed = m_Car.Speed;
        speed = Mathf.Round(speed);
        if (speed >= 0)
        {
            if(speed<=140)
            {
                m_SpeedLabel.text = speed + "";
            }
            else
            {
                m_SpeedLabel.text = 140 + "";
            }
        }
        else
        {
          m_SpeedLabel.text = 0 + "";
        }
        float newZRotation = (speed-m_PreAngle)*270/140;
        m_Point.eulerAngles = new Vector3(m_Point.eulerAngles.x, m_Point.eulerAngles.y, newZRotation);
      }

    }
}
