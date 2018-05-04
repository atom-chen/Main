using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishQuad : MonoBehaviour {
  public  int m_ID;
  void OnTriggerEnter(Collider other)
  {
    Debug.Log("关卡" + m_ID + "遭遇");
    if(other.tag==Tags.PLAYER)
    {
      GameManager.Instance.OnFinish(m_ID);
    }
  }
}
