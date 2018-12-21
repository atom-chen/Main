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
        public static Dictionary<byte, CG_FactoryBase> mFactaryDic = new Dictionary<byte, CG_FactoryBase>();             //byte表示OperationCode
        private static List<RoutinueBase> routinues = new List<RoutinueBase>();
        public static Main2DB _IPC2DB;
        public static DB2Main _IPC2Main;

        public void Init()
        {
            RegisteHandlers();
            RegisteRoutinue();
            ConnectToIPC();
            for(int i = 0;i<100;i++)
            {
                Thread tMain = new Thread(MainLoop);
            }
        }
        void RegisteHandlers()
        {
            //用反射机制注册所有Factory
            Type[] types = Assembly.GetAssembly(typeof(CG_FactoryBase)).GetTypes();
            foreach (var type in types)
            {
                if (type.FullName.EndsWith("FACTORY"))
                {
                    CG_FactoryBase obj = Activator.CreateInstance(type) as CG_FactoryBase;
                    if(obj!=null)
                    {
                        mFactaryDic.Add((byte)obj.OpCode, obj);
                    }
                }
            }
            LogManager.Log("Factory Size ={0}", mFactaryDic.Count.ToString());
        }

        void RegisteRoutinue()
        {
            //用反射机制注册所有Routinue
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
        void ConnectToIPC()
        {
            IpcClientChannel channel = new IpcClientChannel();
            ChannelServices.RegisterChannel(channel, false);
            _IPC2DB = (Main2DB)Activator.GetObject(typeof(Main2DB), "ipc://MainDB/Main2DB");
            _IPC2Main = (DB2Main)Activator.GetObject(typeof(DB2Main), "ipc://MainDB/DB2Main");
            if(_IPC2DB!=null && _IPC2Main!=null)
            {
                LogManager.Debug("IPC建立成功!");
            }
            else
            {
                LogManager.Debug("IPC建立失败!");
            }
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