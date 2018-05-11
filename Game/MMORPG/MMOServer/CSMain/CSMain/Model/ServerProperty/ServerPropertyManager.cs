﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
  class ServerPropertyManager
  {
    private static ServerPropertyManager _Instance = new ServerPropertyManager();

    private ServerPropertyManager()
    {

    }
    public static ServerPropertyManager Instance
    {
      get
      {
        return _Instance;
      }
    }

    //从数据库获取全部区服的信息
    public IList<_DBServerPropert> GetAllServer()
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          CSMain.Server.log.Debug("session="+session.ToString());
          using (var transction = session.BeginTransaction())
          {
            CSMain.Server.log.Debug("transction=" + transction.ToString());
            var servers = session.QueryOver<_DBServerPropert>();
            CSMain.Server.log.Debug("servers=" + servers.ToString());
            transction.Commit();
            CSMain.Server.log.Debug("transction=" + transction.ToString());
            return servers.List();
          }
        }
      }
      catch (Exception ex)
      {
        CSMain.Server.log.Debug("获取服务器列表发生异常      ：    "+ex.Message);
      }
      return null;
    }
  }
}


