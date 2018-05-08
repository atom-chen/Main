using System;
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

    /// <summary>
    /// 启服
    /// </summary>
    /// <param name="maxConnect">最大连接数</param>
    public ServerStart(int port,int maxConnect=100)
    {
      m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      m_MaxClient = maxConnect;
      m_AcceptClients = new Semaphore(maxConnect, maxConnect);//初始化 Semaphore 类的新实例，并指定初始入口数和最大并发入口数。
      m_UserPool = new UserTokenPool(maxConnect);//初始化连接池
      for (int i = 0; i < maxConnect; i++)
      {
        UserToken token = new UserToken();
        token.m_ReceiveSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息接收处理函数
        token.m_SendSAEA.Completed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(IO_Comleted);//消息发送处理函数
        m_UserPool.Push(token);
      }

      m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));      //socket监听当前服务器网卡所有可用IP地址10001端口->外网IP和内网IP

      m_ServerSocket.Listen(10);                                     //10：挂起的连接队列的最大长度->队列阻塞之后，最多允许连接10个

      //等待连接
      Accept();
    }
#region 建立连接
    //等待连接
    public void Accept(SocketAsyncEventArgs socket=null)//SocketAsyncEventArgs->异步socket
    {
      //如果当前传入为空，说明调用新的客户端连接监听事件     
      if (socket == null)
      {
        socket = new SocketAsyncEventArgs();
        socket.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted);//连接成功后的回调
      }
      else
      {
        socket = null;//否则移除当前客户端连接
      }
      m_AcceptClients.WaitOne();     //信号量--,若小于1则此处发生阻塞

      if (!m_ServerSocket.AcceptAsync(socket))
      {
        //如果没有挂起，则立即连接。
        ProcessAccept(socket);         
      }
    }

    /// <summary>
    /// 开启消息监听
    /// </summary>
    /// <param name="token">开启监听的user</param>
    public void StartReceive(UserToken token)
    {
      bool result=token.m_Connect.ReceiveAsync(token.m_ReceiveSAEA);
      //如果没有挂起 开启异步数据接收
      if(!result)
      {
        ProcessReceive(token.m_ReceiveSAEA);
      }
    }


    //正式建立连接
    public void ProcessAccept(SocketAsyncEventArgs socket)
    {
      UserToken token = m_UserPool.Pop();
      token.m_Connect = socket.AcceptSocket;
      // TODO 通知应用层有客户端连接
      StartReceive(token);    //建立连接后开启监听
      Accept(socket);   //将当前异步对象置位空
    }
    //一个连接断开
    public void ClientDown(UserToken token, string error)
    {
      if (token.m_Connect != null)
      {
        lock (token)
        {
          //通知应用层 TODO

          token.Close();
          m_UserPool.Push(token);
          m_AcceptClients.Release();//信号量++
        }
      }
    }
    //异步连接建立的回调
    public void Accept_Comleted(object sender, SocketAsyncEventArgs socket)
    {
      ProcessAccept(socket);
    }
#endregion


#region 消息处理
    //接收/发送消息的回调
    public void IO_Comleted(object sender, SocketAsyncEventArgs socket)
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

    //异步接收消息的回调
    public void ProcessReceive(SocketAsyncEventArgs socket)
    {
      UserToken token = socket.UserToken as UserToken;
      if(token!=null)
      {
        //网络消息接收成功
        if(token.m_ReceiveSAEA.BytesTransferred>0 && token.m_ReceiveSAEA.SocketError==SocketError.Success)
        {
          byte[] msg=new byte[token.m_ReceiveSAEA.BytesTransferred];
          Buffer.BlockCopy(token.m_ReceiveSAEA.Buffer, 0, msg,0, msg.Length);
          token.ReceiveMsg(msg);
          StartReceive(token);
        }
        else
        {
          if(token.m_ReceiveSAEA.SocketError!=SocketError.Success)
          {
            //读取报错
            ClientDown(token, token.m_ReceiveSAEA.SocketError.ToString());
          }
          else
          {
            //长度为0-》客户端异常断开
            ClientDown(token,"客户端主动断开连接");
          }
        }
      }

    }
    //异步发送消息的回调
    public void ProcessSend(SocketAsyncEventArgs socket)
    {
      UserToken token = socket.UserToken as UserToken;
      if(token!=null)
      {
        if(socket.SocketError!=SocketError.Success)
        {
          ClientDown(token, socket.SocketError.ToString());
        }
        else
        {
          //发送成功 回调
          token.SendMagSuccessedCallBack();
        }
      }
    }
#endregion


  }
}
