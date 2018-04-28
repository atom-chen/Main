using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
  public Transform m_Player;
  private Transform m_Trans;

  private float m_Height = 3.5f;
  private float m_Distance = 7;

  private float m_SmoothSpeed = 1;
	void Start () {
    m_Trans = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    Vector3 targetForward = m_Player.forward;
    targetForward.y = 0;
    Vector3 currentForward = m_Trans.forward;
    currentForward.y = 0;


    Vector3 forward = Vector3.Lerp(currentForward.normalized, targetForward.normalized, m_SmoothSpeed * Time.deltaTime);//将摄像机的前方缓慢转向玩家的前方

    Vector3 targetPos = m_Player.position + Vector3.up * m_Height - forward * m_Distance;
    m_Trans.position = targetPos;//设置位置
    m_Trans.LookAt(m_Player);//设置视点
	}
}
