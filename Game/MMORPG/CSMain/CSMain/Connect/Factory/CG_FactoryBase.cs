using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class CG_FactoryBase
{
    public abstract OperationCode OpCode { get; }
    public  abstract CG_PAK_BASE GetPak();

    public abstract void GCPak(CG_PAK_BASE pak);
}

