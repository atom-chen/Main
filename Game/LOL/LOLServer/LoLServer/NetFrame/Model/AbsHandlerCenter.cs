using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  public abstract class AbsHandlerCenter
  {
    public abstract void ClientConnect(UserToken token);//连接事件
    public abstract void MessageReceive(UserToken token,object message);//消息到达
    public abstract void ClientClose(UserToken token, string error);//连接断开
  }
}
