using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace NetFrame
{
    public class ServerStart
    {
        Socket server;//服务器socket监听对象
        int maxClient;//最大客户端连接数
        //Semaphore：限制可同时访问某一资源或资源池的线程数。
        Semaphore acceptClients; 
        UserTokenPool pool;

        //初始化通信监听     输入：max 最大访问量
        public ServerStart(int max)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//新实例初始化 Socket 类使用指定的地址族、 套接字类型和协议。
            maxClient = max;
            pool = new UserTokenPool(max);//初始化一个最大max个连接数的连接池
            acceptClients = new Semaphore(max, max);//初始化 Semaphore 类的新实例，并指定初始入口数和最大并发入口数。
            //新建max个token插入到pool，以供我们使用
            for(int i=0;i<max;i++)
            {
                UserToken token = new UserToken();
                //初始化token信息
                pool.push(token);
            }

        }
        //port 监听的端口号
        public void start(int port)
        {
            //socket监听当前服务器网卡所有可用IP地址的port端口
            //外网IP，内网IP192.168.x.x 本机IP127.0.0.1
            server.Bind(new IPEndPoint(IPAddress.Any,port));
            //置于监听状态
            server.Listen(10);//10：挂起的连接队列的最大长度->队列阻塞之后，最多允许连接10个
            //开始连接
            StartAccept(null);

        }
        //开始客户端连接
        //SocketAsyncEventArgs类表示异步套接字操作。
        public void StartAccept(SocketAsyncEventArgs e)
        {
            //如果当前传入为空，说明调用新的客户端连接监听事件，否则的话 移除当前客户端
            if(e==null)
            {
                //新建
                e = new SocketAsyncEventArgs();
                //EventHandler类表示将用于处理不具有事件数据的事件的方法。
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted);//Completed	用于完成异步操作的事件。
            }
            else
            {
                e = null;
            }
            //信号量值空出来一个
            acceptClients.WaitOne();
            //开启监听
            bool result=server.AcceptAsync(e);
            //判断异步事件是否挂起，没挂起说明立刻执行完成，直接处理事件。否则会在处理完成后触发Accept_Comleted事件
            if(!result)
            {
                //如果马上完成，则直接处理连接事件
                ProcessAccept(e);
            }
        }
        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            UserToken token = pool.pop();
            token.conn = e.AcceptSocket;
            //开启消息到达监听
            //释放当前异步对象
            StartAccept(e);
        }
        public void Accept_Comleted(object sender,SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
    }
}
