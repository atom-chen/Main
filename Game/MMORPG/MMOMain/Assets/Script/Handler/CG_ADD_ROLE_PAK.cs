using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class CG_ADD_ROLE_PAK
{
    public void SendPak()
    {
        GameManager.NetManager.SendRequest(OperationCode.RoleAdd, dic);
    }
}