using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class RegisterHandler:HandlerBase
{

  public override OperationCode OpCode
  {
    get { return OperationCode.Register; }
  }

  public override void OnHandlerMessage(Photon.SocketServer.OperationRequest request, Photon.SocketServer.OperationResponse response, UserConnect peer, Photon.SocketServer.SendParameters sendParameters)
  {
    User newUser = ParaTools.GetParameter<User>(request.Parameters, ParameterCode.User);
    if(newUser!=null)
    {
      //检验合法性
      if(newUser.UserName.Length<=3)
      {
        response.ReturnCode = (short)ReturnCode.Fail;
        response.Parameters.Add((byte)ParameterCode.ErrorInfo,"未知异常");
        return;
      }
      bool isSuccess=UserManager.Register(newUser);
      //成功
      if(isSuccess)
      {
        response.ReturnCode = (short)ReturnCode.Success;
        peer.LoginUser = newUser;
        response.Parameters.Add((byte)ParameterCode.User, ParaTools.GetJson<User>(newUser));
      }
      else
      {
        response.Parameters.Add((byte)ParameterCode.ErrorInfo, "用户名重复");
        response.ReturnCode = (short)ReturnCode.Fail;
      }
    }
    else
    {
      response.ReturnCode = (short)ReturnCode.Error;
      response.Parameters.Add((byte)ParameterCode.ErrorInfo,"未知异常");
    }
  }
}

