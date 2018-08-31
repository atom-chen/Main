using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class DM_RoleMsg : DM_MsgBase
{

    public override DM_MsgCode code
    {
        get { return DM_MsgCode.ROLE; }
    }

    public DB._DBRole Role
    {
        get
        {
            return GetPara<DB._DBRole>(ParaCode.ROLE);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.ROLE); m_ParaDic.Add(ParaCode.ROLE, value);
        }
    }
}

