﻿using System.Collections;
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
                break;
            default:
                break;
        }
    }
}

public class CG_ENTER_GAME_PAK
{
    Dictionary<byte, object> dic = new Dictionary<byte, object>();

    public Tab_Server Server
    {
        set { dic.Add((byte)ParameterCode.Server, ParaTools.GetJson<Tab_Server>(value)); }
    }
    public void SendPak()
    {
        PhotoEngine.Instance.SendRequest(OperationCode.EnterGame, dic);
    }
}