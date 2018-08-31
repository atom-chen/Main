using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Reflection;
using System.Threading;

namespace CSMain
{
    public partial class Server
    {
        public static Dictionary<byte, CG_FactoryBase> handlers = new Dictionary<byte, CG_FactoryBase>();             //byte表示OperationCode
        private static List<RoutinueBase> routinues = new List<RoutinueBase>();
        public static Main2DB _IPC2DB;
        public static DB2Main _IPC2Main;
        private void InitServer()
        {
            RegisteHandlers();
            RegisteRoutinue();
            ConnectToIPC();
            Thread tMain = new Thread(MainLoop);
            Thread tMain1 = new Thread(MainLoop);
            Thread tMain2 = new Thread(MainLoop);
            Thread tMain3 = new Thread(MainLoop);
            Thread tMain4 = new Thread(MainLoop);
        }
        void RegisteHandlers()
        {
            //用反射机制注册所有Factory
            Type[] types = Assembly.GetAssembly(typeof(CG_FactoryBase)).GetTypes();
            foreach (var type in types)
            {
                if (type.FullName.EndsWith("FACTORY"))
                {
                    Activator.CreateInstance(type);
                }
            }
        }

        void RegisteRoutinue()
        {
            //用反射机制注册所有Factory
            Type[] types = Assembly.GetAssembly(typeof(RoutinueBase)).GetTypes();
            foreach (var type in types)
            {
                if (type.FullName.EndsWith("Routinue"))
                {
                    RoutinueBase obj = Activator.CreateInstance(type) as RoutinueBase;
                    if (obj != null)
                    {
                        routinues.Add(obj);
                    }

                }
            }
        }

        //建立IPC连接
        public void ConnectToIPC()
        {
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, false);
            _IPC2DB = (Main2DB)Activator.GetObject(typeof(Main2DB), "ipc://MainDB/Main2DB");
            _IPC2Main = (DB2Main)Activator.GetObject(typeof(DB2Main), "ipc://MainDB/DB2Main");
        }

        public void MainLoop()
        {
            while (true)
            {
                for (int i = 0; i < routinues.Count; i++)
                {
                    routinues[i].Tick();
                }
            }
        }

    }
}