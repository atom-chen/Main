﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using System.Reflection;
using ExitGames.Logging;

namespace CSMain
{
  public class Server : ApplicationBase
  {
    private static Server _instance;
    public static Server Instance
    {
      get { return _instance; }
    }

    public Dictionary<byte, HandlerBase> handlers = new Dictionary<byte, HandlerBase>();//byte表示OperationCode
    public Server()
    {
       _instance = this;
       RegisteHandlers();
    }

    private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
    protected override PeerBase CreatePeer(InitRequest initRequest)
    {
      UserConnect user = new UserConnect(initRequest.Protocol, initRequest.PhotonPeer);
      return user;
    }

    //启服
    protected override void Setup()
    {

    }

    //关服务->将在线的所有用户的数据存入
    protected override void TearDown()
    {

    }
    

    void RegisteHandlers()
    {
      //用反射机制注册所有HandlerBase
      Type[] types = Assembly.GetAssembly(typeof(HandlerBase)).GetTypes();
      foreach (var type in types)
      {
        if (type.FullName.EndsWith("Handler"))
        {
          Activator.CreateInstance(type);
        }
      }
    }
  }
}




