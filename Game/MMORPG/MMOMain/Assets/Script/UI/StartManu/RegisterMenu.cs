using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterMenu : MonoBehaviour {
  public UIInput m_UserName;
  public UIInput m_Password;
  public UIInput m_RePassword;

  public void Register()
  {
    if (m_Password.value.Equals(m_RePassword.value))
    {
      //发一个包到服务器

    }
    else
    {
      //弹出提示
    }
  }

  public void OnRePasswordCommit()
  {

  }
}
