using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour {
  public List<EventDelegate> m_OnPress = new List<EventDelegate>();
  void OnPress(bool isPress)
  {
      EventDelegate.Execute(m_OnPress);
  }
}
