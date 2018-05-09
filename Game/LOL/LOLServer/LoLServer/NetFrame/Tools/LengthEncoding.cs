using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  //长度编码，让接收方确认该包实际长度（处理粘包拆包问题）
  class LengthEncoding
  {
    /// <summary>
    /// 粘包长度编码
    /// </summary>
    /// <param name="buffer">原始包</param>
    /// <returns>编码后的包</returns>
    public static byte[] Encode(byte[] buffer)
    {
      MemoryStream ms=new MemoryStream();//内存流
      BinaryWriter sw = new BinaryWriter(ms);//绑定
      sw.Write(buffer.Length);//写入实际长度(int类型->4个字节)
      sw.Write(buffer);

      byte[] result = new byte[ms.Length];
      Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0,(int) ms.Length);
      sw.Close();
      ms.Close();
      return result;
    }

    /// <summary>
    /// 粘包长度解码
    /// </summary>
    /// <param name="cache">收到的包</param>
    /// <returns></returns>
    public static byte[] DeCode(ref List<byte> cache)
    {
      if(cache.Count<4)
      {
        return null;
      }
      MemoryStream ms = new MemoryStream(cache.ToArray());
      BinaryReader br = new BinaryReader(ms);
      int length = br.ReadInt32();//读取int类型的消息体长度
      //消息是否读取完
      if(length>ms.Length-ms.Position)
      {
        br.Close();
        ms.Close();
        return null;
      }

      byte[] result = br.ReadBytes(length);//读取正确长度的数据
      cache.Clear();
      cache.AddRange(br.ReadBytes((int)ms.Length - (int)ms.Position));//把内存流剩余数据写入到cache
      br.Close();
      ms.Close();
      return result;
    }
  }
}
