using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CG_ADD_ROLE_FACTORY
{
    private static Queue<CG_ADD_ROLE_PAK> m_Queue = new Queue<CG_ADD_ROLE_PAK>();
    static CG_ADD_ROLE_FACTORY()
    {
        for(int i=0;i<100;i++)
        {
            CG_ADD_ROLE_PAK pak = new CG_ADD_ROLE_PAK();
            m_Queue.Enqueue(pak);
        }
    }
    public static CG_ADD_ROLE_PAK GetPak()
    {
        return m_Queue.Dequeue();
    }

    public static void GCPak(CG_ADD_ROLE_PAK pak)
    {
        m_Queue.Enqueue(pak);
    }
}

