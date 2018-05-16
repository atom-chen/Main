using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetFrame.auto;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //服务器初始化
            ServerStart ss = new ServerStart(9000);
            ss.encode = MessageEncoding.encode;
            ss.center = new HandlerCenter();
            ss.decode = MessageEncoding.decode;
            ss.LD = LengthEncoding.decode;
            ss.LE = LengthEncoding.encode;
            ss.Start(9999);
            Console.WriteLine("服务器启动成功");
            while (true) { }
        }
    }
}
