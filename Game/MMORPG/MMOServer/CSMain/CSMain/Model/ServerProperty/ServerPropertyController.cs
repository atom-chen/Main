using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class ServerPropertyController
    {
        //从数据库获取全部区服的信息
        public static IList<_DBServerPropert> GetAllServer()
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
                LogManager.Error("获取服务器列表发生异常      ：    " + ex.Message);
            }
            return null;
        }
    }
}


