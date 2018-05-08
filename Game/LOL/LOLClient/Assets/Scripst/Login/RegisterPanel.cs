using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * 注册界面
 */ 
public class RegisterPanel : MonoBehaviour {


  public UnityEngine.EventSystems.EventTrigger m_Exit;//reg/close按钮
  public InputField m_UserName;
  public InputField m_PassWord;
  public InputField m_RePassWord;

  public Button m_RegisterBtn;//btns/reg按钮
  
  void Start()
  {
    m_RegisterBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(OnRegisteroClick));
    //new UnityEngine.EventSystems.PointerEventData
  }
  void OnEnable()
  {
    ReSetUI();
  }

  //点击注册
  private void OnRegisteroClick()
  {
    if (m_UserName.text.Length == 0 || m_UserName.text.Length > 6)
    {
      WaringManager.ShowWaring("账号不合法");
      return;
    }
    else if (m_PassWord.text.Length == 0 || m_PassWord.text.Length > 6)
    {
      WaringManager.ShowWaring("密码不合法");
      return;
    }
    else if (!m_PassWord.text.Equals(m_RePassWord.text))
    {
      WaringManager.ShowWaring("两次输入密码不一致");
      return;
    }
    //验证通过，申请注册...

    //清空输入框
    m_UserName.text = "";
    m_PassWord.text = "";
    m_RePassWord.text = "";
    //禁用UI
    DisUI();
    //发一个包
  }

  public void OnClose()
  {
    //清空输入框
    m_UserName.text = "";
    m_PassWord.text = "";
    m_RePassWord.text = "";
    LoginScreenLogic.Instance.CloseRegister();
  }

  public void ReSetUI()
  {
    m_UserName.text = "";
    m_PassWord.text = "";
    m_RePassWord.text = "";
    m_UserName.enabled = true;
    m_PassWord.enabled = true;
    m_RePassWord.enabled = true;
    m_RegisterBtn.enabled = true;
  }
  public void DisUI()
  {
    m_UserName.enabled = false;
    m_PassWord.enabled = false;
    m_RePassWord.enabled = false;
    m_RegisterBtn.enabled = false;
  }
}
