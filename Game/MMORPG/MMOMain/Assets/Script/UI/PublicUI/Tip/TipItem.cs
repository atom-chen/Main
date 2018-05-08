using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipItem : MonoBehaviour {

  public UILabel m_Text;
  public void Init(string tip)
  {
    m_Text.text = tip;
    StartCoroutine(DestoryTip());
  }

  IEnumerator DestoryTip()
  {
    yield return new WaitForSeconds(2.0f);
    Destroy(this.gameObject);
    Tips.OnItemDestory(this);
  }
}
