
using CSMain;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserConnect : PeerBase
{
  private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

  public User LoginUser { get; set; }//存储当前登录的user账号

  public ServerPropert LoginServer { get; set; }
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
    HandlerBase handler;
    Server.Instance.handlers.TryGetValue(operationRequest.OperationCode, out handler);//尝试去获取一个处理该operation的Handler

    OperationResponse response = new OperationResponse();
    response.OperationCode = operationRequest.OperationCode;
    response.Parameters = new Dictionary<byte, object>();
    if (handler != null)
    {
      log.Info(string.Format("收到来自{0}:{1}的   {2}包", this.LocalIP, LoginUser==null?"":string.Format("ID={0},Name={1}",LoginUser.Guid,LoginUser.UserName), (OperationCode)operationRequest.OperationCode));
      handler.OnHandlerMessage(operationRequest, response, this, sendParameters);
      SendOperationResponse(response, sendParameters);
    }
    else
    {
      log.Debug("不能解释的包 " + operationRequest.OperationCode);
    }
  }


  //一条连接的消息处理函数
  private bool Login(string userName, string password)
  {
    return false;
  }

}
