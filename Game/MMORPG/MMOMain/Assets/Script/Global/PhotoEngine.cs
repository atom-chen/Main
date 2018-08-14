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
public class PhotoEngine : MonoBehaviour, IPhotonPeerListener
{
    private static string m_IPAddres = "";
    private static string m_AppName = "";
    private static PhotoEngine _Instance;
    public static PhotoEngine Instance
    {
        get { return _Instance; }
    }

    PhotonPeer peer;
    private StatusCode m_State = new StatusCode();//当前状态

    void Awake()
    {
        _Instance = this;
        peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
        DontDestroyOnLoad(this.gameObject);
    }

    void FixelUpdate()
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

    private void TryConnect(Tab_Server server)
    {
        if (server == null || _Instance == null) return;
        m_IPAddres = string.Format("{0:{1}", server.ipAddress, server.port.ToString());
        m_AppName = server.appName;
        _Instance.StartCoroutine(_Instance.TryConnect());
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
    private Dictionary<byte, HandlerBase> controllers = new Dictionary<byte, HandlerBase>();//逻辑模块集合
    private Queue<MsgPair> m_Cache = new Queue<MsgPair>();

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log(string.Format("收到包{0}", (OperationCode)operationResponse.OperationCode));
        HandlerBase controller;
        controllers.TryGetValue(operationResponse.OperationCode, out controller);
        if (controller != null)
        {
            controller.OnOperationResponse(operationResponse);//将消息转发到对应Handler
        }
        else
        {
            Debug.Log("Receive a unknown response . OperationCode :" + operationResponse.OperationCode);
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
            TryConnect(PlayData.ServerData);
            m_Cache.Enqueue(new MsgPair(opCode, parameters));
        }
    }
    //注册Handler
    public void RegisterController(OperationCode opCode, HandlerBase controller)
    {
        controllers.Add((byte)opCode, controller);
    }

    //注销Handler
    public void UnRegisterController(OperationCode opCode)
    {
        controllers.Remove((byte)opCode);
    }
//----------------------------------------------消息相关 end------------------------------------------
}

