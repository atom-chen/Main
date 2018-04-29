using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
  private WheelCollider m_Wheel;
  private Transform m_Trans;

  void Start()
  {
    m_Wheel = this.GetComponent<WheelCollider>();
    m_Trans = this.GetComponent<Transform>().parent;
  }
  void Update()
  {
    m_Trans.Rotate(m_Wheel.rpm * 6 * Time.deltaTime * Vector3.right);
  }
}
