﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CG_START_GAME_FACTORY : CG_FactoryBase
{
    public override OperationCode OpCode
    {
        get { return OperationCode.StartGame; }
    }
    private  Queue<CG_START_GAME_PAK> m_Queue = new Queue<CG_START_GAME_PAK>();
    CG_START_GAME_FACTORY()
    {
        for(int i=0;i<100;i++)
        {
            CG_START_GAME_PAK pak = new CG_START_GAME_PAK();
            m_Queue.Enqueue(pak);
        }
    }
    public override CG_PAK_BASE GetPak()
    {
        return m_Queue.Dequeue();
    }

    public override void GCPak(CG_PAK_BASE pak)
    {
        CG_START_GAME_PAK temp = pak as CG_START_GAME_PAK;
        if (temp!=null)
        {
            m_Queue.Enqueue(temp);
        }
    }
}

