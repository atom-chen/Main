
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.logic.login
{
    class LoginHandler:HandlerInterface
    {
        public void ClientClose(NetFrame.UserToken token, string error)
        {
           
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {

        }



        public void ClientConnect(NetFrame.UserToken token)
        {
            
        }
    }
}
