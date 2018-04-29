using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
  //与客户端进行通信
  class ChatPeer : PeerBase
  {
    public ChatPeer(IRpcProtocol protocol, IPhotonPeer unmanagedPeer):base(protocol,unmanagedPeer)
    {
      
    }
    protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
    {

    }

    protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
    {

    }
  }
}
