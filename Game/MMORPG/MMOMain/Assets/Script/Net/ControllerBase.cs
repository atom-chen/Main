using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour {

  public abstract OperationCode msgCode { get; }

  public virtual void Start()
  {
    PhotoEngine.Instance.RegisterController(msgCode, this);
  }
  public virtual void OnDestory()
  {
    PhotoEngine.Instance.UnRegisterController(msgCode);
  }

  //逻辑模块的消息处理函数
  public abstract void OnOperationResponse(OperationResponse response);
}
