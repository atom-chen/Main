using System;
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

    public IList<_DBServerPropert> GetAllServer()
    {
      try
      {
        using (var session = NHibernateHelper.OpenSession())
        {
          using (var transction = session.BeginTransaction())
          {
            var servers = session.QueryOver<_DBServerPropert>();
            transction.Commit();
            return servers.List();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return null;
    }
  }
}


