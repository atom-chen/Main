using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class LoginHandler : HandlerBase
{

  public override OperationCode OpCode
  {
    get { return OperationCode.Login; }
  }

  public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
  {
    Dictionary<byte, object> req = request.Parameters;
    object para = null;
    if (req.TryGetValue((byte)ParameterCode.User, out para))
    {
      User user = LitJson.JsonMapper.ToObject<User>(para.ToString());
      //成功
      if (UserController.Instance.Login(user))
      {
        peer.LoginUser = user;
        response.ReturnCode = (short)ReturnCode.Success;
        response.Parameters.Add((byte)ParameterCode.User, para);
      }
      //密码错误
      else
      {
        response.ReturnCode = (short)ReturnCode.Fail;
        response.Parameters.Add((byte)ParameterCode.ErrorInfo,"密码错误");
      }
    }
    else
    {
      response.ReturnCode = (short)ReturnCode.Error;
    }
  }
}
