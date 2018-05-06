﻿using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ServerListController : ControllerBase {

  public virtual void Start()
  {
    base.Start();
  }
  void OnEnable()
  {
    GetServerList();
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
    Dictionary<byte, object> parameters = response.Parameters;
    object json = null;
    if(parameters.TryGetValue((byte)ParameterCode.ServerList,out json))
    {
      List<ServerProperty> serverList = JsonMapper.ToObject<List<ServerProperty>>(json.ToString());
      if(serverList!=null)
      {
        StartMenu.Instance.SetServerList(serverList);
      }
    }
  }

  public virtual void OnDestory()
  {
    base.OnDestory();
  }
}
