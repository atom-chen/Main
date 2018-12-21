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

    public GameObject m_WaitServerObj;

    private string mCurUserName = "";
    private string mCurPassword = "";
    private Tab_Server mCurServer = null;
    public Tab_Server CurServer
    {
        get { return mCurServer; }
    }
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
        m_CurUserLabel.text = string.IsNullOrEmpty(mCurUserName) == null ? "点击登录" : mCurUserName;
        m_CurServerLabel.text = mCurServer == null ? "请选择服务器" : mCurServer.name;
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


    //点击确认
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
        //将账号信息保存下来
        mCurUserName = m_UserName.value;
        mCurPassword = m_Password.value;         
        InitUI();
    }
    //点击用户名
    public void OnUserNameClick()
    {
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(true);
        m_RegisterMenu.gameObject.SetActive(false);
        m_ChooseServer.gameObject.SetActive(false);
        m_UserName.value = mCurUserName;
        m_Password.value = mCurPassword;
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
        if (mCurServer == null)
        {
            Tips.ShowTip("请选择服务器");
            return;
        }
        if (string.IsNullOrEmpty(mCurUserName))
        {
            Tips.ShowTip("请输入账号和密码");
            return;
        }
        GameManager.NetManager.TryConnect(mCurServer);
        //发包
        CG_ENTER_GAME_PAK pak = new CG_ENTER_GAME_PAK();
        pak.userName = mCurUserName;
        pak.passWord = mCurPassword;
        pak.SendPak();
        m_WaitServerObj.SetActive(true);
    }


    //点击注册
    public void OnClickRegister()
    {
        m_StartObj.SetActive(false);
        m_LoginObj.SetActive(false);
        m_RegisterMenu.gameObject.SetActive(true);
        m_ChooseServer.gameObject.SetActive(false);
    }

    //注册成功  缓存所注册的账号信息
    public void HandlePackage(GC_REGISTER_USER_RET_PAK package)
    {
        m_WaitServerObj.SetActive(false);
        if(package.Success)
        {
            User user = package._User;
            if (user != null)
            {
                mCurUserName = user.UserName;
                mCurPassword = user.PassWord;
                InitUI();
            }
        }
    }

    public void HandlePackage(GC_ENTER_GAME_RET_PAK pak)
    {
        m_WaitServerObj.SetActive(false);
        if(pak.Success)
        {
            m_StartObj.SetActive(false);
            m_SelectRoleMenu.SetActive(true);
        }

    }

    //选择了一个服务器
    public void HandleOnChooseServer(Tab_Server server)
    {
        if (server != null)
        {
            mCurServer = server;
            InitUI();
        }
    }
}






