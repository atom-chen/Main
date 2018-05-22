using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class StartGameHandler : HandlerBase
{
  public override OperationCode OpCode
  {
    get { return OperationCode.StartGame; }
  }

  //选定角色，开始游戏
  public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
  {
    Role role = ParaTools.GetParameter<Role>(request.Parameters, ParameterCode.Role);
    if (role != null)
    {
      if(RoleController.Instance.RoleOnline(role))
      {
        peer.LoginRole = role;
        //传回去
        response.Parameters.Add((byte)ParameterCode.RoleList, ParaTools.GetJson<Role>(role));
      }
    }
  }
}

