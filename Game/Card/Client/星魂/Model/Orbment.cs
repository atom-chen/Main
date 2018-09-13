/********************************************************************************
 *	文件名：	Orbment.cs
 *	全路径：	\Script\Orbment\Orbment.cs
 *	创建人：	王喆
 *	创建时间：2017-06-12
 *
 *	功能说明：星宿命理数据基础信息
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.Table;
using ProtobufPacket;
using Games;

public class OrbmentSlot
{
    public OrbmentSlot()
    {
        m_Quartz.CleanUp();
    }

    private Quartz m_Quartz = new Quartz();
    public Quartz Quartz
    {
        get { return m_Quartz; }
    }
}

public class Orbment
{
    public Orbment()
    {
        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            m_QuartzSlot[i] = new Quartz();
        }
    }

    public void CleanUp()
    {
        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            m_QuartzSlot[i].CleanUp();
        }
    }

    public void CopyFrom(Orbment orbment)
    {
        if (orbment == null)
        {
            return;
        }

        for (int i = 0; i < m_QuartzSlot.Length && i < orbment.m_QuartzSlot.Length; i++)
        {
            m_QuartzSlot[i].CopyFrom(orbment.m_QuartzSlot[i]);
        }
    }

    public void BuildFromProtoOrbment(_ORBMENT orbment)
    {
        if (orbment == null)
        {
            return;
        }

        for (int i = 0; i < m_QuartzSlot.Length && i < orbment.QuartzSlot.Count; i++)
        {
            m_QuartzSlot[i].BuildFromProtoQuartz(orbment.QuartzSlot[i]);
        }
    }

    public void BuildProtoOrbment(_ORBMENT orbment)
    {
        if (orbment == null)
        {
            return;
        }

        orbment.QuartzSlot.Clear();
        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            _QUARTZ quartz = new _QUARTZ();
            m_QuartzSlot[i].BuildProtoQuartz(quartz);
            orbment.QuartzSlot.Add(quartz);
        }
    }

    public Quartz GetQuartz(ulong quartzGuid)
    {
        if (quartzGuid == GlobeVar.INVALID_GUID)
        {
            return null;
        }

        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            if (m_QuartzSlot[i].Guid == quartzGuid)
            {
                return m_QuartzSlot[i];
            }
        }

        return null;
    }

    public Quartz GetQuartzByIndex(int nSlotIndex)
    {
        if (nSlotIndex < 0 || nSlotIndex >= m_QuartzSlot.Length)
        {
            return null;
        }

        return m_QuartzSlot[nSlotIndex];
    }

    public bool DeleteQuartz(ulong quartzGuid)
    {
        if (quartzGuid == GlobeVar.INVALID_GUID)
        {
            return false;
        }

        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            if (m_QuartzSlot[i].Guid == quartzGuid)
            {
                m_QuartzSlot[i].CleanUp();
                return true;
            }
        }
        return false;
    }

    public bool UpdateQuartzByGuid(_QUARTZ quartz)
    {
        if (quartz == null)
        {
            return false;
        }

        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            if (m_QuartzSlot[i].Guid == quartz.Guid)
            {
                m_QuartzSlot[i].BuildFromProtoQuartz(quartz);
                return true;
            }
        }
        return false;
    }

    public bool UpdateQuartzByIndex(_QUARTZ quartz, int nSlotIndex)
    {
        if (quartz == null)
        {
            return false;
        }

        if (nSlotIndex < 0 || nSlotIndex >= m_QuartzSlot.Length)
        {
            return false;
        }

        m_QuartzSlot[nSlotIndex].BuildFromProtoQuartz(quartz);

        return true;
    }

    public bool GetAttrRefixValue(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        int nPercent = 0;
        int nAdd = 0;
        int nFinal = 0;

        if (false == GetAttrRefixValue_Single(type, out nPercent, out nAdd, out nFinal))
        {
            return false;
        }

        rPercent += nPercent;
        rAdd += nAdd;
        rFinal += nFinal;

        if (false == GetAttrRefixValue_Set(type, out nPercent, out nAdd, out nFinal))
        {
            return false;
        }

        rPercent += nPercent;
        rAdd += nAdd;
        rFinal += nFinal;

        return true;
    }

    public bool GetAttrRefixValueWithoutOrbal(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        int nPercent = 0;
        int nAdd = 0;
        int nFinal = 0;

        if (false == GetAttrRefixValue_Single(type, out nPercent, out nAdd, out nFinal))
        {
            return false;
        }

        rPercent += nPercent;
        rAdd += nAdd;
        rFinal += nFinal;

        if (false == GetAttrRefixValue_Set(type, out nPercent, out nAdd, out nFinal))
        {
            return false;
        }

        rPercent += nPercent;
        rAdd += nAdd;
        rFinal += nFinal;

        return true;
    }

    public bool GetAttrRefixValue_Single(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        int nPercent = 0;
        int nAdd = 0;
        int nFinal = 0;

        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            if (false == m_QuartzSlot[i].GetAttrRefixValue(type, out nPercent, out nAdd, out nFinal))
            {
                continue;
            }

            rPercent += nPercent;
            rAdd += nAdd;
            rFinal += nFinal;
        }

        return true;
    }

    public bool GetAttrRefixValue_Set(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        List<int> setIdList = GetQuartzSetId();
        for (int i = 0; i < setIdList.Count; i++)
        {
            Tab_QuartzSet tSet = TableManager.GetQuartzSetByID(setIdList[i], 0);
            if (tSet == null)
            {
                continue;
            }

            for (int j = 0; j < tSet.getAttrRefixTypeCount() && j < tSet.getAttrRefixValueCount(); j++)
            {
                int nSetAttrRefixType = tSet.GetAttrRefixTypebyIndex(j);
                int nSetAttrRefixValue = tSet.GetAttrRefixValuebyIndex(j);

                if (type == AttrType.MaxHP)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.MaxHPPercent)
                    {
                        rPercent += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.MaxHPAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.MaxHPFinal)
                    {
                        rFinal += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.Attack)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.AttackPercent)
                    {
                        rPercent += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.AttackAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.AttackFinal)
                    {
                        rFinal += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.Defense)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.DefensePercent)
                    {
                        rPercent += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.DefenseAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.DefenseFinal)
                    {
                        rFinal += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.Speed)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.SpeedPercent)
                    {
                        rPercent += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.SpeedAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                    else if (nSetAttrRefixType == (int)AttrRefixType.SpeedFinal)
                    {
                        rFinal += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.CritChance)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.CritChanceAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.CritEffect)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.CritEffectAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.ImpactChance)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.ImpactChanceAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                }
                else if (type == AttrType.ImpactResist)
                {
                    if (nSetAttrRefixType == (int)AttrRefixType.ImpactResistAdd)
                    {
                        rAdd += nSetAttrRefixValue;
                    }
                }

            }
        }

        return true;
    }

    public List<int> GetQuartzSetId()
    {
        List<int> setIdList = new List<int>();
        List<int> classIdList = new List<int>();
        List<int> classCountList = new List<int>();

        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            int nClassId = m_QuartzSlot[i].GetClassId();
            if (classIdList.Contains(nClassId))
            {
                continue;
            }

            classIdList.Add(nClassId);
            classCountList.Add(GetQuartzClassIdCount(nClassId));
        }

        for (int i = 0; i < classIdList.Count && i < classCountList.Count; i++)
        {
            if (classCountList[i] <= 1)
            {
                continue;
            }

            foreach (var pair in TableManager.GetQuartzSet())
            {
                if (pair.Value == null || pair.Value.Count < 1)
                {
                    continue;
                }

                Tab_QuartzSet tSet = pair.Value[0];
                if (tSet == null)
                {
                    continue;
                }

                if (tSet.NeedCount <= 0)
                {
                    continue;
                }

                if (classIdList[i] != tSet.NeedClassId || classCountList[i] < tSet.NeedCount)
                {
                    continue;
                }

                for (int j = 0; j < classCountList[i] / tSet.NeedCount; j++)
                {
                    setIdList.Add(tSet.Id);
                }
            }
        }

        return setIdList;
    }

    public int GetQuartzClassIdCount(int nClassId)
    {
        int count = 0;
        for (int i = 0; i < m_QuartzSlot.Length; i++)
        {
            if (m_QuartzSlot[i] == null || false == m_QuartzSlot[i].IsValid())
            {
                continue;
            }

            if (m_QuartzSlot[i].GetClassId() == nClassId)
            {
                count += 1;
            }
        }

        return count;
    }

    private Quartz[] m_QuartzSlot = new Quartz[GlobeVar.ORBMENT_SLOT_SIZE];
    public Quartz[] QuartzSlot
    {
        get { return m_QuartzSlot; }
    }
}

public class OrbmentTool
{
    public static string GetAttrIcon(AttrType type)
    {
        switch (type)
        {
            case AttrType.MaxHP:
                return "Star_Shengming";
            case AttrType.Attack:
                return "Star_Gongji";
            case AttrType.Defense:
                return "Star_fangyu";
            case AttrType.Speed:
                return "Star_Sudu";
            case AttrType.CritChance:
                return "Star_Baoji";
            case AttrType.CritEffect:
                return "Star_Baoshang";
            case AttrType.ImpactChance:
                return "Star_Mingzhong";
            case AttrType.ImpactResist:
                return "Star_Baokang";
            default:
                return "";
        }
    }


}