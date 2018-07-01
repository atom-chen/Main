using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class EnterGameHandler : HandlerBase
{
  public override OperationCode OpCode
  {
    get { return OperationCode.EnterGame; }
  }

  //请求登录的消息
  public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
  {
    ServerPropert server = ParaTools.GetParameter<ServerPropert>(request.Parameters, ParameterCode.Server);
    if(server!=null)
    {
      peer.LoginServer = server;
      //拿到该玩家所有角色信息
      List<Role> roleList = RoleController.Instance.GetUserAllRole(peer.LoginUser.Guid);
      //传回去
      response.Parameters.Add((byte)ParameterCode.RoleList, ParaTools.GetJson<List<Role>>(roleList));
    }
  }
}

