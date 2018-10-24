using CSMain;
using ExitGames.Logging;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Connect : PeerBase
{
    public delegate void UserEvent(User nUser);
    public event UserEvent OnUserConnect;               //建立连接时调用
    public event UserEvent OnUserDownLine;              //连接断开时调用
    //--------------------------------------------attribute---------------------------------------------------------------//
    private User m_LoginUser;
    public User LoginUser { get { return m_LoginUser; } private set { m_LoginUser = value; } }        //当前登录的user账号




    //--------------------------------------------attribute---------------------------------------------------------------//



    ///////////////////Connect Begin////////////////////////////////////////////////
    public Connect(IRpcProtocol protocol, IPhotonPeer unmanagedPeer)
        : base(protocol, unmanagedPeer)
    {
        if (OnUserConnect != null)
        {
            OnUserConnect(this.LoginUser);
        }
    }
    //连接断开，入库
    protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
    {
        if (OnUserDownLine != null)
        {
            OnUserDownLine(this.LoginUser);
        }
        if (LoginUser != null)
        {
            if (LoginUser.RoleData != null)
            {
                RoleManager.RoleDownLine(LoginUser.RoleData);
                LoginUser.RoleData = null;
            }
            //UserManager.DownLine(LoginUser);
            LogManager.Debug("玩家{0}下线", LoginUser.UserName);
            LoginUser = null;
        }
    }

    //消息分发函数
    protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
    {
        LogManager.Info(string.Format("收到来自{0}:{1}的   {2}包", this.LocalIP, LoginUser == null ? "" : 
            string.Format("ID={0},Name={1}", LoginUser.Guid, LoginUser.UserName), (OperationCode)operationRequest.OperationCode));
        CG_FactoryBase factory;
        Server.mFactaryDic.TryGetValue(operationRequest.OperationCode, out factory);//尝试去获取一个处理该operation的Handler
        if(factory!=null)
        {
            CG_PAK_BASE pak = factory.GetPak();
            if (pak != null)
            {
                pak.SetDic(operationRequest.Parameters);
                if (m_LoginUser !=null)
                {
                    pak.SenderId = m_LoginUser.Guid;
                }
                pak.SenderIp = this.LocalIP;
                 //如果是登录包或注册包
                switch(factory.OpCode)
                {
                    case OperationCode.EnterGame:
                        CG_ENTER_GAME_PAK temp = pak as CG_ENTER_GAME_PAK;
                        if(temp != null)
                        {
                            ConnectRoutinue.ReceiveEnterGamePak(temp);
                        }
                        break;
                    case OperationCode.Register:
                        CG_REGISTER_PAK temp2 = pak as CG_REGISTER_PAK;
                        if(temp2 != null)
                        {
                            ConnectRoutinue.ReceiveRegisterPak(temp2);
                        }
                        break;
                }
                if (factory.OpCode < OperationCode.MAX)
                {
                    UserRoutinue.ReceiveCGPak(pak);
                }
            }
            else
            {
                LogManager.Error("不能解释的包 " + operationRequest.OperationCode);
            }
            //回收
            factory.GCPak(pak);
        }
    }
}
