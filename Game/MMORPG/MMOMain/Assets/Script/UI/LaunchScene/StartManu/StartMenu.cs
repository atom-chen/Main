﻿using System.Collections;
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
        if (PlayData.ServerData == null)
        {
            m_CurServerLabel.text = "请选择服务器";
        }
        else
        {
            m_CurServerLabel.text = PlayData.ServerData.name;
        }
        if (PlayData.UserData == null)
        {
            m_CurUserLabel.text = "点击登录";
        }
        else
        {
            m_CurUserLabel.text = PlayData.UserData.UserName;
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
        if (PlayData.ServerData == null)
        {
            Tips.ShowTip("请选择服务器");
            return;
        }
        if(PlayData.UserData==null)
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

    public void LoginSuccessed(User user)
    {
        if (user != null)
        {
            PlayData.UserData = user;
            m_CurUserLabel.text = user.UserName;
        }
        InitUI();
    }
    public void SetCurServer(Tab_Server server)
    {
        if (server != null)
        {
            PlayData.ServerData = server;
            m_CurServerLabel.text = server.name;
            InitUI();
        }
    }

}
