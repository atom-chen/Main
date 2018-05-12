using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 逻辑层服务器列表操作
 */ 

class ServerPropertyController
{
  private ServerPropertyController()
  {

  }
  public static ServerPropertyController _Instance=new ServerPropertyController();
  public static ServerPropertyController Instance
  {
    get
    {
      return _Instance;
    }
  }

  private List<ServerPropert> m_AllServer;
  public List<ServerPropert> GetAllServerPropert()
  {
    if(m_AllServer==null)
    {
      //获取全部服务器列表
      m_AllServer = new List<ServerPropert>();
      IList<_DBServerPropert> allDBServer = ServerPropertyManager.Instance.GetAllServer();
      foreach (var item in allDBServer)
      {
        ServerPropert sp = new ServerPropert(item);
        m_AllServer.Add(sp);
      }
    }
    return m_AllServer;
  }
}

