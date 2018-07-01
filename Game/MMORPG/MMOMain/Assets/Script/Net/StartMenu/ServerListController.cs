using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ServerListController : ControllerBase {


  public override OperationCode msgCode
  {
    get { return OperationCode.GetServer; } //表示是获取服务器消息的码
  }


  //消息处理
  public override void OnOperationResponse(OperationResponse response)
  {
    Dictionary<byte, object> parameters = response.Parameters;
    object json = null;
    if(parameters.TryGetValue((byte)ParameterCode.ServerList,out json))
    {
        List<ServerPropert> serverList = JsonMapper.ToObject<List<ServerPropert>>(json.ToString());
      if(serverList!=null)
      {
        StartMenu.Instance.SetServerList(serverList);
      }
    }
  }
}
