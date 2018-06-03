using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCar : MonoBehaviour {
  private float m_Vertical;
  private float m_Horizontal;
	
	void Update () {
    m_Vertical = Input.GetAxis("Vertical");
    m_Horizontal = Input.GetAxis("Horizontal");
    Run();
    Rotate();
	}
  void Rotate()
  {
    if(m_Horizontal!=0)
    {
      Vector3 euler=transform.eulerAngles;
      transform.eulerAngles = new Vector3(euler.x, euler.y + m_Horizontal, euler.z);
    }
  }

  void Run()
  {
    if(m_Vertical!=0)
    {
      transform.position += transform.forward*m_Vertical* Time.deltaTime*4;
    }
  }
}
