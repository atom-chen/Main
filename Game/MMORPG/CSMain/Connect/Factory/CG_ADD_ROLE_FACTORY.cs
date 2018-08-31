using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CG_ADD_ROLE_FACTORY:CG_FactoryBase
{
    private Queue<CG_ADD_ROLE_PAK> m_Queue = new Queue<CG_ADD_ROLE_PAK>();
    public CG_ADD_ROLE_FACTORY()
    {
        for(int i=0;i<100;i++)
        {
            CG_ADD_ROLE_PAK pak = new CG_ADD_ROLE_PAK();
            m_Queue.Enqueue(pak);
        }
    }

    public  override CG_PAK_BASE GetPak()
    {
        return m_Queue.Dequeue();
    }

    public override OperationCode OpCode
    {
        get { return OperationCode.RoleAdd; }
    }

    public override void GCPak(CG_PAK_BASE pak)
    {
        CG_ADD_ROLE_PAK temp = pak as CG_ADD_ROLE_PAK;
        if (temp != null)
        {
            m_Queue.Enqueue(temp);
        }
    }
}

