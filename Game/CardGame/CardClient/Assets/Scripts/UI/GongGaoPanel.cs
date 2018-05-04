using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongGaoPanel : MonoBehaviour {
  public UILabel m_GongGao;
  public void SetGongGaoText(string text)
  {
    m_GongGao.text = text;
  }
}
