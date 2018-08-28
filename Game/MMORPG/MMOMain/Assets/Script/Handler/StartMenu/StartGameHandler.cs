using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class CG_START_GAME_PAK
{
    public void SendPak()
    {
        //发送进入游戏的包
        if (PhotoEngine.Instance != null)
        {
            PhotoEngine.Instance.SendRequest(OperationCode.StartGame, dic);
        }
    }
}