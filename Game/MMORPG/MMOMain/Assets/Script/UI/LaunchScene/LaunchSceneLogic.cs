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
  void OnEnable()
  {
    SwitchToStartMenu();
  }

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
    PlayData.RoleData = roleList;
    if(PlayData.RoleData==null)
    {
      PlayData.RoleData = new List<Role>();
    }
    RoleSelectLogic.Instance.Init(roleList);
  }

  public void AddRoleSuccess(Role role)
  {
    PlayData.RoleData.Add(role);
    SwitchToSelectRoleMenu();
    RoleSelectLogic.Instance.Init(PlayData.RoleData);
  }
}






