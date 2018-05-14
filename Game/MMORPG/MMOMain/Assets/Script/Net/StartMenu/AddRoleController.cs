using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoleController : ControllerBase  {


  public override OperationCode msgCode
  {
    get { return OperationCode.RoleAdd; }
  }

  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    switch(response.ReturnCode)
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
