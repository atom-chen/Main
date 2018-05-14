using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class LoginController:ControllerBase
{
  public override OperationCode msgCode
  {
    get { return OperationCode.Login; }
  }

  //消息处理
  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    switch((ReturnCode)response.ReturnCode)
    {
      case ReturnCode.Success:
        Tips.ShowTip("登录成功");
        if(StartMenu.Instance!=null)
        {
          User user = ParaTools.GetParameter<User>(response.Parameters, ParameterCode.User);
          StartMenu.Instance.LoginSuccessed(user);
        }
        break;
      default:
        object obj;
        response.Parameters.TryGetValue((byte)ParameterCode.ErrorInfo,out obj);
        Tips.ShowTip(obj.ToString());
        break;
    }
  }
}

