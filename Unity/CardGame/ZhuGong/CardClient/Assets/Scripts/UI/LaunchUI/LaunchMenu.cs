using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMenu : MonoBehaviour
{
  public GameObject register;
  public GameObject login;
  public GameObject forgetPassword;
  void Start()
  {
    if (register != null)
    {
      UILabel label = register.GetComponentInChildren<UILabel>();
      if(label!=null)
      {
        label.text = "注册账号";
      }
      UIEventListener listener = UIEventListener.Get(register);
      listener.onClick = OnRegisterClick;
    }


    if (login != null)
    {
      UILabel label = login.GetComponentInChildren<UILabel>();
      if (label != null)
      {
        label.text = "已有账号";
      }
      UIEventListener listener = UIEventListener.Get(login);
      listener.onClick = OnLoginClick;
    }


    if (forgetPassword != null)
    {
      UILabel label = forgetPassword.GetComponentInChildren<UILabel>();
      if (forgetPassword != null)
      {
        label.text = "忘记密码";
      }
      UIEventListener listener = UIEventListener.Get(forgetPassword);
      listener.onClick = OnForgetPasswordClick;
    }
  }

  public void OnLoginClick(GameObject obj)
  {
    LaunchSceneLogic.Instance.OpenLoginPanel();
  }

  public void OnRegisterClick(GameObject obj)
  {
    LaunchSceneLogic.Instance.OpenRegisterPanel();
  }

  public void OnForgetPasswordClick(GameObject obj)
  {

  }
}
