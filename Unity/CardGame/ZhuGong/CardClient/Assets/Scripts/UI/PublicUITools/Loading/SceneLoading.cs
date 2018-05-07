using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {
  public UISlider m_Slider;
  public UILabel m_Label;
  private const float m_Timer=0.1f;
  
  public void Init(int nextSceneID,float waitTime)
  {
    StartCoroutine(Loading(nextSceneID, m_Timer / waitTime));
  }
  IEnumerator Loading(int nextSceneID,float increment)
  {
    while(m_Slider.value <1)
    {
      yield return new WaitForSeconds(m_Timer);
      SetValue(m_Slider.value + increment);
    }
    SceneManager.LoadScene(nextSceneID);
    
  }

  void OnEnable()
  {
    SetValue(0);
  }
	
  public void SetValue(float value)
  {
    m_Slider.value = value;
    m_Label.text = Math.Min(Math.Round(value,3),1) * 100 + "%";
  }

  //设置加载总时间
}
