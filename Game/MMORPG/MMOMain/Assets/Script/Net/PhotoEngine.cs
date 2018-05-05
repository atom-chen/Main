using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoEngine : MonoBehaviour,IPhotonPeerListener {
  private const string m_IPAddres = "192.168.2.102:4530";
  private const string m_AppName = "MMOServer";
  private static PhotoEngine _Instance;
  public static PhotoEngine Instance
  {
    get { return _Instance; }
  }

  PhotonPeer peer;
  private StatusCode m_State = new StatusCode();//当前状态

  private Dictionary<byte, ControllerBase> controllers = new Dictionary<byte, ControllerBase>();//逻辑模块集合

  public delegate void OnConnectStateChange();
  public event OnConnectStateChange OnConnectSuccess;//成功连接后的事件
  public event OnConnectStateChange OnConnectBreak;//连接断开后的事件

  void Awake()
  {
    _Instance = this;
    peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
    peer.Connect(m_IPAddres, m_AppName);
    DontDestroyOnLoad(this.gameObject);
  }

  void Update()
  {
    if(peer!=null)
    {
      peer.Service();
    }
  }
  public void RegisterController(OperationCode opCode, ControllerBase controller)
  {
    controllers.Add((byte)opCode, controller);
  }
  public void UnRegisterController(OperationCode opCode)
  {
    controllers.Remove((byte)opCode);
  }


  public void DebugReturn(DebugLevel level, string message)
  {

  }
  //发一个OperationCode消息
  public void SendRequest(OperationCode opCode, Dictionary<byte, object> parameters)
  {
    Debug.Log("sendrequest to server , opcode : " + opCode);
    peer.OpCustom((byte)opCode, parameters, true);
  }


  public void OnEvent(EventData eventData)
  {

  }


  public void OnOperationResponse(OperationResponse operationResponse)
  {
    Debug.Log(string.Format("收到包{0}",operationResponse.OperationCode));
    ControllerBase controller;
    controllers.TryGetValue(operationResponse.OperationCode, out controller);
    if (controller != null)
    {
      controller.OnOperationResponse(operationResponse);//将消息转发到对应Controller
    }
    else
    {
      Debug.Log("Receive a unknown response . OperationCode :" + operationResponse.OperationCode);
    }
  }

  public void OnStatusChanged(StatusCode statusCode)
  {
    m_State = statusCode;
    switch (statusCode)
    {
      case StatusCode.Connect:
        Debug.Log("已连上服务器");
        if(OnConnectSuccess!=null)
        {
          Delegate[] connectSuccessed = OnConnectSuccess.GetInvocationList();
          foreach (OnConnectStateChange onSucessed in connectSuccessed)
          {
            try
            {
              onSucessed();
            }
            catch (Exception ex)
            {
              Debug.Log(ex.Message);
            }
          }
        }
        break;
      case StatusCode.Disconnect:
        Debug.Log("连接断开");
        if(OnConnectBreak!=null)
        {
          Delegate[] connectBreak = OnConnectBreak.GetInvocationList();
          foreach (Action onBreak in connectBreak)
          {
            try
            {
              onBreak();
            }
            catch (Exception ex)
            {
              Debug.Log(ex.Message);
            }
          }
        }
        break;
      default:
        Debug.Log("状态异常");
        break;
    }
  }
}
