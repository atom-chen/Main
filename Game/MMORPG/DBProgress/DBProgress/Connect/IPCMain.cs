using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


partial class IPCMain
{
    static void Main(string[] args)
    {
        IpcServerChannel channel = new IpcServerChannel("MainDB");
        ChannelServices.RegisterChannel(channel, false);
        RemotingConfiguration.RegisterWellKnownServiceType(typeof(Main2DB), "Main2DB", WellKnownObjectMode.Singleton);
        RemotingConfiguration.RegisterWellKnownServiceType(typeof(DB2Main), "DB2Main", WellKnownObjectMode.Singleton);

        Thread pid = new Thread(RoutninueMain);        //接收消息线程
        pid.Start();

        Thread routinueMain = new Thread(ControllerMain);        //处理tick线程
        pid.Start();
    }

    static void RoutninueMain()
    {
        while (true)
        {
            if (Main2DB.qMessage != null)
            {
                if (Main2DB.qMessage.Count > 0)
                {
                    MD_MsgBase message = Main2DB.qMessage.Dequeue();
                    switch (message.code)
                    {
                        case MD_MsgCode.LOGIN:
                            ReceiveMsg((MD_LoginMsg)message);
                            break;
                        case MD_MsgCode.REGISTER:
                            ReceiveMsg((MD_RegisterMsg)message);
                            break;
                        case MD_MsgCode.ROLE_ADD:
                            ReceiveMsg((MD_RoleAddMsg)message);
                            break;
                    }
                }
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }

    static void ControllerMain()
    {
        while(true)
        {

        }
    }
}

