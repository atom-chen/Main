using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class NewWorkScripts{
    private static NewWorkScripts instance;
    private static Socket socket;
    private static string ip = "127.0.0.1";
    private static  int port = 12344;
    public static NewWorkScripts getInstance()
    {
        if(instance==null)
        {
            instance = new NewWorkScripts();
            init();
        }

        return instance;
        
    }
    public static void init()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            Debug.Log("服务器连接成功");
        }
        catch(Exception e)
        {
            Debug.Log("服务器连接失败 "+ e.Message);
        }

    }
}
