﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace NetFrame
{
  public class ServerStart
  {
    Socket m_ServerSocket;
    int m_MaxClient;                

    Semaphore m_AcceptClients; //信号量

    UserTokenPool m_UserPool;//连接池

    public LengthDecode m_LDecode;
    public LengthEncode m_LEncode;
    public Encode m_Encode;
    public Decode m_Decode;

    /// <summary>
    /// 消息处理中心，由外部应用传入
    /// </summary>
    public AbsHandlerCenter m_HandlerCenter;

    /// <summary>
    /// 启服
    /// </summary>
    /// <param name="maxConnect">最大连接数</param>
    public ServerStart(int port,AbsHandlerCenter center,int maxConnect=5000)
    {
      m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//socket
      this.m_HandlerCenter = center;//center
      m_MaxClient = maxConnect;
      m_AcceptClients = new Semaphore(maxConnect, maxConnect);//初始化 Semaphore 类的新实例，并指定初始入口数和最大并发入口数。
      m_LEncode = LengthEncoding.Encode;//le
      m_LDecode = LengthEncoding.DeCode;//ld
      m_Encode = MessageEncodeing.Encode;//e
      m_Decode = MessageEncodeing.DeCode;//d



      m_UserPool = new UserTokenPool(maxConnect);//初始化连接池
      for (int i = 0; i < maxConnect; i++)
      {
        UserToken token = new UserToken();
        token.m_ReceiveSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息接收处理函数
        token.m_SendSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息发送处理函数
        token.m_LDecode = m_LDecode;
        token.m_LEncode = m_LEncode;
        token.m_Decode = m_Decode;
        token.m_Encode = m_Encode;
        token.m_CloseProcess = ClientDown;
        token.m_SendProcess = ProcessSend;
        token.m_HandlerCenter = m_HandlerCenter;
        m_UserPool.Push(token);
      }
      m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));      //socket监听当前服务器网卡所有可用IP地址10001端口->外网IP和内网IP
      m_ServerSocket.Listen(10);                                     //10：挂起的连接队列的最大长度->队列阻塞之后，最多允许连接10个

      //等待连接
      Accept();
    }

    public ServerStart(int port, AbsHandlerCenter center, int maxConnect,LengthEncode lEncode,LengthDecode lDecode,Encode encode,Decode decode)
    {
      m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.m_HandlerCenter = center;
      m_MaxClient = maxConnect;
      m_AcceptClients = new Semaphore(maxConnect, maxConnect);//初始化 Semaphore 类的新实例，并指定初始入口数和最大并发入口数。
      this.m_Encode = encode;
      this.m_Decode = decode;
      this.m_LEncode = lEncode;
      this.m_LDecode = lDecode;

      m_UserPool = new UserTokenPool(maxConnect);//初始化连接池
      for (int i = 0; i < maxConnect; i++)
      {
        UserToken token = new UserToken();
        token.m_ReceiveSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息接收处理函数
        token.m_SendSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息发送处理函数
        token.m_LDecode = m_LDecode;
        token.m_LEncode = m_LEncode;
        token.m_Decode = m_Decode;
        token.m_Encode = m_Encode;
        token.m_CloseProcess = ClientDown;
        token.m_SendProcess = ProcessSend;
        token.m_HandlerCenter = m_HandlerCenter;
        m_UserPool.Push(token);
      }
      m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));      //socket监听当前服务器网卡所有可用IP地址10001端口->外网IP和内网IP
      m_ServerSocket.Listen(10);                                     //10：挂起的连接队列的最大长度->队列阻塞之后，最多允许连接10个

      //等待连接
      Accept();
    }


#region 建立连接
    /// <summary>
    /// 连接循环：为空连接分配内存，为已存在连接释放内存
    /// </summary>
    /// <param name="socket">若为空则表示建立一个连接，若不为空则表示释放一个连接</param>
    private void Accept(SocketAsyncEventArgs socket=null)//SocketAsyncEventArgs->异步socket
    {   
      if (socket == null)
      {
        socket = new SocketAsyncEventArgs();
        socket.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted);
      }
      else
      {
        socket = null;
      }
      m_AcceptClients.WaitOne();     //信号量--,若小于1则此处发生阻塞

      if (!m_ServerSocket.AcceptAsync(socket))
      {
        ProcessAccept(socket);         
      }
    }

    //连接处理函数
    private void ProcessAccept(SocketAsyncEventArgs socket)
    {
      UserToken token = m_UserPool.Pop();
      token.m_Connect = socket.AcceptSocket;
      // TODO 通知应用层有客户端连接
      m_HandlerCenter.ClientConnect(token);
      StartReceive(token);    //建立连接后开启监听
      Accept(socket);
    }

    //一个连接断开
    private void ClientDown(UserToken token, string error)
    {
      if (token.m_Connect != null)
      {
        lock (token)
        {
          //通知应用层
          m_HandlerCenter.ClientClose(token, error);

          token.Close();
          m_UserPool.Push(token);
          m_AcceptClients.Release();//信号量++
        }
      }
    }
    //异步连接建立的回调
    private void Accept_Comleted(object sender, SocketAsyncEventArgs socket)
    {
      ProcessAccept(socket);
    }
#endregion


#region 消息处理
    //消息处理函数
    private void ProcessReceive(SocketAsyncEventArgs socket)
    {
      UserToken token = socket.UserToken as UserToken;
      if (token != null)
      {
        //网络消息接收成功
        if (token.m_ReceiveSAEA.BytesTransferred > 0 && token.m_ReceiveSAEA.SocketError == SocketError.Success)
        {
          byte[] msg = new byte[token.m_ReceiveSAEA.BytesTransferred];
          Buffer.BlockCopy(token.m_ReceiveSAEA.Buffer, 0, msg, 0, msg.Length);
          token.Receive(msg);
          StartReceive(token);//继续监听
        }
        else
        {
          if (token.m_ReceiveSAEA.SocketError != SocketError.Success)
          {
            //读取报错
            ClientDown(token, token.m_ReceiveSAEA.SocketError.ToString());
          }
          else
          {
            //长度为0-》客户端异常断开
            ClientDown(token, "客户端主动断开连接");
          }
        }
      }

    }
    /// <summary>
    /// 消息监听循环
    /// </summary>
    /// <param name="token">开启监听的user</param>
    private void StartReceive(UserToken token)
    {
      try
      {
        bool result = token.m_Connect.ReceiveAsync(token.m_ReceiveSAEA);
        if (!result)
        {
          ProcessReceive(token.m_ReceiveSAEA);
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    //接收/发送消息的回调
    private void IO_Comleted(object sender, SocketAsyncEventArgs socket)
    {
      switch(socket.LastOperation)
      {
        case SocketAsyncOperation.Receive:
          ProcessReceive(socket);
          break;
        case SocketAsyncOperation.Send:
          ProcessSend(socket);
          break;
      }
    }


    //异步发送消息的回调
    private void ProcessSend(SocketAsyncEventArgs socket)
    {
      UserToken token = socket.UserToken as UserToken;
      if(token!=null)
      {
        if(socket.SocketError==SocketError.Success)
        {
          token.SendMagSuccessedCallBack();
        }
        else
        {
          ClientDown(token, socket.SocketError.ToString());
        }
      }
    }
#endregion


  }
}
