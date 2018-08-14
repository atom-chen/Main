using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CG_START_GAME_FACTORY
{
    private static Queue<CG_START_GAME_PAK> m_Queue = new Queue<CG_START_GAME_PAK>();
    static CG_START_GAME_FACTORY()
    {
        for(int i=0;i<100;i++)
        {
            CG_START_GAME_PAK pak = new CG_START_GAME_PAK();
            m_Queue.Enqueue(pak);
        }
    }
    public static CG_START_GAME_PAK GetPak()
    {
        return m_Queue.Dequeue();
    }

    public static void GCPak(CG_START_GAME_PAK pak)
    {
        m_Queue.Enqueue(pak);
    }
}

