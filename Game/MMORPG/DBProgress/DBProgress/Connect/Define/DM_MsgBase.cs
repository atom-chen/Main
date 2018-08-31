using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class DM_MsgBase 
{
    public abstract DM_MsgCode code { get; }

    public RETURN_CODE returnCode;
    public int InfoId;

    protected Dictionary<ParaCode, object> m_ParaDic = new Dictionary<ParaCode, object>();

    
    protected T GetPara<T>(ParaCode code)
    {
        object ret;
        m_ParaDic.TryGetValue(code, out ret);
        return (T)ret;
    }
}

