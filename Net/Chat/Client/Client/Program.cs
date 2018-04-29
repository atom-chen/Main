using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
  class ChatListener:IPhotonPeerListener
  {
    public bool m_IsConnect = false;
    //debug的回调
    public void DebugReturn(DebugLevel level, string message)
    {

    }
    //事件处理函数
    public void OnEvent(EventData eventData)
    {

    }
    //服务器响应
    public void OnOperationResponse(OperationResponse operationResponse)
    {

    }
    //当连接状态改变时
    public void OnStatusChanged(StatusCode statusCode)
    {
      switch(statusCode)
      {
        case StatusCode.Connect:
          Console.WriteLine("连接成功");
          m_IsConnect = true;
          break;
        default:
          Console.WriteLine("其他状态");
          break;
      }
    }
  }
  class Program
  {
    static void Main(string[] args)
    {
      ChatListener listener=new ChatListener();
      PhotonPeer peer = new PhotonPeer(listener,ConnectionProtocol.Tcp);
      peer.Connect("192.168.2.102:4530", "ChatServer");//添加请求到服务器
      Console.WriteLine("正在连接...");
      while(!listener.m_IsConnect)
      {
        peer.Service();//立即发包
      }
    }
  }
}
