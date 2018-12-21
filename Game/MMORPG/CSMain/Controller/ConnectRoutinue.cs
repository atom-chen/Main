using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConnectRoutinue:RoutinueBase
{
    private static Dictionary<string, Connect> m_ConnDic = new Dictionary<string, Connect>();

    private static Queue<CG_ENTER_GAME_PAK> m_EnterGameReq = new Queue<CG_ENTER_GAME_PAK>();        //登录请求
    private static Queue<CG_REGISTER_PAK> m_RegisterReq = new Queue<CG_REGISTER_PAK>();        //注册请求
    public override void Tick()
    {
        if(m_EnterGameReq.Count > 0)
        {
            CG_ENTER_GAME_PAK pak = m_EnterGameReq.Dequeue();
            MD_LoginMsg msg = new MD_LoginMsg();
            msg.LoginUserName = pak.userName;
            msg.LoginPassword = pak.passWord;
            msg.IPAddrea = pak.SenderIp;
            DBRoutinue.SendMsgToDB(msg);
        }
        if(m_RegisterReq.Count > 0)
        {
            CG_REGISTER_PAK pak = m_RegisterReq.Dequeue();
            MD_RegisterMsg msg = new MD_RegisterMsg();
            msg.UserName = pak._User.UserName;
            msg.Password = pak._User.PassWord;
            msg.IPAddrea = pak.SenderIp;
            DBRoutinue.SendMsgToDB(msg);
        }
    }

    //接收登录请求
    public static void ReceiveEnterGamePak(CG_ENTER_GAME_PAK package)
    {
        m_EnterGameReq.Enqueue(package);
    }

    //接收注册请求
    public static void ReceiveRegisterPak(CG_REGISTER_PAK package)
    {
        m_RegisterReq.Enqueue(package);
    }

    //登录成功，数据进入UserRoutinue
    public static void ReceiverMsg(DM_UserConnectMsg msg)
    {
        Connect conn = null;
        if (m_ConnDic.TryGetValue(msg.RequesterIP,out conn))
        {
            m_ConnDic.Remove(msg.RequesterIP);
            UserRoutinue.LoginSuccess(msg, conn);
        }
    }
}

