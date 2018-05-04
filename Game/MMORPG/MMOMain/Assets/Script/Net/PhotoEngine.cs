using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoEngine : MonoBehaviour,IPhotonPeerListener {

  PhotonPeer peer;
  private StatusCode m_State = new StatusCode();//当前状态
  // Use this for initialization
  void Awake()
  {
    peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
    peer.Connect("192.168.2.102:4530", "MMOServer");
  }
  void Start()
  {

  }

  void Update()
  {
    peer.Service();
  }


  public void DebugReturn(DebugLevel level, string message)
  {

  }

  public void OnEvent(EventData eventData)
  {

  }

  public void OnOperationResponse(OperationResponse operationResponse)
  {
    Debug.Log("收到包" + operationResponse.OperationCode);

  }

  public void OnStatusChanged(StatusCode statusCode)
  {
    m_State = statusCode;
    switch (statusCode)
    {
      case StatusCode.Connect:
        Debug.Log("已连上服务器");
        break;
      default:
        Debug.Log("状态异常");
        break;
    }
  }
}
