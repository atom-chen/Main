using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class LoginController:ControllerBase
{
  User user= new User();
  public override OperationCode msgCode
  {
    get { return OperationCode.Login; }
  }
  //发一个登录请求
  public void Login(string userName,string passWord)
  {
    user.UserName = userName;
    //user.PassWord = MD5Tool.GetMD5(passWord);
    user.PassWord = passWord;
    string json = LitJson.JsonMapper.ToJson(user);
    Dictionary<byte, object> dic = new Dictionary<byte, object>();
    dic.Add((byte)ParameterCode.User, json);

    if (PhotoEngine.Instance != null)
    {
      PhotoEngine.Instance.SendRequest(OperationCode.Login,dic);
    }
  }

  //消息处理
  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    switch((ReturnCode)response.ReturnCode)
    {
      case ReturnCode.Success:
        Debug.Log("登录成功");
        if(StartMenu.Instance!=null)
        {
          StartMenu.Instance.LoginSuccessed(user);
        }
        break;
      default:
        Debug.Log("登录失败");
        break;
    }
  }
}

