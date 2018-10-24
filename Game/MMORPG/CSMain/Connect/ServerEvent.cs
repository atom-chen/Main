using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;

namespace CSMain
{
    public partial class Server : ApplicationBase
    {
        public delegate void ServerEvent();
        public static event ServerEvent OnStartUp;
        public static event ServerEvent OnTeamDown;



        //有玩家连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogManager.Info(string.Format("获得连接{0}", initRequest.LocalIP.ToString()));
            Connect conn = new Connect(initRequest.Protocol, initRequest.PhotonPeer);
            return conn;
        }

        //启服
        protected override void Setup()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "TD" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
            LogManager.Info("启服成功");

            if (OnStartUp != null)
            {
                OnStartUp();
            }
            Init();
        }

        //关服务->将在线的所有用户的数据存入
        protected override void TearDown()
        {
            if (OnTeamDown != null)
            {
                OnTeamDown();
            }
            LogManager.Info("服务器关闭");
        }

    }


}




