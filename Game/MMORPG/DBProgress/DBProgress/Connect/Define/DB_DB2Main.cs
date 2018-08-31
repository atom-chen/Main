using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


partial class DB2Main : MarshalByRefObject
{
    public static void SendMessage(DM_MsgBase message)
    {
        if (qMessage == null)
        {
            qMessage = new Queue<DM_MsgBase>();
        }
        qMessage.Enqueue(message);
    }
}
