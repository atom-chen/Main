using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Main写 DB读
public partial class Main2DB : MarshalByRefObject
{
    public static Queue<MD_MsgBase> qMessage { get; set; } 
}

