using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  public class Serialize
  {
    /// <summary>
    /// 对象序列化工具
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EnCode(object value)
    {
       MemoryStream ms;
      BinaryFormatter bw;
      try
      {
        ms = new MemoryStream();
        bw = new BinaryFormatter();//二进制序列化对象
        bw.Serialize(ms, value);      //将obj对象序列化成二进制数据 写入到内存流
        byte[] result = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0,(int) ms.Length);
        ms.Close();
        return result;
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return null;
    }

    /// <summary>
    /// 反序列化工具
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object DoCode(byte[] value)
    {
      MemoryStream ms;
      BinaryFormatter bw;
      try
      {
        ms = new MemoryStream();
        ms.Write(value, 0, value.Length);     //往流中写入byte数据
        bw = new BinaryFormatter();
        object result=bw.Deserialize(ms);      //反序列化
        ms.Close();
        return result;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return null;
    }
  }
}
