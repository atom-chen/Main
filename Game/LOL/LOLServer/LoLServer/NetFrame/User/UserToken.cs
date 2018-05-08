using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


//一个玩家的连接
namespace NetFrame
{
  //用户连接信息类

  public class UserToken
  {
    public Socket m_Connect;//与该Token关联的连接

    public SocketAsyncEventArgs m_ReceiveSAEA;//用户异步接收

    public SocketAsyncEventArgs m_SendSAEA;//用户异步发送

    public UserToken()
    {
      m_SendSAEA = new SocketAsyncEventArgs();
      m_ReceiveSAEA = new SocketAsyncEventArgs();
      m_ReceiveSAEA.UserToken = this;
      m_SendSAEA.UserToken = this;
    }

    //消息处理函数
    public void ReceiveMsg(byte[] buffer)
    {
      
    }
    //消息发送结束的回调
    public void SendMagSuccessedCallBack()
    {

    }

    public void Close()
    {
      try
      {
        m_Connect.Shutdown(SocketShutdown.Both);
        m_Connect.Close();
        m_Connect = null;
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }


  }
}
