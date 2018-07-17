/********************************************************************************
 *	文件名：NetManager.cs
 *	全路径：	\Script\LogicCore\NetManager.cs
 *	创建人：	王华
 *	创建时间：2013-12-05
 *
 *	功能说明： 网络管理器
*********************************************************************************/

using System;
using UnityEngine;
using System.IO;
//using SPacket.SocketInstance;
using System.Collections.Generic;
using Games.Table;
using Network;

public class NetManager : MonoBehaviour
{   
    public static bool IsReconnecting = false;

    //static public NetManager m_netManager;
   
    private static NetManager m_instance = null;
    public static NetManager Instance()
    {
        return m_instance;
    }


    public static bool IsDisConnect()
    {
        return NetworkLogic.State == NetworkLogic.ConnectState.DISCONNECT ; 
    }


    private string m_connectIP;
    private int m_connectPort;
    private string m_reconnectIP;
    private int m_reconnectPort;

    private NetworkLogic.DelConnectResult m_delConnect = null;

    void Awake()
    {
        if (m_instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        m_instance = this;
        DontDestroyOnLoad(this.gameObject);
        NetworkLogic.SetStateListener(OnConnectStateChanged);
    }

    void Update()
    {
        NetworkLogic.Update();
    }

    public void ReconnectToBigWorld(string ip, int port, NetworkLogic.DelConnectResult delConnect)
    {
        GameManager.OnConnectLost();
        ConnectToServer(ip, port, false, delConnect);
    }

    public void ReconnectToServer(NetworkLogic.DelConnectResult delConnect)
    {
        GameManager.OnConnectLost();
        //LogModule.DebugLog("Reconnect To Server");
        ConnectToServer(m_reconnectIP, m_reconnectPort, false, delConnect);
    }
    public void ConnectToServer(string _ip, int _port, bool bUseReconnect, NetworkLogic.DelConnectResult delConnect)
    {
        m_connectIP = _ip;
        m_connectPort = _port;
        if (bUseReconnect)
        {

            m_reconnectIP = _ip;
            m_reconnectPort = _port;
        }

        m_delConnect = delConnect;

        DoConnectToServer();
        //CDN更新屏蔽
        //if (PlatformHelper.IsEnableUpdate())
        //{
        //    StartCoroutine(UpdateHelper.CheckResVersion(OnConnectCheckResVersion));
        //}
        //else
        //{
        //    DoConnectToServer();
        //}
    }

    private void OnConnectCheckResVersion(UpdateHelper.CheckVersionResult result)
    {
        if (result == UpdateHelper.CheckVersionResult.NEEDUPDATE)
        {
            // 需要资源更新，退出游戏重新登录
            //TODO:

        }
        else if (result == UpdateHelper.CheckVersionResult.NONEEDUPDATE)
        {
            DoConnectToServer();
        }
        else
        {
			NetworkLogic.DelConnectResult delTmp = m_delConnect;
			m_delConnect = null;
			if (null != delTmp)
				delTmp(false);
        }
    }

    //连接上次连接过的服务器
    private void DoConnectToServer()
    {
		NetworkLogic.DelConnectResult delTmp = m_delConnect;
		m_delConnect = null;
		NetworkLogic.Connect(m_connectIP, m_connectPort, delTmp);
    }
      
    //网络环境变化
    private void OnConnectStateChanged(NetworkLogic.ConnectState state, bool bManual)
    {
        if (state == NetworkLogic.ConnectState.DISCONNECT && !bManual)
        {
            //断线了
            if (state == NetworkLogic.ConnectState.DISCONNECT && !bManual)
            {
                WattingTipController.Close();
                //断线了
                if (LoginController.Instance() != null)
                {
                    LoginController.Instance().ConnectLost();
                }
                else if (ReconnectTipController.Instance() != null)
                {
                    ReconnectTipController.Instance().ShowAskReconnect();
                }
                else
                {
                    UIManager.ShowUI(UIInfo.ReconnectTip);
                    GameManager.OnConnectLost();
                }
            }
        }
    }

    private void OnClickQuitGame()
    {
        Application.Quit();
    }
}
