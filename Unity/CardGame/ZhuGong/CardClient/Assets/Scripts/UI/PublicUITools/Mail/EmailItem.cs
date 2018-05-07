using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailItem : MonoBehaviour {
  public UISprite m_Head;
  public UILabel m_Sender;
  public UILabel m_Time;
  public UIEventTrigger m_RemoveBtn;

  public void InitEmailItem(string headName,string sender,string time)
  {
    m_Head.spriteName = headName;
    m_Sender.text = string.Format("{0}的来信", sender);
    m_Time.text = time;
  }
 

}
