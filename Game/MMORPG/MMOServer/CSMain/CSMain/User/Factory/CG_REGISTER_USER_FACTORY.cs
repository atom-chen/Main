using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CG_REGISTER_USER_FACTORY
{
    private static Queue<CG_REGISTER_PAK> m_Queue = new Queue<CG_REGISTER_PAK>();
    static CG_REGISTER_USER_FACTORY()
    {
        for (int i = 0; i < 100; i++)
        {
            CG_REGISTER_PAK pak = new CG_REGISTER_PAK();
            m_Queue.Enqueue(pak);
        }
    }
    public static CG_REGISTER_PAK GetPak()
    {
        return m_Queue.Dequeue();
    }

    public static void GCPak(CG_REGISTER_PAK pak)
    {
        m_Queue.Enqueue(pak);
    }
}
