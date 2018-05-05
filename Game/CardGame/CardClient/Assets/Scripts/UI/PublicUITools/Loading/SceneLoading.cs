using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoading : MonoBehaviour {
  public UISlider m_Slider;
  public UILabel m_Label;

  void OnEnable()
  {
    SetValue(0);
  }
	
  public void SetValue(float value)
  {
    m_Slider.value = value;
    m_Label.text = value * 100 + "%";
  }
}
