using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDownUI : MonoBehaviour {

  bool m_IsPress = false;

  private UITweener m_Tween;
  void Start()
  {
    m_Tween = this.GetComponent<UITweener>();
  }
  public void Press()
  {
    //松开
    if(m_IsPress)
    {
      m_IsPress = false;
      m_Tween.PlayReverse();
    }
    //按下，发生减速
    else
    {
      m_IsPress = true;
      m_Tween.ResetToBeginning();
      m_Tween.PlayForward();
    }
  }
}
