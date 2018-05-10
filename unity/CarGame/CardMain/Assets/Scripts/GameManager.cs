using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  private int m_CurFinish=0;
  private int m_NeedFish = 1;
  private int m_CurID = -1;
  public FinishQuad m_FinishQuad;

  public Driver m_Player;
  public CardInWait m_PlayerInWait;
  public UILabel m_WaitTimerLabel;

  public UILabel m_TimeLabel;//消耗时间
  private float m_Time;
  private bool m_IsCanRun = false;

  public UILabel m_NowFinishLabel;//当前已完成圈数

  public UILabel m_BestRecordLabel;//最高纪录显示
  float m_BestPoint=0;//最高纪录

  public UIEventTrigger m_ReStartGameBtn;//重新开始

  public Btn m_Skid;//漂移
  public Btn m_Stop;//刹车

  private int m_StartTime = 2;
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

  void OnEnable()
  {
    //取出最高纪录
    if(PlayerPrefs.HasKey("point1"))
    {
      m_BestPoint = PlayerPrefs.GetFloat("point1");
      int minute, second, ms;
      Utils.GetTime(m_BestPoint,out minute,out second,out ms);
      m_BestRecordLabel.text=string.Format("{0}:{1}:{2}", minute.ToString(), second.ToString(), ms.ToString());
    }
    else
    {
      m_BestPoint = float.MaxValue;
      m_BestRecordLabel.text = string.Format("00:00:00");
    }
    m_ReStartGameBtn.gameObject.SetActive(false);

  }
  void Start()
  {
    m_Time = 0;
    m_NowFinishLabel.text = "0/" + m_NeedFish;
    m_ReStartGameBtn.onClick.Add(new EventDelegate(ReStart));

    m_Skid.m_OnPress.Add(new EventDelegate(m_Player.Skid));
    m_Stop.m_OnPress.Add(new EventDelegate(m_Player.ShotDownCar));

    m_NowFinishLabel.gameObject.SetActive(false);
    m_TimeLabel.transform.parent.gameObject.SetActive(false);
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
    m_TimeLabel.transform.parent.gameObject.SetActive(true);
    m_IsCanRun = true;
    m_NowFinishLabel.gameObject.SetActive(true);
  }

  void Update()
  {
    if (m_IsCanRun)
    {
      m_Time += Time.deltaTime;
      int minute,second,ms;
      Utils.GetTime(m_Time, out minute, out second, out ms);
      m_TimeLabel.text = string.Format("{0}:{1}:{2}", minute.ToString(), second.ToString(), ms.ToString());
    }

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
        m_NowFinishLabel.text = m_CurFinish+"/" +m_NeedFish;
        Debug.Log(string.Format("完成{0}圈", m_CurFinish));
        m_CurID = -1;
        if(m_CurFinish>=m_NeedFish)
        {
          OnVictory();
        }
      }
    }
  }

  public void OnVictory()
  {
    m_IsCanRun = false;
    m_Player.enabled = false;
    //保存成绩
    if(m_Time<m_BestPoint)
    {
      //保存
      PlayerPrefs.SetFloat("point1", m_Time);
      int minute, second, ms;
      Utils.GetTime(m_Time, out minute, out second, out ms);
      m_BestRecordLabel.text = string.Format("{0}:{1}:{2}", minute.ToString(), second.ToString(), ms.ToString());
    }
    StartCoroutine(OpenRestartBtn());
  }
  IEnumerator OpenRestartBtn()
  {
    yield return new WaitForSeconds(5);
    m_ReStartGameBtn.gameObject.SetActive(true);
  }

  private void ReStart()
  {
    BG.Instance.OnSwitchScene(2);
    SceneManager.LoadScene(2);
  }


  
}
