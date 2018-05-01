using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhtotServerEngine : MonoBehaviour,IPhotonPeerListener {
  PhotonPeer peer;
  private StatusCode m_State = new StatusCode();
	// Use this for initialization
  void Awake()
  {
    peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
    peer.Connect("192.168.2.102:4530", "ChatServer");
  }
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    peer.Service();
	}

  void OnGUI()
  {
    if(m_State==StatusCode.Connect)
    {
      if(GUILayout.Button("发一个包"))
      {
        Dictionary<byte, object> dic = new Dictionary<byte, object>();
        dic.Add(1, "aaa123");
        peer.OpCustom(1, dic, true);
      }
    }
  }

  public void DebugReturn(DebugLevel level, string message)
  {
    //Debug.Log("收到包"+level+"")
  }

  public void OnEvent(EventData eventData)
  {
    
  }

  public void OnOperationResponse(OperationResponse operationResponse)
  {
    Debug.Log("收到包" + operationResponse.OperationCode);
    foreach (KeyValuePair<byte, object> item in operationResponse.Parameters)
    {
      Debug.Log(item.Value);
    }
  }

  public void OnStatusChanged(StatusCode statusCode)
  {
    m_State = statusCode;
    switch(statusCode)
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
