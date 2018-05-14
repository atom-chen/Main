using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterController : ControllerBase {
  public override OperationCode msgCode
  {
    get { return OperationCode.Register; }
  }

  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    switch((response.ReturnCode))
    {
      case (short)ReturnCode.Success:
        //改变当前账号
        Tips.ShowTip("注册成功，已为您登录");
        if (StartMenu.Instance != null)
        {
          User user = ParaTools.GetParameter<User>(response.Parameters, ParameterCode.User);
          StartMenu.Instance.LoginSuccessed(user);
        }
        break;
      case (short)ReturnCode.Fail:
        //获取提示信息
        object error;
        response.Parameters.TryGetValue((byte)ParameterCode.ErrorInfo,out error);
        //show error
        Tips.ShowTip(error.ToString());
        break;
    }
  }
}
