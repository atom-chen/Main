using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;







  class NHibernateHelper
  {
    private static ISessionFactory m_SessionFactory;
    private static void InitFactory()
    {
      try
      {
        m_SessionFactory = Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(db => db.Server(MySQLInfo.IPAddress).Database(MySQLInfo.DataBase).Username(MySQLInfo.
          UserName).Password(MySQLInfo.Password))).Mappings(x => x.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
      }
      catch(FluentConfigurationException ex)
      {
        CSMain.Server.log.Debug(ex.Message);
      }

    }

    private static ISessionFactory SessionFactory
    {
      get
      {
        if (m_SessionFactory == null)
          InitFactory();
        return m_SessionFactory;
      }
    }

    public static ISession OpenSession()
    {
      return SessionFactory.OpenSession();
    }
  }

