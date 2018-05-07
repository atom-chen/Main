using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenu : MonoBehaviour {
  public UIInput m_UserName;
  public UIInput m_Password;

  public UIButton m_LoginBtn;
  public UIButton m_RegisterBtn;
  public UIButton m_Exit;
  void Start()
  {
    m_LoginBtn.onClick.Add(new EventDelegate(OnClickLogin));
    m_RegisterBtn.onClick.Add(new EventDelegate(OnClickRegister));
    m_Exit.onClick.Add(new EventDelegate(StartMenu.Instance.InitUI));
  }
  //点击登录
  private void OnClickLogin()
  {
    LoginController controller=this.GetComponent<LoginController>();
    controller.Login(m_UserName.value, m_Password.value);
  }

  //点击注册
  public void OnClickRegister()
  {
    StartMenu.Instance.OnClickRegister();
  }

}
