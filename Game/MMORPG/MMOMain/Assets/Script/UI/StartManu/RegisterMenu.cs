using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterMenu : MonoBehaviour {
  public UIInput m_UserName;
  public UIInput m_Password;
  public UIInput m_RePassword;

  public UIButton m_Register;
  public UIButton m_CancelRegister;
  public UIButton m_Exit;
  void Start()
  {
    m_RePassword.onSubmit.Add(new EventDelegate(OnRePasswordCommit));
    m_Register.onClick.Add(new EventDelegate(OnRegisterClick));
    m_CancelRegister.onClick.Add(new EventDelegate(StartMenu.Instance.OnUserNameClick));
    m_Exit.onClick.Add(new EventDelegate(StartMenu.Instance.InitUI));
  }
  private void OnRePasswordCommit()
  {

  }

  //注册新账号
  private void OnRegisterClick()
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


  
}
