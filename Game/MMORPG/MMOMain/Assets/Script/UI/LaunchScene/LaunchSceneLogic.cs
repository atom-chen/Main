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

    public UIButton m_UserBtn;
    public UIButton m_ServerBtn;
    public UIButton m_EnterGame;

    public UIButton m_LoginBtn;
    public UIButton m_RegisterBtn;
    public UIButton m_Exit;

    public UIInput m_UserName;
    public UIInput m_Password;

    public GameObject m_StartObj;
    public GameObject m_LoginObj;
    public RegisterMenu m_RegisterMenu;
    public ChooseServer m_ChooseServer;
    public GameObject m_SelectRoleMenu;
    public GameObject m_CreateRoleMenu;

    public UILabel m_CurUserLabel;
    public UILabel m_CurServerLabel;


    void OnEnable()
    {
        InitUI();
    }

    void Start()
    {
        m_LoginBtn.onClick.Add(new EventDelegate(OnClickLogin));
        m_RegisterBtn.onClick.Add(new EventDelegate(OnClickRegister));
        m_Exit.onClick.Add(new EventDelegate(InitUI));

        m_UserBtn.onClick.Add(new EventDelegate(OnUserNameClick));
        m_ServerBtn.onClick.Add(new EventDelegate(OnServerClick));
        m_EnterGame.onClick.Add(new EventDelegate(OnEnterGameClick));
    }
    //回到初始界面
    public void InitUI()
    {
        m_StartObj.SetActive(true);
        m_LoginObj.SetActive(false);
        m_RegisterMenu.gameObject.SetActive(false);
        m_ChooseServer.gameObject.SetActive(false);
        m_CurUserLabel.text = PlayData.UserData == null ? "点击登录" : PlayData.UserData.UserName;
        m_CurServerLabel.text = PlayData.ServerData == null ? "请选择服务器" : PlayData.ServerData.name;
    }

    //角色选择界面
    public void SwitchToSelectRoleMenu()
    {
        m_CreateRoleMenu.SetActive(false);
        m_SelectRoleMenu.SetActive(true);
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(false);
    }

    //创角界面
    public void SwitchToCreateRoleMenu()
    {
        m_CreateRoleMenu.SetActive(true);
        m_SelectRoleMenu.SetActive(false);
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(false);
    }


    //点击登录
    private void OnClickLogin()
    {
        if (m_UserName.value.Length <= 3)
        {
            Tips.ShowTip("账号输入不正确");
            return;
        }
        else if (m_Password.value.Length <= 3)
        {
            Tips.ShowTip("密码输入不正确");
            return;
        }
        User user = new User();
        user.UserName = m_UserName.value;
        user.PassWord = m_Password.value;

        PlayData.UserData = user;              //将账号信息保存下来
        InitUI();
    }
    //点击用户名
    public void OnUserNameClick()
    {
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(true);
        m_RegisterMenu.gameObject.SetActive(false);
        m_ChooseServer.gameObject.SetActive(false);
    }

    //点击服务器列表
    public void OnServerClick()
    {
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(false);
        m_RegisterMenu.gameObject.SetActive(false);
        m_ChooseServer.gameObject.SetActive(true);
    }
    //点击进入游戏
    public void OnEnterGameClick()
    {
        if (PlayData.ServerData == null)
        {
            Tips.ShowTip("请选择服务器");
            return;
        }
        if (PlayData.UserData == null)
        {
            Tips.ShowTip("请输入账号和密码");
        }
        //发包
        CG_ENTER_GAME_PAK pak = new CG_ENTER_GAME_PAK();
        pak._User = PlayData.UserData;
        pak.SendPak();
    }


    //点击注册
    public void OnClickRegister()
    {
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(false);
        m_RegisterMenu.gameObject.SetActive(true);
        m_ChooseServer.gameObject.SetActive(false);
    }


    public void HandlePackageRegister(GC_REGISTER_USER_RET_PAK package)
    {
        if(package.Success)
        {
            User user = package._User;
            if (user != null)
            {
                PlayData.UserData = user;
                m_CurUserLabel.text = user.UserName;
            }
            InitUI();
        }
        else
        {

        }
    }

    public void HandleOnChooseServer(Tab_Server server)
    {
        if (server != null)
        {
            PlayData.ServerData = server;
            m_CurServerLabel.text = server.name;
            InitUI();
        }
    }
}






