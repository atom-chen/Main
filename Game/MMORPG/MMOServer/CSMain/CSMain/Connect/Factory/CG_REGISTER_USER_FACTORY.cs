using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CG_REGISTER_USER_FACTORY : CG_FactoryBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.Register; }
    }
    private  Queue<CG_REGISTER_PAK> m_Queue = new Queue<CG_REGISTER_PAK>();
    CG_REGISTER_USER_FACTORY()
    {
        for (int i = 0; i < 100; i++)
        {
            CG_REGISTER_PAK pak = new CG_REGISTER_PAK();
            m_Queue.Enqueue(pak);
        }
    }
    public override CG_PAK_BASE GetPak()
    {
        return m_Queue.Dequeue();
    }

    public override void GCPak(CG_PAK_BASE pak)
    {
        CG_REGISTER_PAK temp = (CG_REGISTER_PAK)pak;
        if(temp!=null)
        {
            m_Queue.Enqueue(temp);
        }
    }
}
