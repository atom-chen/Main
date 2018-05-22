using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameControllr : ControllerBase {


  public override OperationCode msgCode
  {
    get { return OperationCode.StartGame; }
  }

  public override void OnOperationResponse(ExitGames.Client.Photon.OperationResponse response)
  {
    //解析角色信息
    switch(response.ReturnCode)
    {
      case (short)ReturnCode.Success:
        //解析角色信息
        Role role = ParaTools.GetParameter<Role>(response.Parameters, ParameterCode.Role);
        PlayData.RoleData = role;
        //切换场景
        SceneManager.LoadScene(1);
        break;
      default:
        break;
    }
  }
}
