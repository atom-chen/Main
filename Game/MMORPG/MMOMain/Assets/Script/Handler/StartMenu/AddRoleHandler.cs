using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class CG_ADD_ROLE_PAK
{
    public void SendPak()
    {
        PhotoEngine.Instance.SendRequest(OperationCode.RoleAdd, dic);
    }
}