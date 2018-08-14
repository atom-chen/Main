using Public.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tab_Server
{
    public int id;
    public string name;
    public string ipAddress;
    public int port;
    public string appName;
    public int maxConnect;
    public bool Hot
    {
        get { return maxConnect >= 50; }
    }
}

public class TabServerManager
{
    private static List<Tab_Server> m_ServerList = new List<Tab_Server>();
    public static List<Tab_Server> ServerList { get { return m_ServerList; } }
    static TabServerManager()
    {
        ReLoad();
    }

    public static void ReLoad()
    {
        try
        {
            m_ServerList.Clear();
            ReadTableTools table = new ReadTableTools("Server.txt");
            //将每个元组构造成Tab_Item，添加到表格
            while (!table.IsTupleEnd())
            {
                string data;
                Tab_Server server = new Tab_Server();

                //ID
                data = table.GetData();
                server.id = Convert.ToInt32(data);

                //name
                data = table.GetData();
                server.name = data;

                //ip
                data = table.GetData();
                server.ipAddress = data;

                //port
                data = table.GetData();
                server.port = Convert.ToInt32(data);

                //maxConnect
                data = table.GetData();
                server.maxConnect = Convert.ToInt32(data);

                m_ServerList.Add(server);
                table.LineDown();
            }
        }
        catch (Exception)
        {

        }
    }

}