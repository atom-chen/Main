using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//获取玩家数据
public class DM_UserConnectMsg : DM_MsgBase
{
    public override DM_MsgCode code
    {
        get { return DM_MsgCode.USER_CONNECT; }
    }
    public _DBUser User
    {
        get
        {
            return GetPara<_DBUser>(ParaCode.USER);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.USER); m_ParaDic.Add(ParaCode.USER, value);
        }
    }

    public IList<_DBRole> RoleList
    {
        get
        {
            return GetPara<IList<_DBRole>>(ParaCode.ROLELIST);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.ROLELIST); m_ParaDic.Add(ParaCode.ROLELIST, value);
        }
    }

    public IList<_DBItem> ItemList
    {
        get
        {
            return GetPara<IList<_DBItem>>(ParaCode.ITEMLIST);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.ITEMLIST); m_ParaDic.Add(ParaCode.ITEMLIST, value);
        }
    }

    public IList<_DBEquip> EquipList
    {
        get
        {
            return GetPara<IList<_DBEquip>>(ParaCode.EQUIPLIST);
        }
        set
        {
            m_ParaDic.Remove(ParaCode.EQUIPLIST); m_ParaDic.Add(ParaCode.EQUIPLIST, value);
        }
    }

    public string RequesterIP
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

