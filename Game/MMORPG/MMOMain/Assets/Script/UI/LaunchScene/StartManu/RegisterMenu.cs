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
    m_Register.onClick.Add(new EventDelegate(OnRegisterClick));
    m_CancelRegister.onClick.Add(new EventDelegate(StartMenu.Instance.OnUserNameClick));
    m_Exit.onClick.Add(new EventDelegate(StartMenu.Instance.InitUI));
  }
  void OnEnable()
  {
    m_UserName.value = "";
    m_Password.value = "";
    m_RePassword.value = "";
  }
  //注册新账号
  private void OnRegisterClick()
  {
    if(m_UserName.value.Length<=3)
    {
      //提示
      Tips.ShowTip("账号长度至少为3");
      return;
    }
    if(m_Password.value.Length<=3)
    {
      //提示
      Tips.ShowTip("密码长度至少为3");
      return;
    }
    if (m_Password.value.Equals(m_RePassword.value))
    {
      //允许登录
      Register(m_UserName.value, m_Password.value);
    }
    else
    {
      Tips.ShowTip("重复输入的密码不一致");
    }
  }
  private void Register(string userName, string passWord)
  {
      User user = new User() { UserName = userName, PassWord = MD5Tools.GetMD5(passWord) };

    string json = ParaTools.GetJson<User>(user);
    Dictionary<byte, object> dic = new Dictionary<byte, object>();
    dic.Add((byte)ParameterCode.User, json);
    PhotoEngine.Instance.SendRequest(OperationCode.Register, dic);
  }


  
}
