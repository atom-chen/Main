using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMain
{
  class MySQLInfo
  {
    private static string m_IPAddress = "127.0.0.1";
    private static int m_Port = 3306;
    private static string m_UserName = "root";
    private static string m_PassWord = "root";
    private static string m_Database = "taidou";

    public static string IPAddress
    {
      get
      {
        return m_IPAddress;
      }
    }
    public static int port
    {
      get
      {
        return m_Port;
      }
    }
    public static string UserName
    {
      get
      {
        return m_UserName;
      }
    }
    public static string Password
    {
      get
      {
        return m_PassWord;
      }
    }
    public static string DataBase
    {
      get
      {
        return m_Database;
      }
    }

  }
}
