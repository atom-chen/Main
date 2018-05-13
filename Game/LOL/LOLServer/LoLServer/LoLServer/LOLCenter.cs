using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace LoLServer
{
  public class LOLCenter:AbsHandlerCenter
  {

    public override void ClientConnect(UserToken token)
    {
      Console.WriteLine("一个客户端连接");
    }

    public override void ClientClose(UserToken token, string error)
    {
      Console.WriteLine("客户端连接断开");
    }

    public override void MessageReceive(UserToken token, object message)
    {
      Console.WriteLine("收到一条消息");
    }


  }
}
