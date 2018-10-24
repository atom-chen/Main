using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class CG_ENTER_GAME_PAK
{
    public void SendPak()
    {
        GameManager.NetManager.SendRequest(OperationCode.EnterGame, dic);
    }
}