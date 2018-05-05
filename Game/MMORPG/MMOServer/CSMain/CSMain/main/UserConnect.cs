
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class UserConnect : PeerBase
{

  public UserConnect(IRpcProtocol protocol, IPhotonPeer unmanagedPeer)
    : base(protocol, unmanagedPeer)
  {

  }
  protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
  {

  }

  protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
  {
    switch (operationRequest.OperationCode)
    {

    }
  }


  //客户端消息处理函数
  private bool Login(string userName, string password)
  {
    return false;
  }

}
