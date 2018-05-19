using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSceneLogic : MonoBehaviour
{
  private static LaunchSceneLogic _Instance;
  public static LaunchSceneLogic Instance
  {
    get
    {
      return _Instance;
    }
  }
  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {
    m_AddRoleController = this.GetComponent<AddRoleController>();
    m_EnterGameController = this.GetComponent<EnterGameControllr>();
    m_LoginController = this.GetComponent<LoginController>();
    m_RegisterController = this.GetComponent<RegisterController>();
    m_ServerListController = this.GetComponent<ServerListController>();
  }
  void OnEnable()
  {
    SwitchToStartMenu();
  }

  private AddRoleController m_AddRoleController;
  private EnterGameControllr m_EnterGameController;
  private LoginController m_LoginController;
  private RegisterController m_RegisterController;
  private ServerListController m_ServerListController;


  public GameObject m_StartMenu;
  public GameObject m_SelectRoleMenu;
  public GameObject m_CreateRoleMenu;
  public void SwitchToStartMenu()
  {
    m_CreateRoleMenu.SetActive(false);
    m_SelectRoleMenu.SetActive(false);
    m_StartMenu.SetActive(true);
  }
  public void SwitchToSelectRoleMenu()
  {
    m_CreateRoleMenu.SetActive(false);
    m_SelectRoleMenu.SetActive(true);
    m_StartMenu.SetActive(false);
  }
  public void SwitchToCreateRoleMenu()
  {
    m_CreateRoleMenu.SetActive(true);
    m_SelectRoleMenu.SetActive(false);
    m_StartMenu.SetActive(false);
  }

  public void SetRoleList(List<Role> roleList)
  {
    SwitchToSelectRoleMenu();
    PlayData.RoleList = roleList;
    if(PlayData.RoleData==null)
    {
      PlayData.RoleList = new List<Role>();
    }
    RoleSelectLogic.Instance.Init(roleList);
  }

  public void AddRoleSuccess(Role role)
  {
    PlayData.RoleList.Add(role);
    SwitchToSelectRoleMenu();
    RoleSelectLogic.Instance.Init(PlayData.RoleList);
  }
}






