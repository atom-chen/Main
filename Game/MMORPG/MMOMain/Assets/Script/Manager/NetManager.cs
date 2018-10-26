using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MsgPair
{
    public OperationCode opCode;
    public Dictionary<byte, object> parameters;
    public MsgPair(OperationCode code,Dictionary<byte, object> para)
    {
        opCode = code;
        parameters = para;
    }
}
public class NetManager : MonoBehaviour, IPhotonPeerListener
{
    private static string m_IPAddres = "";
    private static string m_AppName = "";

    PhotonPeer peer;
    private StatusCode m_State = new StatusCode();//当前状态

    void Awake()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
        DontDestroyOnLoad(this.gameObject);
    }

    void FixedUpdate()
    {
        if(m_State ==StatusCode.Connect &&  m_Cache.Count>0)
        {
            MsgPair pair = m_Cache.Dequeue();
            SendRequest(pair.opCode, pair.parameters);
        }
        if (peer != null)
        {
            peer.Service();
        }
    }
//----------------------------------------------事件相关 begin------------------------------------------
    public delegate void OnConnectStateChange();
    public event OnConnectStateChange OnConnectSuccess;//成功连接后的事件
    public event OnConnectStateChange OnConnectBreak;//连接断开后的事件
    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {

    }
//----------------------------------------------事件相关 end------------------------------------------


//------------------------------------------------连接相关 begin--------------------------------

    public void OnStatusChanged(StatusCode statusCode)
    {
        m_State = statusCode;
        switch (statusCode)
        {
            case StatusCode.Connect:
                StopAllCoroutines();
                Tips.ShowTip("已连上服务器");
                if (OnConnectSuccess != null)
                {
                    try
                    {
                        OnConnectSuccess();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
                break;
            case StatusCode.Disconnect:
                Tips.ShowTip("连接断开");
                if (OnConnectBreak != null)
                {
                    try
                    {
                        OnConnectBreak();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
                StartCoroutine(TryConnect());
                break;
            default:
                //尝试连接本地
                Tips.ShowTip("连接状态异常");
                StartCoroutine(TryConnect());
                break;
        }
    }

    public void TryConnect(Tab_Server server)
    {
        if (server == null) 
            return;
        m_IPAddres = string.Format("{0}:{1}", server.ipAddress, server.port.ToString());
        m_AppName = server.appName;
        StartCoroutine(TryConnect());
    }

    IEnumerator TryConnect()
    {
        while (m_State != StatusCode.Connect)
        {
            peer.Connect(m_IPAddres, m_AppName);
            yield return new WaitForSeconds(3);
        }
    }
 //------------------------------------------------连接相关 end--------------------------------

//----------------------------------------------消息相关 begin------------------------------------------
    private Dictionary<byte, GC_PAK_BASE> m_PakDic = new Dictionary<byte, GC_PAK_BASE>();//逻辑模块集合
    private Queue<MsgPair> m_Cache = new Queue<MsgPair>();

    //收到消息
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch(operationResponse.OperationCode)
        {
            case (byte)OperationCode.EnterGame: GC_ENTER_GAME_RET_PAK pak = new GC_ENTER_GAME_RET_PAK(); pak._Response = operationResponse; PlayData.ReceivePacket(pak); break;
            case (byte)OperationCode.Register: GC_REGISTER_USER_RET_PAK pak2 = new GC_REGISTER_USER_RET_PAK(); pak2._Response = operationResponse; PlayData.ReceivePacket(pak2); break;
            case (byte)OperationCode.RoleAdd: GC_ROLE_ADD_RET_PAK pak3 = new GC_ROLE_ADD_RET_PAK(); pak3._Response = operationResponse; PlayData.ReceivePacket(pak3); break;
            case (byte)OperationCode.StartGame: GC_START_GAME_RET_PAK pak4 = new GC_START_GAME_RET_PAK(); pak4._Response = operationResponse; PlayData.ReceivePacket(pak4); break;
            default: Debug.Log("Receive a unknown response . OperationCode :" + operationResponse.OperationCode); break;
        }
    }

    //发一个OperationCode消息
    public void SendRequest(OperationCode opCode, Dictionary<byte, object> parameters)
    {
        if (m_State == StatusCode.Connect)
        {
            peer.OpCustom((byte)opCode, parameters, true);
        }
        else
        {
            //TryConnect(GameManager..ServerData);
            m_Cache.Enqueue(new MsgPair(opCode, parameters));
        }
    }

    //注销Handler
    public void UnRegisterController(OperationCode opCode)
    {
        m_PakDic.Remove((byte)opCode);
    }

    public void RegisterController(OperationCode opCode, GC_PAK_BASE controller)
    {
        m_PakDic.Add((byte)opCode, controller);
    }

//----------------------------------------------消息相关 end------------------------------------------
}

