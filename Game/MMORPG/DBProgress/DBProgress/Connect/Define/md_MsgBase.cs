using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class MD_MsgBase 
{
    public abstract MD_MsgCode code { get; }


    protected Dictionary<ParaCode, object> m_ParaDic = new Dictionary<ParaCode, object>();

    
    protected T GetPara<T>(ParaCode code)
    {
        object ret;
        m_ParaDic.TryGetValue(code, out ret);
        return (T)ret;
    }
}

