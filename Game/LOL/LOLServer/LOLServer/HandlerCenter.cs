
using LOLServer.logic;
using LOLServer.logic.login;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer
{
   public class HandlerCenter:AbsHandlerCenter
    {
       HandlerInterface login;
       
       public HandlerCenter() {
           login = new LoginHandler();
       }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接了");
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接了");
        }

        public override void MessageReceive(UserToken token, object message)
        {

        }
    }
}
