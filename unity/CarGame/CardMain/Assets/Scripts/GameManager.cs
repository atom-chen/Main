using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  private int m_CurFinish=0;
  private int m_NeedFish = 2;
  private int m_CurID = -1;
  public FinishQuad m_FinishQuad;

  public Driver m_Player;
  public CardInWait m_PlayerInWait;
  public UILabel m_WaitTimerLabel;

  private int m_StartTime = 5;
  private static GameManager _Instance;
  public static GameManager Instance
  {
    get
    {
      return _Instance;
    }
  }
  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {
    m_Player.enabled = false;
    m_PlayerInWait.enabled = true;
    //开始计时
    StartCoroutine(StartTimer());
  }
  IEnumerator StartTimer()
  {
    float m_CD = 1;
    float m_Timer = m_StartTime;
    while(m_Timer>0)
    {
      m_WaitTimerLabel.text = m_Timer.ToString();
      yield return new WaitForSeconds(m_CD);
      m_Timer -= m_CD;
    }
    m_WaitTimerLabel.gameObject.SetActive(false);
    m_PlayerInWait.enabled = false;
    m_Player.enabled = true;
  }
  //突破关卡时被调用
  public void OnFinish(int id)
  {
    //非法性检测
    if(id==m_CurID+1)
    {
      m_CurID++;
      if (id == m_FinishQuad.m_ID)
      {
        m_CurFinish++;
        Debug.Log(string.Format("完成{0}圈", m_CurFinish));
        m_CurID = -1;
      }
    }
  }


  
}
