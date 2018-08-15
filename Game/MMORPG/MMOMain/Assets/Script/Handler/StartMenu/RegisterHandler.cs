using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CG_REGISTER_PAK
{
    public void SendPak()
    {
        PhotoEngine.Instance.SendRequest(OperationCode.Register, dic);
    }

}