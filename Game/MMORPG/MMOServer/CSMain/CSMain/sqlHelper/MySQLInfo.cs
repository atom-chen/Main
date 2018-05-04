using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMain
{
  class MySQLInfo
  {
    private const string m_IPAddress = "127.0.0.1";
    private const int m_Port = 3306;
    private const string m_UserName = "root";
    private const string m_PassWord = "root";
    private const string m_Database = "taidou";

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
