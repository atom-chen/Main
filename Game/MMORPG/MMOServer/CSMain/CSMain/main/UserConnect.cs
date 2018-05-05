
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
  //连接断开，入库
  protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
  {

  }

  protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
  {
    switch (operationRequest.OperationCode)
    {
      case (byte)OperationCode.GetServer:
        List<ServerPropert> serverList=ServerPropertyController.Instance.GetAllServerPropert();
        OperationResponse res = new OperationResponse(1, serverList);
        SendOperationResponse(res, sendParameters);
        break;
      case (byte)OperationCode.Login:
       //User user=UserController.Instance.Login()
        break;
    }
  }


  //一条连接的消息处理函数
  private bool Login(string userName, string password)
  {
    return false;
  }

}
