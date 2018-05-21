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
    StartCoroutine(TryConnectLocal());
    StartCoroutine(TryConnect());
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
    Debug.Log("发一个" + opCode+"包到服务器");
    peer.OpCustom((byte)opCode, parameters, true);
  }


  public void OnEvent(EventData eventData)
  {

  }


  public void OnOperationResponse(OperationResponse operationResponse)
  {
    Debug.Log(string.Format("收到包{0}", (OperationCode)operationResponse.OperationCode));
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
        Tips.ShowTip("已连上服务器");
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
        Tips.ShowTip("连接断开");
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
        StartCoroutine(TryConnect());
        break;
      default:
        //尝试连接本地
        Tips.ShowTip("连接状态异常");
        StartCoroutine(TryConnectLocal());
        StartCoroutine(TryConnect());
        break;
    }
  }

  IEnumerator TryConnect()
  {
    while (m_State != StatusCode.Connect)
    {
      peer.Connect(m_IPAddres, m_AppName);
      yield return new WaitForSeconds(3);
    }
  }

  IEnumerator TryConnectLocal()
  {
      while (m_State != StatusCode.Connect)
      {
          peer.Connect("127.0.0.1:4530", m_AppName);
          yield return new WaitForSeconds(3);
      }
  }
}
