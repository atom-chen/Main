using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MD_RoleAddMsg:MD_MsgBase
{
    public override MD_MsgCode code
    {
        get { return MD_MsgCode.ROLE_ADD; }
    }

    public string RoleName
    {
        get { return GetPara<string>(ParaCode.ROLE_NAME); }
        set { m_ParaDic.Remove(ParaCode.ROLE_NAME); m_ParaDic.Add(ParaCode.ROLE_NAME, value); }
    }

    public bool RoleSex
    {
        get { return GetPara<bool>(ParaCode.ROLE_SEX); }
        set { m_ParaDic.Remove(ParaCode.ROLE_SEX); m_ParaDic.Add(ParaCode.ROLE_SEX, value); }
    }

    public int OwnerId
    {
        get { return GetPara<int>(ParaCode.ROLE_OWNER); }
        set { m_ParaDic.Remove(ParaCode.ROLE_OWNER); m_ParaDic.Add(ParaCode.ROLE_OWNER, value); }
    }
}

