using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Main2DB
{
    public void SendMessage(MD_MsgBase message)
    {
        if (qMessage == null)
        {
            qMessage = new Queue<MD_MsgBase>();
        }
        qMessage.Enqueue(message);
    }
}
