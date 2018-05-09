using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginScreenLogic : MonoBehaviour
{


  private static LoginScreenLogic _Instance;
  public static LoginScreenLogic Instance
  {
    get
    {
      return _Instance;
    }
  }
  public InputField m_LoginUsername;
  public InputField m_LoginPassword;
  public Button m_EnterGame;//登陆按钮

  public GameObject m_PopRegister;//注册账号UI

  public Button m_Register;
  public Button m_GuanWang;
  void Awake()
  {
    _Instance = this;
  }
  void OnEnable()
  {
    InitUI();
  }
  void Start()
  {
    m_Register.onClick.AddListener(new UnityEngine.Events.UnityAction(OnRegisterClick));
    m_EnterGame.onClick.AddListener(new UnityEngine.Events.UnityAction(OnLoginClick));
    m_PopRegister.gameObject.SetActive(false);
  }
  public void InitUI()
  {
    m_Register.enabled = true;
    m_GuanWang.enabled = true;
    m_EnterGame.enabled = true;
    m_LoginUsername.enabled = true;
    m_LoginPassword.enabled = true;
  }
  public void DisUI()
  {
    m_Register.enabled = false;
    m_GuanWang.enabled = false;
    m_EnterGame.enabled = false;
    m_LoginUsername.enabled = false;
    m_LoginPassword.enabled = false;
  }
  //点击登录
  public void OnLoginClick()
  {
    if (m_LoginUsername.text.Length == 0 || m_LoginUsername.text.Length > 18)
    {
      //弹出提示窗
      WaringManager.ShowWaring("账号不合法！");
      return;
    }
    else if (m_LoginPassword.text.Length == 0 || m_LoginPassword.text.Length > 20)
    {
      WaringManager.ShowWaring("密码不合法！");
      return;
    }
    //客户端验证通过 申请登陆
    DisUI();
  }

  //点击注册账号
  public void OnRegisterClick()
  {
    //注册界面可见
    m_PopRegister.gameObject.SetActive(true);
  }
  //监听reg/close
  public void CloseRegister()
  {
    //关闭注册UI
    m_PopRegister.SetActive(false);
  }


}
