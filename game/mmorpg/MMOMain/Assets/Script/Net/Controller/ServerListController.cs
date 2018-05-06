using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerListController : ControllerBase {

  public virtual void Start()
  {
    base.Start();
    PhotoEngine.Instance.OnConnectSuccess += GetServerList;
  }

  public override OperationCode msgCode
  {
    get { return OperationCode.GetServer; } //表示是获取服务器消息的码
  }
  public void GetServerList()
  {
    PhotoEngine.Instance.SendRequest(OperationCode.GetServer, null);
  }
  //消息处理
  public override void OnOperationResponse(OperationResponse response)
  {
   
  }
  public virtual void OnDestory()
  {
    base.OnDestory();
    PhotoEngine.Instance.OnConnectSuccess -= GetServerList;
  }
}
