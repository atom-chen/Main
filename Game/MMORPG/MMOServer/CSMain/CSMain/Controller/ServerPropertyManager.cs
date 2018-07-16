using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 逻辑层服务器列表操作
 */

class ServerPropertyManager
{
    private static List<ServerPropert> m_AllServer;
    public static List<ServerPropert> GetAllServerPropert()
    {
        if (m_AllServer == null)
        {
            //获取全部服务器列表
            m_AllServer = new List<ServerPropert>();
            IList<_DBServerPropert> allDBServer = ServerPropertyController.GetAllServer();
            foreach (var item in allDBServer)
            {
                ServerPropert sp = new ServerPropert(item);
                m_AllServer.Add(sp);
            }
        }
        return m_AllServer;
    }

}

