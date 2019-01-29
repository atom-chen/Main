using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMarkView : MonoBehaviour {
    private static SwitchMarkView m_Instance;
    public static SwitchMarkView Instance()
    {
        return m_Instance;
    }
    public int StartIndex = 0;

    private static Color32 m_NormalColor = new Color32(255, 255, 255, 255);
    private static Color32 m_PressColor = new Color32(183, 163, 123, 255); 
    public UISprite[] m_SwitchMark;
    private UISprite m_CurMark;
    void Awake()
    {
        m_Instance = this;
    }
    void Start()
    {
        if (m_SwitchMark == null)
        {
            m_SwitchMark = this.GetComponentsInChildren<UISprite>();
        }
        m_CurMark = m_SwitchMark[StartIndex];
        SwitchMark(StartIndex);
    }


  public void SwitchMark(int index)
  {
      if(index>=m_SwitchMark.Length)
      {
          return;
      }
      else if(index <0)
      {
          m_CurMark.color = m_NormalColor;
      }
      else
      {
          m_CurMark.color = m_NormalColor;
          m_CurMark = m_SwitchMark[index];
          m_CurMark.color = m_PressColor;
      }
  }
}
