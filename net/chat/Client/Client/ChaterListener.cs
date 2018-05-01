using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Client.Photon;

namespace Client
{
  class ChatListener : IPhotonPeerListener
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
      Console.WriteLine("收到包"+operationResponse.OperationCode);
      foreach(KeyValuePair<byte,object> item in operationResponse.Parameters)
      {
        Console.WriteLine(item.Value);
      }

    }

    //当连接状态改变时
    public void OnStatusChanged(StatusCode statusCode)
    {
      switch (statusCode)
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
}
