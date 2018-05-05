using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailItem : MonoBehaviour {
  public UISprite m_Head;
  public UILabel m_Sender;
  public UILabel m_Time;
  public UIEventTrigger m_RemoveBtn;


  void OnEnable()
  {
    m_RemoveBtn.onClick.Add(new EventDelegate( OnRemoveClick));
  }
  public void InitEmailItem(string headName,string sender)
  {
    m_Head.spriteName = headName;
    m_Sender.text = string.Format("{0}的来信", sender);
  }
 
  public void OnRemoveClick()
  {
    Destroy(this.gameObject);
  }
}
