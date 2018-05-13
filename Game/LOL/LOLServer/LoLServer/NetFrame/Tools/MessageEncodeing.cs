using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  public class MessageEncodeing
  {
    /// <summary>
    /// 消息体编码
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] Encode(object value)
    {
      SocketModel model = value as SocketModel;
      if(model!=null)
      {
        ByteArray ba = new ByteArray();
        ba.write(model.type);
        ba.write(model.area);
        ba.write(model.command);
        if(model.message!=null)
        {
          ba.write(Serialize.EnCode(model.message));
        }
        byte[] result = ba.getBuff();
        ba.Close();
        return result;
      }
      return null;
    }

    /// <summary>
    /// 消息体解码
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object DeCode(byte[] value)
    {
      ByteArray ba = new ByteArray(value);
      SocketModel model = new SocketModel();
      byte type;
      int area, command;
      //写入顺序和读取顺序必须一致
      ba.read(out type);
      ba.read(out area);
      ba.read(out command);
      model.type = type;
      model.area = area;
      model.command = command;
      if(ba.Readnable)
      {
        //有消息体
        byte[] message;
        ba.read(out message,ba.Length-ba.Position);
        model.message = Serialize.DoCode(message);//反序列化为object
      }
      ba.Close();
      return model;
    }
  }
}
