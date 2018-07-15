using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 装备背包
/// </summary>
public partial class EquipBag
{
    private List<Equip> m_EquipBag;

    public EquipBag()
    {
        m_EquipBag = new List<Equip>();
    }

    public EquipBag(List<Equip> equipBag)
    {
        m_EquipBag = equipBag;
    }

    public Equip GetEquipById(int guid)
    {
        foreach(Equip equip in m_EquipBag)
        {
            if(equip.guid==guid)
            {
                return equip;
            }
        }
        return null;
    }

    public List<Equip> GetEquipByTabID(int equipTabId)
    {
        List<Equip> ret = new List<Equip>();
        foreach(Equip equip in m_EquipBag)
        {
            if(equip.equipID==equipTabId)
            {
                ret.Add(equip);
            }
        }
        return ret;
    }

    /// <summary>
    /// 根据位置取出装备
    /// </summary>
    /// <param name="type">位置</param>
    /// <returns>装备</returns>
    public List<Equip> GetEquip(EQUIP_TYPE type)
    {
        List<Equip> ret = new List<Equip>();
        foreach (Equip equip in m_EquipBag)
        {
            //根据equipID从 equip表取出所属位置
            Tab_Equip tab=TabEquipManager.GetEquipByID(equip.equipID);
            if (tab != null && tab.type == (int)type)
            {
                ret.Add(equip);
            }
        }
        return ret;
    }
}

