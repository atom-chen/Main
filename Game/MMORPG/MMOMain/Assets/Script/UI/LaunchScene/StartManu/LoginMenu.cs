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
    if(m_UserName.value.Length<=3)
    {
      Tips.ShowTip("账号输入不正确");
      return;
    }
    else if (m_Password.value.Length <= 3)
    {
      Tips.ShowTip("密码输入不正确");
      return;
    }
    Login(m_UserName.value, m_Password.value);


  }
  private void Login(string userName, string passWord)
  {
    User user = new User();
    user.UserName = userName;
    user.PassWord = MD5Tools.GetMD5(passWord);
    string json = LitJson.JsonMapper.ToJson(user);
    Dictionary<byte, object> dic = new Dictionary<byte, object>();
    dic.Add((byte)ParameterCode.User, json);

    if (PhotoEngine.Instance != null)
    {
      PhotoEngine.Instance.SendRequest(OperationCode.Login, dic);
    }
  }

  //点击注册
  public void OnClickRegister()
  {
    StartMenu.Instance.OnClickRegister();
  }

}
