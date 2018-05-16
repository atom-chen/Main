using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// 客户端网络连接：同步
/// </summary>
public class NetIO
{
  private static NetIO _Instance = new NetIO();
  public static NetIO Instance
  {
    get
    {
      return _Instance;
    }
  }
  private const string ip = "192.168.2.102";
  private const int port = 9999;
  private Socket socket;

  private byte[] readbuff = new byte[1024];

  List<byte> cache = new List<byte>();
  List<SocketModel> messages = new List<SocketModel>();//消息体列表

  private bool isReading = false;
  private bool isWriting = false;
  private NetIO()
  {
    try
    {
      //创建客户端连接
      socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      //连接到服务器
      socket.Connect(ip, port);
      //开启异步消息接收 消息到达后会直接写入缓冲区 readbuff
      socket.BeginReceive(readbuff, 0, readbuff.Length, SocketFlags.None, ReceiveCallBack, readbuff);
    }
    catch (Exception ex)
    {
      Debug.LogError(ex.Message);
    }
  }

  //收到消息的回调
  private void ReceiveCallBack(IAsyncResult response)
  {
    try
    {
      //获取当前收到的消息长度
      int length = socket.EndReceive(response);
      byte[] message = new byte[length];
      Buffer.BlockCopy(readbuff, 0, message, 0, length);
      cache.AddRange(message);
      if (!isReading)
      {
        isReading = true;
        onData();
      }
      socket.BeginReceive(readbuff, 0, readbuff.Length, SocketFlags.None, ReceiveCallBack, readbuff);
    }
    catch (Exception ex)
    {
      Debug.Log("服务器主动断开连接" + ex.Message);
      socket.Close();
    }
    
  }

  void onData()
  {
    try
    {
      //长度解码返回空说明消息体不全，等待下条消息过来补全
      byte[] result = MSGTools.LD(ref cache);
      if (result == null)
      {
        isReading = false;
        return;
      }
      SocketModel message = MSGTools.decode(result);
      if (message == null)
      {
        isReading = false;
        return;
      }

      //进行消息的处理
      messages.Add(message);

      //尾递归 防止在消息处理过程中 有其他消息到达而没有经过处理
      onData();
    }
    catch(Exception ex)
    {
      Debug.LogError(ex.Message);
    }

  }

  public void write(byte type,int area,int command,object message)
  {
    SocketModel model=new SocketModel();
    model.type=type;
    model.area=area;
    model.command=command;
    model.message=message;
    byte[] msg=MSGTools.encode(model);
  }


}
