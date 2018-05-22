using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
  public Transform m_Player;
  public Transform m_FirstCameraPos;//第一人称摄像机的位置
  private Transform m_Trans;

  private float m_Height = 3.5f;
  private float m_Distance = 7;

  private float m_SmoothSpeed = 5;

  private bool m_IsFirst = false;
	void Start () {
    m_Trans = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    if(m_IsFirst)
    {
      FirstView();
    }
    else
    {
      ThirdView();
    }
    if(Input.GetKeyDown(KeyCode.V))
    {
      m_IsFirst = !m_IsFirst;
    }
	}

  void FirstView()
  {
    m_Trans.position = m_FirstCameraPos.position;
    m_Trans.rotation = m_FirstCameraPos.rotation;
  }

  void ThirdView()
  {
    Vector3 targetForward = m_Player.forward;//车子的前方向
    targetForward.y = 0;
    Vector3 currentForward = m_Trans.forward;//自身的前方向
    currentForward.y = 0;

    //将自身的前方向缓缓转向车子的前方向，拿到插值后的前方向
    Vector3 forward = Vector3.Lerp(currentForward.normalized, targetForward.normalized, m_SmoothSpeed * Time.deltaTime);

    //位置变换：车子位置+高度+距离
    Vector3 targetPos = m_Player.position + Vector3.up * m_Height - forward * m_Distance;
    m_Trans.position = targetPos;//设置位置
    m_Trans.LookAt(m_Player);//设置视点
  }
}
