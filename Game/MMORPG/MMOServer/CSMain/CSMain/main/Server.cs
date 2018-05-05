
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;


    public class Server:ApplicationBase
    {

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
    }



