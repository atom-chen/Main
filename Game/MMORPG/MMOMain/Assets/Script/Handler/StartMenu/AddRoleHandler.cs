using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoleHandler : HandlerBase
{


    public override OperationCode msgCode
    {
        get { return OperationCode.RoleAdd; }
    }

    public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
    {
        switch (response.ReturnCode)
        {
            case (short)ReturnCode.Success:
                //添加角色成功
                LaunchSceneLogic.Instance.AddRoleSuccess(ParaTools.GetParameter<Role>(response.Parameters, ParameterCode.Role));
                Tips.ShowTip("添加角色成功");
                break;
            default:
                Tips.ShowTip(ParaTools.GetErrInfo(response.Parameters));
                break;
        }
    }
}

public class CG_ADD_ROLE_PAK
{
    Dictionary<byte, object> dic = new Dictionary<byte, object>();

    public User _User
    {
        set
        {
            value.PassWord = MD5Tools.GetMD5(value.PassWord);
            string json = LitJson.JsonMapper.ToJson(value);
            Dictionary<byte, object> dic = new Dictionary<byte, object>();
            dic.Add((byte)ParameterCode.User, json);
        }
    }
    public void SendPak()
    {
        PhotoEngine.Instance.SendRequest(OperationCode.RoleAdd, dic);
    }
}