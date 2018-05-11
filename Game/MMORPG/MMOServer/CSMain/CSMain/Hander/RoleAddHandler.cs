using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RoleAddHandler : HandlerBase
{
  public override OperationCode OpCode
  {
    get { return OperationCode.RoleAdd; }
  }

  //添加角色的消息处理
  public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
  {
    

  }
}

