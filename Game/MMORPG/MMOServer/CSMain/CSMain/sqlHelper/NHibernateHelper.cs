using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;


namespace CSMain
{

  class SQLHelper
  {
    private SQLHelper()
    {
      try
      {
        conn = new SqlConnection(string.Format("server={0};port={1};user={2};password={3}; database={4};", MySQLInfo.IPAddress, MySQLInfo.port, MySQLInfo.UserName, MySQLInfo.Password, MySQLInfo.DataBase));
        conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
        Console.WriteLine("已经建立连接");
        //在这里使用代码对数据库进行增删查改
      }
      catch (MySqlException ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        conn.Close();
      }
    }
    private SqlConnection conn;


    private static SQLHelper _Instance = new SQLHelper();
    public static SQLHelper Instance
    {
      get
      {
        return _Instance;
      }
    }

  }



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
        Console.WriteLine(ex.Message);
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
}
