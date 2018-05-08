using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterController : ControllerBase {
  User user;
  public override OperationCode msgCode
  {
    get { return OperationCode.Register; }
  }

  public void Register(string userName,string passWord)
  {
    user = new User() { UserName = userName, PassWord = MD5Tool.GetMD5(passWord) };

    string json = ParaTools.GetJson<User>(user);
    Dictionary<byte, object> dic = new Dictionary<byte, object>();
    dic.Add((byte)ParameterCode.User, json);
    PhotoEngine.Instance.SendRequest(OperationCode.Register, dic);
  }

  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    switch((response.ReturnCode))
    {
      case (short)ReturnCode.Success:
        //改变当前账号
        Tips.ShowTip("注册成功，已为您登录");
        StartMenu.Instance.LoginSuccessed(user);
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
