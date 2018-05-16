using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class MSGTools
{
  /// <summary>
  /// 粘包长度编码
  /// </summary>
  /// <param name="buff"></param>
  /// <returns></returns>
  public static byte[] LE(byte[] buff)
  {
    MemoryStream ms = new MemoryStream();//创建内存流对象
    BinaryWriter sw = new BinaryWriter(ms);//写入二进制对象流
    //写入消息长度
    sw.Write(buff.Length);
    //写入消息体
    sw.Write(buff);
    byte[] result = new byte[ms.Length];
    Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
    sw.Close();
    ms.Close();
    return result;

  }

  /// <summary>
  /// 粘包长度解码
  /// </summary>
  /// <param name="cache"></param>
  /// <returns></returns>
  public static byte[] LD(ref List<byte> cache)
  {
    if (cache.Count < 4) return null;

    MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据写入进去
    BinaryReader br = new BinaryReader(ms);//二进制读取流
    int length = br.ReadInt32();//从缓存中读取int型消息体长度
    //如果消息体长度 大于缓存中数据长度 说明消息没有读取完 等待下次消息到达后再次处理
    if (length > ms.Length - ms.Position)
    {
      return null;
    }
    //读取正确长度的数据
    byte[] result = br.ReadBytes(length);
    //清空缓存
    cache.Clear();
    //将读取后的剩余数据写入缓存
    cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
    br.Close();
    ms.Close();
    return result;
  }

  /// <summary>
  /// 消息体序列化
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static byte[] encode(object value)
  {
    SocketModel model = value as SocketModel;
    ByteArray ba = new ByteArray();
    ba.write(model.type);
    ba.write(model.area);
    ba.write(model.command);
    //判断消息体是否为空  不为空则序列化后写入
    if (model.message != null)
    {
      ba.write(SerializeUtil.encode(model.message));
    }
    byte[] result = ba.getBuff();
    ba.Close();
    return result;
  }
  /// <summary>
  /// 消息体反序列化
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static SocketModel decode(byte[] value)
  {
    ByteArray ba = new ByteArray(value);
    SocketModel model = new SocketModel();
    byte type;
    int area;
    int command;
    //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
    ba.read(out type);
    ba.read(out area);
    ba.read(out command);
    model.type = type;
    model.area = area;
    model.command = command;
    //判断读取完协议后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
    if (ba.Readnable)
    {
      byte[] message;
      //将剩余数据全部读取出来
      ba.read(out message, ba.Length - ba.Position);
      //反序列化剩余数据为消息体
      model.message = SerializeUtil.decode(message);
    }
    ba.Close();
    return model;
  }
}

