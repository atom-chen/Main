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
    //相应客户端发起的请求
    protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
    {
      //消息转发
      switch(operationRequest.OperationCode)
      {
        
      }
      OperationResponse res = new OperationResponse(1, null);
      //给客户端响应
      SendOperationResponse(res, sendParameters);
    }
  }
}
