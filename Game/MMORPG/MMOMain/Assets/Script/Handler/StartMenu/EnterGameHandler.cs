using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameHandler : HandlerBase
{


    public override OperationCode msgCode
    {
        get { return OperationCode.EnterGame; }
    }

    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        //解析角色信息
        switch (response.ReturnCode)
        {
            case (short)ReturnCode.Success:
                //解析角色信息
                List<Role> roleList = ParaTools.GetParameter<List<Role>>(response.Parameters, ParameterCode.RoleList);
                LaunchSceneLogic.Instance.SetRoleList(roleList);
                Tips.ShowTip("登录成功");

                //User信息
                if (StartMenu.Instance != null)
                {
                    User user = ParaTools.GetParameter<User>(response.Parameters, ParameterCode.User);
                    StartMenu.Instance.LoginSuccessed(user);
                }
                break;
            default:
                object obj;
                response.Parameters.TryGetValue((byte)ParameterCode.ErrorInfo, out obj);
                Tips.ShowTip(obj.ToString());
                break;
        }
    }
}

public partial class CG_ENTER_GAME_PAK
{
    public void SendPak()
    {
        PhotoEngine.Instance.SendRequest(OperationCode.EnterGame, dic);
    }
}