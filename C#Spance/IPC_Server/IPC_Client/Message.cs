using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class test : MarshalByRefObject
{
    private int iCount = 0;
    public int count()
    {
        iCount++;
        return iCount;
    }

    public int Add(int x)
    {
        iCount += x;
        return iCount;
    }
}
namespace MessageObject
{
    //MarshalByRefObject 允许在支持远程处理的应用程序中跨应用程序域边界访问对象。
    public class RemoteObject : MarshalByRefObject
    {
        public static Queue<string> qMessage { get; set; } //使用消息队列储存消息

        public void SendMessage(string message)
        {
            if (qMessage == null)
            {
                qMessage = new Queue<string>();
            }
            qMessage.Enqueue(message);
        }
    }
}