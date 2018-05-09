using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
  private static StartMenu _Instance;
  public static StartMenu Instance
  {
    get
    {
      return _Instance;
    }
  }
  public UIButton m_UserBtn;
  public UIButton m_ServerBtn;
  public UIButton m_EnterGame;

  public GameObject m_StartObj;
  public LoginMenu m_LoginMenu;
  public RegisterMenu m_RegisterMenu;
  public ChooseServer m_ChooseServer;

  public UILabel m_CurUserLabel;
  public UILabel m_CurServerLabel;

  private User  m_CurUser;//当前用户
  private ServerProperty m_CurServer;//当前选择的服务器
  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {
    m_UserBtn.onClick.Add(new EventDelegate(OnUserNameClick));
    m_ServerBtn.onClick.Add(new EventDelegate(OnServerClick));
    m_EnterGame.onClick.Add(new EventDelegate(OnEnterGameClick));
  }
  void OnEnable()
  {
    InitUI();
    if (m_CurServer == null)
    {
      m_CurServerLabel.text = "请选择服务器";
    }
    else
    {
      m_CurServerLabel.text = m_CurServer.Name;
    }
    if(m_CurUser==null)
    {
      m_CurUserLabel.text = "点击登录";
    }
    else
    {
      m_CurUserLabel.text = m_CurUser.UserName;
    }
  }
  //点击用户名
  public void OnUserNameClick()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(true);
    m_RegisterMenu.gameObject.SetActive(false);
    m_ChooseServer.gameObject.SetActive(false);
  }
  //点击服务器列表
  public void OnServerClick()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(false);
    m_RegisterMenu.gameObject.SetActive(false);
    m_ChooseServer.gameObject.SetActive(true);
  }
  //点击进入游戏
  public void OnEnterGameClick()
  {
    //发包
  }

  //点击注册
  public void OnClickRegister()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(false);
    m_RegisterMenu.gameObject.SetActive(true);
    m_ChooseServer.gameObject.SetActive(false);
  }
  //点击登录
   public void GotoLoginMenu()
  {
    m_StartObj.SetActive(false);
    m_LoginMenu.gameObject.SetActive(true);
    m_RegisterMenu.gameObject.SetActive(false);
    m_ChooseServer.gameObject.SetActive(false);
  }
  //回到初始界面
  public void InitUI()
  {
    m_StartObj.SetActive(true);
    m_LoginMenu.gameObject.SetActive(false);
    m_RegisterMenu.gameObject.SetActive(false);
    m_ChooseServer.gameObject.SetActive(false);
  }

  public void SetServerList(List<ServerProperty> serverList)
  {
    m_ChooseServer.SetServerList(serverList,m_CurServer);
  }

  public void LoginSuccessed(User user)
  {
    if (user != null)
    {
      m_CurUser = user;
      m_CurUserLabel.text = user.UserName;
    }
    InitUI();
  }
  public void SetCurServer(ServerProperty server)
  {
    if(server!=null)
    {
      this.m_CurServer = server;
      m_CurServerLabel.text = server.Name;
      InitUI();
    }
  }

}
