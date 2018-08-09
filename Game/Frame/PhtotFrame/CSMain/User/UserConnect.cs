
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
    public delegate void UserEvent(UserConnect nUser);
    public event UserEvent OnUserConnect;               //建立连接时调用
    public event UserEvent OnUserDownLine;              //连接断开时调用
    //--------------------------------------------attribute---------------------------------------------------------------//
    public User LoginUser { get; set; }        //当前登录的user账号




    //--------------------------------------------attribute---------------------------------------------------------------//



    ///////////////////Connect Begin////////////////////////////////////////////////
    public UserConnect(IRpcProtocol protocol, IPhotonPeer unmanagedPeer)
        : base(protocol, unmanagedPeer)
    {
        if (OnUserConnect != null)
        {
            OnUserConnect(this);
        }

    }
    //连接断开，入库
    protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
    {
        if (OnUserDownLine != null)
        {
            OnUserDownLine(this);
        }
    }

    //消息分发函数
    protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
    {
        HandlerBase handler;
        Server.Instance.handlers.TryGetValue(operationRequest.OperationCode, out handler);//尝试去获取一个处理该operation的Handler

        OperationResponse response = new OperationResponse();
        response.OperationCode = operationRequest.OperationCode;
        response.Parameters = new Dictionary<byte, object>();
        if (handler != null)
        {
            LogManager.Info(string.Format("收到来自{0}:{1}的   {2}包", this.LocalIP, LoginUser == null ? "" : string.Format("ID={0},Name={1}", LoginUser.Guid, LoginUser.UserName), (OperationCode)operationRequest.OperationCode));
            handler.OnHandlerMessage(operationRequest, response, this, sendParameters);
            SendOperationResponse(response, sendParameters);
        }
        else
        {
            LogManager.Error("不能解释的包 " + operationRequest.OperationCode);
        }
    }

}
