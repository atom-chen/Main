using MessageObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;

namespace IPC_Client
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    IpcChannel tcc = new IpcChannel();
        //    ChannelServices.RegisterChannel(tcc, false);
        //    WellKnownClientTypeEntry remotEntry = new WellKnownClientTypeEntry(typeof(test), "ipc://testchannel/test");
        //    RemotingConfiguration.RegisterWellKnownClientType(remotEntry);

        //    test st = new test();
        //    Console.WriteLine("{0},{1}", st.count(), st.Add(1));
        //    Console.ReadLine();
        //}
        static void Main(string[] args)
        {
            RemoteObject objRemoteObject = ConnectServer();
            Send(objRemoteObject);
        }
        private static void Send(RemoteObject objRemoteObject)
        {
            while (true)
            {
                Console.WriteLine("please input message...");
                string message = Console.ReadLine();
                try
                {
                    objRemoteObject.SendMessage(message);
                }
                catch (System.Runtime.Remoting.RemotingException)
                {
                    Console.WriteLine("can not connect message server");
                }
            }
        }
        private static RemoteObject ConnectServer()
        {
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, false);
            RemoteObject objRemoteObject = (RemoteObject)Activator.GetObject(typeof(RemoteObject), "ipc://ServerChannel/RemoteObject");
            return objRemoteObject;
        }
    }
}
