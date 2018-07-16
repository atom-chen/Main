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
        m_AddRoleController = this.GetComponent<AddRoleHandler>();
        m_EnterGameController = this.GetComponent<EnterGameHandler>();
        m_LoginController = this.GetComponent<LoginHandler>();
        m_RegisterController = this.GetComponent<RegisterHandler>();
        m_ServerListController = this.GetComponent<ServerListHandler>();
    }
    void OnEnable()
    {
        SwitchToStartMenu();
    }

    private AddRoleHandler m_AddRoleController;
    private EnterGameHandler m_EnterGameController;
    private LoginHandler m_LoginController;
    private RegisterHandler m_RegisterController;
    private ServerListHandler m_ServerListController;


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
        if (PlayData.RoleData == null)
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






