using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGameControllr : ControllerBase {


  public override OperationCode msgCode
  {
    get { return OperationCode.EnterGame; }
  }

  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    //解析角色信息
    switch(response.ReturnCode)
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
