using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//DB写 Main读
public partial class  DB2Main : MarshalByRefObject
{
    public static Queue<DM_MsgBase> qMessage { get; set; } //使用消息队列储存消息
}

