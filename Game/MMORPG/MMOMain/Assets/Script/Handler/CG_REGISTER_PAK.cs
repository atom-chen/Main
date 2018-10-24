using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CG_REGISTER_PAK
{
    public void SendPak()
    {
        GameManager.NetManager.SendRequest(OperationCode.Register, dic);
    }

}