using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  public class SocketModel
  {
    /// <summary>
    /// 一级协议，区分所属模块
    /// </summary>
    public byte type{get;set;}
    /// <summary>
    /// 二级协议，区分模块下所属子模块
    /// </summary>
    public Int32 area { get; set; }
    /// <summary>
    /// 三级协议，用于当前处理逻辑功能
    /// </summary>
    public Int32 command { get; set; }
    /// <summary>
    /// 消息体
    /// </summary>
    public object message { get; set; }

    public SocketModel()
    {

    }
    public SocketModel(byte type,int area,int command,object message)
    {
      this.type = type;
      this.area = area;
      this.command = command;
      this.message = message;
    }

    public T GetMessage<T>()
    {
      return (T)message;
    }
  }
}
