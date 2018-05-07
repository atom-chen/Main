using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUILogic : MonoBehaviour {
  public UIEventTrigger m_Email;

  void Start()
  {
    m_Email.onClick.Add(new EventDelegate(OnClickMail));
  }

  private void OnClickMail()
  {
    UIManager.CreateUI("Mail",this.transform.root);
  }


}
