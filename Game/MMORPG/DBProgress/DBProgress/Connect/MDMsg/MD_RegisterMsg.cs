using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MD_RegisterMsg : MD_MsgBase
{
    public override MD_MsgCode code
    {
        get { return MD_MsgCode.REGISTER; }
    }
    public string UserName
    {
        get { return GetPara<string>(ParaCode.USER_NAME); }
        set { m_ParaDic.Remove(ParaCode.USER_NAME); m_ParaDic.Add(ParaCode.USER_NAME, value);}
    }

    public string Password
    {
        get { return GetPara<string>(ParaCode.USER_PWD); }
        set { m_ParaDic.Remove(ParaCode.USER_PWD); m_ParaDic.Add(ParaCode.USER_PWD, value); }
    }
    public string IPAddrea
    {
        get
        {
            return GetPara<string>(ParaCode.IPADDREA);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.IPADDREA); m_ParaDic.Add(ParaCode.IPADDREA, value);
        }
    }
}

