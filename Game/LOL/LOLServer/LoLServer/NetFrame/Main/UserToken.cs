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

    private List<byte> m_Cache = new List<byte>();//接收消息缓存
    private Queue<byte[]> m_WriteList = new Queue<byte[]>();//发送消息队列

    public LengthDecode m_LDecode;
    public LengthEncode m_LEncode;
    public encode m_Encode;
    public decode m_Decode;

    public delegate void SendProcess(SocketAsyncEventArgs e);
    public SendProcess m_SendProcess;

    private bool m_IsRending = false;
    private bool m_IsWriting = false;

    public UserToken()
    {
      m_SendSAEA = new SocketAsyncEventArgs();
      m_ReceiveSAEA = new SocketAsyncEventArgs();
      m_ReceiveSAEA.UserToken = this;
      m_SendSAEA.UserToken = this;
    }
    #region 发送消息处理
    //消息发送结束的回调
    public void SendMagSuccessedCallBack()
    {
      OnWrite();//尾递归
    }

    public void Write(byte[] value)
    {
      if (m_Connect == null)
      {
        //此连接已断开
        return;
      }
      //放到发送队列
      m_WriteList.Enqueue(value);
      if (!m_IsWriting)
      {
        //开始写
        m_IsWriting = true;
        OnWrite();
      }
    }
    //写消息的处理逻辑
    public void OnWrite()
    {
      if(m_WriteList.Count==0)
      {
        m_IsWriting = false;
        return;
      }
      byte[] buff = m_WriteList.Dequeue();
      m_SendSAEA.SetBuffer(buff, 0, buff.Length);
      bool result = m_Connect.SendAsync(m_SendSAEA);
      if(!result)
      {
        //如果立即发送
        m_SendProcess(m_SendSAEA);
      }
    }


    #endregion


    #region 接收消息处理

    //消息处理函数
    public void Receive(byte[] buffer)
    {
      //将消息插入到cache
      m_Cache.AddRange(buffer);
      if (!m_IsRending)
      {
        m_IsRending = true;
        OnData();
      }
    }

    //接收消息的处理函数
    public void OnData()
    {
      byte[] buff = null;
      //当解码器存在，进行粘包处理
      if (m_LDecode != null)
      {
        buff = m_LDecode(ref m_Cache);
        //如果数据还没有传完，则退出此次数据处理，等待下次消息到达
        if (buff == null)
        {
          m_IsRending = false;
          return;
        }
        else
        {

        }

      }
      else
      {
        //缓存区中没有数据，直接跳出消息处理  等待下次消息到达
        if (m_Cache.Count == 0)
        {
          m_IsRending = false;
          return;
        }
      }
      if (m_Decode == null)
      {
        throw new Exception("消息解码器为空");
      }

      object message = m_Decode(buff);//进行消息反序列化

      //TODO 通知应用层有消息到达

      //尾递归，防止在消息处理过程中有其他消息到达而没有经过处理
      OnData();
    }


    #endregion









    public void Close()
    {
      try
      {
        m_WriteList.Clear();
        m_Cache.Clear();
        m_IsRending = false;
        m_IsWriting = false;
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
