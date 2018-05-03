
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace CSMain
{
    public class Server:ApplicationBase
    {

      protected override PeerBase CreatePeer(InitRequest initRequest)
      {
        User user = new User(initRequest.Protocol, initRequest.PhotonPeer);
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


}
