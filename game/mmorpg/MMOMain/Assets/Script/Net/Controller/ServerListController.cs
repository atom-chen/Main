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
    get { throw new System.NotImplementedException(); }
  }
  public void GetServerList()
  {
    PhotoEngine.Instance.SendRequest(OperationCode.GetServer, null);
  }
  //消息处理
  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
   
  }
  public virtual void OnDestory()
  {
    base.OnDestory();
    PhotoEngine.Instance.OnConnectSuccess -= GetServerList;
  }
}
