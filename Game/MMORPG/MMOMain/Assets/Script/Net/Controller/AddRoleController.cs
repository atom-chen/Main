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

  }
}
