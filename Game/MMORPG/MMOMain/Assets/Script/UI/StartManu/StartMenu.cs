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

  public GameObject m_StartObj;
  public LoginMenu m_LoginMenu;
  public RegisterMenu m_RegisterMenu;
  public ChooseServer m_ChooseServer;


  private User  m_CurUser;//当前用户
  private ServerProperty m_CurServer;//当前选择的服务器
  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {

  }
  void OnEnable()
  {
    InitUI();
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
    m_ChooseServer.SetServerList(serverList);
  }

  public void SetCurUser(User user)
  {
    m_CurUser = user;
  }

  public void SetCurServer(ServerProperty server)
  {
    if(server!=null)
    {
      this.m_CurServer = server;
      InitUI();
    }
  }

}
