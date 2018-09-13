/********************************************************************************
 *	文件名：	Quartz.cs
 *	全路径：	\Script\Quartz\Quartz.cs
 *	创建人：	王喆
 *	创建时间：2017-06-12
 *
 *	功能说明：星宿命理数据基础信息
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Item;
using Games.Table;
using ProtobufPacket;

public class QuartzAttr
{
    public QuartzAttr()
    {
        CleanUp();
    }

    public void CleanUp()
    {
        m_RefixType = GlobeVar.INVALID_ID;
        m_AttrValue = 0;
        m_Strengthen = 0;
    }

    public void CopyFrom(QuartzAttr attr)
    {
        if (attr == null)
        {
            return;
        }

        m_RefixType = attr.m_RefixType;
        m_AttrValue = attr.m_AttrValue;
        m_Strengthen = attr.m_Strengthen;
    }

    public void BuildFromProtoQuartzAttr(_QUARTZATTR attr)
    {
        if (attr == null)
        {
            return;
        }

        m_RefixType = attr.AttrRefixType;
        m_AttrValue = attr.AttrValue;
        m_Strengthen = attr.Strengthen;
    }

    public void BuildProtoQuartzAttr(_QUARTZATTR attr)
    {
        if (attr == null)
        {
            return;
        }

        attr.AttrRefixType = m_RefixType;
        attr.AttrValue = m_AttrValue;
        attr.Strengthen = m_Strengthen;
    }

    public bool GetAttrRefixValue(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        if (type == AttrType.MaxHP)
        {
            if (m_RefixType == (int)AttrRefixType.MaxHPPercent)
            {
                rPercent = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.MaxHPAdd)
            {
                rAdd = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.MaxHPFinal)
            {
                rFinal = m_AttrValue;
            }
        }
        else if (type == AttrType.Attack)
        {
            if (m_RefixType == (int)AttrRefixType.AttackPercent)
            {
                rPercent = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.AttackAdd)
            {
                rAdd = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.AttackFinal)
            {
                rFinal = m_AttrValue;
            }
        }
        else if (type == AttrType.Defense)
        {
            if (m_RefixType == (int)AttrRefixType.DefensePercent)
            {
                rPercent = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.DefenseAdd)
            {
                rAdd = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.DefenseFinal)
            {
                rFinal = m_AttrValue;
            }
        }
        else if (type == AttrType.Speed)
        {
            if (m_RefixType == (int)AttrRefixType.SpeedPercent)
            {
                rPercent = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.SpeedAdd)
            {
                rAdd = m_AttrValue;
            }
            else if (m_RefixType == (int)AttrRefixType.SpeedFinal)
            {
                rFinal = m_AttrValue;
            }
        }
        else if (type == AttrType.CritChance)
        {
            if (m_RefixType == (int)AttrRefixType.CritChanceAdd)
            {
                rAdd = m_AttrValue;
            }
        }
        else if (type == AttrType.CritEffect)
        {
            if (m_RefixType == (int)AttrRefixType.CritEffectAdd)
            {
                rAdd = m_AttrValue;
            }
        }
        else if (type == AttrType.ImpactChance)
        {
            if (m_RefixType == (int)AttrRefixType.ImpactChanceAdd)
            {
                rAdd = m_AttrValue;
            }
        }
        else if (type == AttrType.ImpactResist)
        {
            if (m_RefixType == (int)AttrRefixType.ImpactResistAdd)
            {
                rAdd = m_AttrValue;
            }
        }

        return true;
    }

    private int m_RefixType = GlobeVar.INVALID_ID;
    public int RefixType
    {
        get { return m_RefixType; }
    }

    private int m_AttrValue = 0;
    public int AttrValue
    {
        get { return m_AttrValue; }
    }

    private int m_Strengthen = 0;
    public int Strengthen
    {
        get { return m_Strengthen; }
    }
}

public class Quartz
{
    public Quartz()
    {
        m_Guid = GlobeVar.INVALID_GUID;
        m_QuartzId = GlobeVar.INVALID_ID;
        m_Star = 1;
        m_Strengthen = 0;
        m_MainAttr.CleanUp();
        for (int i = 0; i < m_AttachAttr.Length; i++)
        {
            m_AttachAttr[i] = new QuartzAttr();
        }
        m_CreateTime = GlobeVar.INVALID_ID;
    }

    public void CleanUp()
    {
        m_Guid = GlobeVar.INVALID_GUID;
        m_QuartzId = GlobeVar.INVALID_ID;
        m_Star = 1;
        m_Strengthen = 0;
        m_MainAttr.CleanUp();
        for (int i = 0; i < m_AttachAttr.Length; i++)
        {
            m_AttachAttr[i].CleanUp();
        }
        m_CreateTime = GlobeVar.INVALID_ID;
    }

    public void CopyFrom(Quartz quartz)
    {
        if (quartz == null)
        {
            return;
        }

        m_Guid = quartz.m_Guid;
        m_QuartzId = quartz.m_QuartzId;
        m_Star = quartz.m_Star;
        m_Strengthen = quartz.m_Strengthen;
        m_MainAttr.CopyFrom(quartz.m_MainAttr);
        for (int i = 0; i < m_AttachAttr.Length && i < quartz.m_AttachAttr.Length; i++)
        {
            m_AttachAttr[i].CopyFrom(quartz.m_AttachAttr[i]);
        }
        m_CreateTime = quartz.CreateTime;
    }

    public bool IsValid()
    {
        return m_Guid != GlobeVar.INVALID_GUID;
    }

    public void BuildFromProtoQuartz(_QUARTZ quartz)
    {
        if (quartz == null)
        {
            return;
        }

        m_Guid = quartz.Guid;
        m_QuartzId = quartz.QuartzId;
        m_Star = quartz.Star;
        m_Strengthen = quartz.Strengthen;
        m_MainAttr.BuildFromProtoQuartzAttr(quartz.MainAttr);
        for (int i = 0; i < m_AttachAttr.Length && i < quartz.AttachAttr.Count; i++)
        {
            m_AttachAttr[i].BuildFromProtoQuartzAttr(quartz.AttachAttr[i]);
        }
        m_CreateTime = quartz.CreateTime;
    }

    public void BuildProtoQuartz(_QUARTZ quartz)
    {
        if (quartz == null)
        {
            return;
        }

        quartz.Guid = m_Guid;
        quartz.QuartzId = m_QuartzId;
        quartz.Star = m_Star;
        quartz.Strengthen = m_Strengthen;

        quartz.MainAttr = new _QUARTZATTR();
        m_MainAttr.BuildProtoQuartzAttr(quartz.MainAttr);

        quartz.AttachAttr.Clear();
        for (int i = 0; i < m_AttachAttr.Length; i++)
        {
            _QUARTZATTR attr = new _QUARTZATTR();
            m_AttachAttr[i].BuildProtoQuartzAttr(attr);
            quartz.AttachAttr.Add(attr);
        }

        quartz.CreateTime = (int)m_CreateTime;
    }

    public Tab_Quartz GetTable_Quartz()
    {
        return TableManager.GetQuartzByID(m_QuartzId, 0);
    }

    public string GetIcon()
    {
        Tab_Quartz tQuartz = GetTable_Quartz();
        if (tQuartz == null)
        {
            return "";
        }

        return tQuartz.Icon;
    }

    public string GetListIcon()
    {
        Tab_Quartz tQuartz = GetTable_Quartz();
        if (tQuartz == null)
        {
            return "";
        }

        return tQuartz.ListIcon;
    }

    public int GetClassId()
    {
        Tab_Quartz tQuartz = GetTable_Quartz();
        if (tQuartz == null)
        {
            return GlobeVar.INVALID_ID;
        }

        return tQuartz.ClassId;
    }

    public int GetSlotType()
    {
        Tab_Quartz tQuartz = GetTable_Quartz();
        if (tQuartz == null)
        {
            return GlobeVar.INVALID_ID;
        }

        return tQuartz.SlotType;
    }

    public string GetName()
    {
        Tab_Quartz tQuartz = GetTable_Quartz();
        if (tQuartz == null)
        {
            return "";
        }

        return tQuartz.Name;
    }

    public int GetCoin()
    {
        int nCoin = 0;
        Tab_QuartzSell tSell = TableManager.GetQuartzSellByID(m_Star, 0);
        if (tSell == null)
        {
            return nCoin;
        }

        nCoin += tSell.BaseCoin;

        for (int i = 1; i <= m_Strengthen; i++)
        {
            Tab_QuartzStrengthen tStrengthen = TableManager.GetQuartzStrengthenByID(i, 0);
            if (tStrengthen == null)
            {
                continue;
            }

            nCoin += tStrengthen.GetNeedMoneybyIndex(m_Star - 1);
        }

        nCoin = (int)(nCoin * GlobeVar._GameConfig.m_QuartzSellReduceRate);

        return nCoin;
    }

    public int GetMainAttrStrengthenValue()
    {
        Tab_QuartzMainStrengthen tMainStrengthen = TableManager.GetQuartzMainStrengthenByID(m_Strengthen + 1, 0);
        if (tMainStrengthen == null)
        {
            return 0;
        }

        int nAttrId = tMainStrengthen.GetAttrRandombyIndex(m_Star - 1);
        Tab_QuartzMainRandom tRandom = TableManager.GetQuartzMainRandomByID(nAttrId, 0);
        if (tRandom == null)
        {
            return 0;
        }

        if (m_MainAttr.RefixType == (int)AttrRefixType.MaxHPAdd)
        {
            return tRandom.MaxHPAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.MaxHPPercent)
        {
            return tRandom.MaxHPPercentMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.MaxHPFinal)
        {
            return tRandom.MaxHPFinalMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.AttackAdd)
        {
            return tRandom.AttackAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.AttackPercent)
        {
            return tRandom.AttackPercentMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.AttackFinal)
        {
            return tRandom.AttackFinalMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.DefenseAdd)
        {
            return tRandom.DefenceAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.DefensePercent)
        {
            return tRandom.DefencePercentMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.DefenseFinal)
        {
            return tRandom.DefenceFinalMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.SpeedAdd)
        {
            return tRandom.SpeedAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.SpeedPercent)
        {
            return tRandom.SpeedPercentMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.SpeedFinal)
        {
            return tRandom.SpeedFinalMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.CritChanceAdd)
        {
            return tRandom.CritChanceAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.CritEffectAdd)
        {
            return tRandom.CritEffectAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.ImpactChanceAdd)
        {
            return tRandom.ImpactChanceAddMin;
        }
        else if (m_MainAttr.RefixType == (int)AttrRefixType.ImpactResistAdd)
        {
            return tRandom.ImpactResistAddMin;
        }

        return 0;
    }

    public bool GetAttrRefixValue(AttrType type, out int rPercent, out int rAdd, out int rFinal)
    {
        rPercent = 0;
        rAdd = 0;
        rFinal = 0;

        int nPercent = 0;
        int nAdd = 0;
        int nFinal = 0;

        if (false == m_MainAttr.GetAttrRefixValue(type, out nPercent, out nAdd, out nFinal))
        {
            return false;
        }

        rPercent += nPercent;
        rAdd += nAdd;
        rFinal += nFinal;

        for (int i = 0; i < m_AttachAttr.Length; i++)
        {
            if (false == m_AttachAttr[i].GetAttrRefixValue(type, out nPercent, out nAdd, out nFinal))
            {
                continue;
            }

            rPercent += nPercent;
            rAdd += nAdd;
            rFinal += nFinal;
        }

        return true;
    }

    private ulong m_Guid = GlobeVar.INVALID_GUID;
    public ulong Guid
    {
        get { return m_Guid; }
    }

    private int m_QuartzId = GlobeVar.INVALID_ID;
    public int QuartzId
    {
        get { return m_QuartzId; }
    }

    private int m_Star = 1;
    public int Star
    {
        get { return m_Star; }
    }

    private int m_Strengthen = 0;
    public int Strengthen
    {
        get { return m_Strengthen; }
    }

    private QuartzAttr m_MainAttr = new QuartzAttr();
    public QuartzAttr MainAttr
    {
        get { return m_MainAttr; }
    }

    private QuartzAttr[] m_AttachAttr = new QuartzAttr[GlobeVar.QUARTZ_ATTACHATTR_COUNT];
    public QuartzAttr[] AttachAttr
    {
        get { return m_AttachAttr; }
    }

    private long m_CreateTime = GlobeVar.INVALID_ID;
    public long CreateTime
    {
        get { return m_CreateTime; }
    }
}

public class QuartzBag
{
    public QuartzBag()
    {
        CleanUp();
    }

    public void CleanUp()
    {
        m_QuartzList.Clear();
        m_NewQuartzGuidList.Clear();
    }

    public bool IsFull(int need = 1)
    {
        return GlobeVar.QUARTZBAG_SIZE - m_QuartzList.Count < need;
    }

    public void HandlePacket(GC_QUARTZ_SYNC pak)
    {
        if (null == pak)
        {
            return;
        }

        m_QuartzList.Clear();
        for (int i = 0; i < pak.QuartzList.Count; i++)
        {
            Quartz quartz = new Quartz();
            quartz.BuildFromProtoQuartz(pak.QuartzList[i]);
            m_QuartzList.Add(quartz);
        }
    }

    public void HandlePacket(GC_QUARTZ_ADD pak)
    {
        if (null == pak)
        {
            return;
        }

        for (int i = 0; i < pak.NewQuartz.Count; i++)
        {
            Quartz quartz = new Quartz();
            quartz.BuildFromProtoQuartz(pak.NewQuartz[i]);
            m_QuartzList.Add(quartz);
            m_NewQuartzGuidList.Add(quartz.Guid);
            m_DropQuartzGuidList.Add(quartz.Guid);
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleQuartzAdd();
        }

    }

    public void HandlePacket(GC_QUARTZ_SELL packet)
    {
        if (null == packet)
        {
            return;
        }

        for (int i = 0; i < packet.QuartzGuid.Count; i++)
        {
            for (int j = 0; j < m_QuartzList.Count; j++)
            {
                if (m_QuartzList[j] == null || false == m_QuartzList[j].IsValid())
                {
                    continue;
                }

                if (m_QuartzList[j].Guid == packet.QuartzGuid[i])
                {
                    m_QuartzList.RemoveAt(j);
                    break;
                }
            }
        }

        if (OrbmentController.Instance != null)
        {
            OrbmentController.Instance.HandleQuartzSell(false);
        }

        if (QuartzSellWindow.Instance != null)
        {
            QuartzSellWindow.Instance.HandleQuartzSell(packet.AddStrengthenStone);
        }
    }

    public void HandlePacket(GC_QUARTZ_STRENGTHEN packet)
    {
        if (null == packet)
        {
            return;
        }

        if (packet.QuartzInfo != null)
        {
            for (int i = 0; i < m_QuartzList.Count; i++)
            {
                if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
                {
                    continue;
                }

                if (m_QuartzList[i].Guid == packet.QuartzInfo.Guid)
                {
                    m_QuartzList[i].BuildFromProtoQuartz(packet.QuartzInfo);
                    break;
                }
            }

            if (OrbmentController.Instance != null)
            {
                OrbmentController.Instance.HandleQuartzStrengthen(false);
            }
        }

        if (QuartzStrengthenWindow.Instance != null)
        {
            QuartzStrengthenWindow.Instance.HandleQuartzStrengthen(packet.Success);
        }
    }

    public void HandlePacket(GC_QUARTZ_EQUIP packet)
    {
        if (null == packet || packet.Quartz == null)
        {
            return;
        }

        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].Guid == packet.Quartz.Guid)
            {
                m_QuartzList.RemoveAt(i);
                break;
            }
        }

        if (packet.OldQuartz != null)
        {
            Quartz quartz = new Quartz();
            quartz.BuildFromProtoQuartz(packet.OldQuartz);
            m_QuartzList.Add(quartz);
        }
    }

    public void HandlePacket(GC_QUARTZ_UNEQUIP packet)
    {
        if (null == packet)
        {
            return;
        }

        Quartz quartz = new Quartz();
        quartz.BuildFromProtoQuartz(packet.Quartz);
        m_QuartzList.Add(quartz);
    }

    public void HandlePacket(GC_CARD_DELETE packet)
    {
        if (packet == null)
        {
            return;
        }

        if (packet.QuartzList.Count <= 0)
        {
            return;
        }

        m_QuartzList.Clear();
        for (int i = 0; i < packet.QuartzList.Count; i++)
        {
            Quartz quartz = new Quartz();
            quartz.BuildFromProtoQuartz(packet.QuartzList[i]);
            m_QuartzList.Add(quartz);
        }
    }

    public void HandlePacket(GC_QUARTZ_DELETE packet)
    {
        if (null == packet)
        {
            return;
        }

        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].Guid == packet.QuartzGuid)
            {
                m_QuartzList.RemoveAt(i);
                break;
            }
        }

        if (packet.CardGuid != GlobeVar.INVALID_GUID)
        {
            Card card = GameManager.PlayerDataPool.PlayerCardBag.GetCard(packet.CardGuid);
            if (card != null && card.Orbment != null)
            {
                card.Orbment.DeleteQuartz(packet.QuartzGuid);
            }
        }
    }

    public Quartz GetQuartz(ulong guid)
    {
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].Guid == guid)
            {
                return m_QuartzList[i];
            }
        }

        return null;
    }

    public int GetQartzCountByClassId(int nClassId)
    {
        int count = 0;
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].GetClassId() == nClassId)
            {
                count += 1;
            }
        }

        return count;
    }

    public int GetQartzCountByClassIdAndSlot(int nClassId, int nSlot)
    {
        int count = 0;
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].GetClassId() != nClassId)
            {
                continue;
            }

            if (nSlot == GlobeVar.INVALID_ID || m_QuartzList[i].GetSlotType() == nSlot)
            {
                count += 1;
            }
        }

        return count;
    }

    public List<Quartz> QuartzFilter(int classid)
    {
        List<Quartz> quartzList = new List<Quartz>();

        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (m_QuartzList[i].GetClassId() != classid)
            {
                continue;
            }

            quartzList.Add(m_QuartzList[i]);
        }

        return quartzList;
    }

    /// <summary>
    /// 对槽进行筛选
    /// </summary>
    /// <param name="type">槽Lists</param>
    /// <returns></returns>
    public List<Quartz> QuartzFilter(List<int> type)
    {
        List<Quartz> quartzList = new List<Quartz>();

        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            int nSlotType = m_QuartzList[i].GetSlotType();

            bool bRet = false;
            for (int j = 0; j < type.Count; j++)
            {
                if (type[j] == nSlotType)
                {
                    bRet = true;
                    break;
                }
            }

            if (bRet == false)
            {
                continue;
            }

            quartzList.Add(m_QuartzList[i]);
        }

        return quartzList;
    }

    public List<Quartz> QuartzFilter(System.Func<Quartz, bool> filter)
    {
        List<Quartz> quartzList = new List<Quartz>();
        for (int i = 0; i < m_QuartzList.Count; i++)
        {
            if (m_QuartzList[i] == null || false == m_QuartzList[i].IsValid())
            {
                continue;
            }

            if (filter != null && !(filter(m_QuartzList[i])))
            {
                continue;
            }

            quartzList.Add(m_QuartzList[i]);
        }

        return quartzList;
    }
    public bool IsHaveNewQuartzGuid(ulong cardGuid)
    {
        return m_NewQuartzGuidList.Contains(cardGuid);
    }

    public void DelNewQuartzGuid(ulong cardGuid)
    {
        if (m_NewQuartzGuidList.Contains(cardGuid))
        {
            m_NewQuartzGuidList.Remove(cardGuid);
        }
    }

    //星辰列表
    private List<Quartz> m_QuartzList = new List<Quartz>();
    public List<Quartz> QuartzList
    {
        get { return m_QuartzList; }
    }

    private List<ulong> m_NewQuartzGuidList = new List<ulong>();
    public List<ulong> NewQuartzGuidList
    {
        get { return m_NewQuartzGuidList; }
    }

    //在战斗结束拦截一份星魂GUID、用于战斗结束，点击掉落星魂，弹星魂自己的Tips
    //每次战斗开始都会清空
    private List<ulong> m_DropQuartzGuidList = new List<ulong>();
    public List<ulong> DropQuartzGuidList
    {
        get { return m_DropQuartzGuidList; }
    }
}

public class QuartzTool
{
    public static int QuartzClassLstSort(OrbmentController.QuartzClass leftClass, OrbmentController.QuartzClass rightClass)
    {
        //如果一个为0且另一个不为0
        if (leftClass.m_Count > 0 && rightClass.m_Count <= 0)
        {
            return -1;
        }
        else if (leftClass.m_Count <= 0 && rightClass.m_Count > 0)
        {
            return 1;
        }
        else
        {
            //比较classID
            if (leftClass.m_ClassId < rightClass.m_ClassId)
            {
                return -1;
            }
            else if (leftClass.m_ClassId > rightClass.m_ClassId)
            {
                return 1;
            }

            else
            {
                if (leftClass.m_Count > rightClass.m_Count)
                {
                    return -1;
                }
                else if (leftClass.m_Count < rightClass.m_Count)
                {
                    return 1;
                }

                else
                {
                    return 0;
                }
            }
        }

    }

    public static int QuartzBagSort_Star_Down(Quartz left, Quartz right)
    {
        if (left == null || false == left.IsValid())
        {
            return 1;
        }

        if (right == null || false == right.IsValid())
        {
            return -1;
        }
        //先按稀有度排
        if (left.Star > right.Star)
        {
            return -1;
        }
        else if (left.Star < right.Star)
        {
            return 1;
        }
        else
        {
            //再按强化等级排
            if (left.Strengthen > right.Strengthen)
            {
                return -1;
            }
            else if (left.Strengthen < right.Strengthen)
            {
                return 1;
            }
            else
            {
                //最后按id排
                if (left.QuartzId < right.QuartzId)
                {
                    return -1;
                }
                else if(left.QuartzId > right.QuartzId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public static int QuartzBagSort_Star_Up(Quartz left, Quartz right)
    {
        return -(QuartzBagSort_Star_Down(left, right));
    }

    public static int QuartzBagSort_Strengthen_Down(Quartz left, Quartz right)
    {
        if (left == null || false == left.IsValid())
        {
            return 1;
        }

        if (right == null || false == right.IsValid())
        {
            return -1;
        }
        //先按强化等级排
        if (left.Strengthen > right.Strengthen)
        {
            return -1;
        }
        else if (left.Strengthen < right.Strengthen)
        {
            return 1;
        }
        else
        {
            //再按稀有度排
            if (left.Star > right.Star)
            {
                return -1;
            }
            else if (left.Star < right.Star)
            {
                return 1;
            }
            else
            {
                //最后按id排
                if (left.QuartzId < right.QuartzId)
                {
                    return -1;
                }
                else if (left.QuartzId > right.QuartzId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public static int QuartzBagSort_Strengthen_Up(Quartz left, Quartz right)
    {
        return -QuartzBagSort_Strengthen_Down(left, right);
    }

    public static int QuartzBagSort_NewGet_Down(Quartz left, Quartz right)
    {
        if (left == null || false == left.IsValid())
        {
            return 1;
        }

        if (right == null || false == right.IsValid())
        {
            return -1;
        }
        //按NetGet排 后获得的在前
        if (left.CreateTime > right.CreateTime)
        {
            return -1;
        }
        else if (left.CreateTime < right.CreateTime)
        {
            return 1;
        }
        else
        {
            //再按稀有度排
            if (left.Star > right.Star)
            {
                return -1;
            }
            else if (left.Star < right.Star)
            {
                return 1;
            }
            else
            {
                if (left.Strengthen < right.Strengthen)
                {
                    return -1;
                }
                else if (left.Strengthen > right.Strengthen)
                {
                    return 1;
                }
                else
                {
                    //最后按id排
                    if (left.QuartzId > right.QuartzId)
                    {
                        return -1;
                    }
                    else if (left.QuartzId < right.QuartzId)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

    public static int QuartzBagSort_NewGet_Up(Quartz left, Quartz right)
    {
        return -QuartzBagSort_NewGet_Down(left, right);
    }


    public static int QuartzClassSort_Star_Down(Quartz left, Quartz right)
    {
        return QuartzBagSort_Star_Down(left, right);
    }
    public static int QuartzClassSort_Star_Up(Quartz left, Quartz right)
    {
        return -QuartzClassSort_Star_Down(left, right);
    }
    public static int QuartzClassSort_Strengthen_Down(Quartz left, Quartz right)
    {
        return QuartzBagSort_Strengthen_Down(left, right);
    }

    public static int QuartzClassSort_Strengthen_Up(Quartz left, Quartz right)
    {
        return -QuartzClassSort_Strengthen_Down(left, right);
    }

    public static int QuartzClassSort_NewGet_Down(Quartz left, Quartz right)
    {
        return QuartzBagSort_NewGet_Down(left, right);
    }

    public static int QuartzClassSort_NewGet_Up(Quartz left, Quartz right)
    {
        return -QuartzClassSort_NewGet_Down(left, right);
    }

    public static int QuartzSort_Sell(Quartz left, Quartz right)
    {
        if (left == null || false == left.IsValid())
        {
            return 1;
        }

        if (right == null || false == right.IsValid())
        {
            return -1;
        }

        if (left.Strengthen < right.Strengthen)
        {
            return -1;
        }
        else if (left.Strengthen > right.Strengthen)
        {
            return 1;
        }
        else
        {
            if (left.Star < right.Star)
            {
                return -1;
            }
            else if (left.Star > right.Star)
            {
                return 1;
            }
            else
            {
                if (left.QuartzId < right.QuartzId)
                {
                    return -1;
                }
                else if (left.QuartzId > right.QuartzId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public static string GetQuartzSlotTypeIcon(int nSlotType)
    {
        switch (nSlotType)
        {
            case 1:
                return "Star_position_01s";
            case 2:
                return "Star_position_02s";
            case 3:
                return "Star_position_03s";
            case 4:
                return "Star_position_04s";
            case 5:
                return "Star_position_05s";
            case 6:
                return "Star_position_06s";
            default:
                return "";
        }
    }

    public static int GetQuartzStarColor(int star)
    {
        switch (star)
        {
            case 1:
                return (int)COLOR.GRAY;
            case 2:
                return (int)COLOR.WHITE;
            case 3:
                return (int)COLOR.GREEN;
            case 4:
                return (int)COLOR.BLUE;
            case 5:
                return (int)COLOR.PURPLE;
            case 6:
                return (int)COLOR.ORANGE;
            default:
                return (int)COLOR.RED;         //异常数据
        }
    }

    public static string GetQuartzStarIconName(int star)
    {
        int color = GetQuartzStarColor(star);
        const string title = "XiYouDu_";
        switch (color)
        {
            case (int)COLOR.GRAY:
                return title + "0";
            case (int)COLOR.WHITE:
                return title + "1";
            case (int)COLOR.GREEN:
                return title + "2";
            case (int)COLOR.BLUE:
                return title + "3";
            case (int)COLOR.PURPLE:
                return title + "4";
            case (int)COLOR.ORANGE:
                return title + "5";
            default:
                return title + "6";          //异常数据
        }
    }

    public static string GetQuartzStarIconName_Circle(int star)
    {
        int color = GetQuartzStarColor(star);
        switch (color)
        {
            case (int)COLOR.NONE:
                return "Star_Quality_empty";
            case (int)COLOR.GRAY:
                return "Star_Quality_1";
            case (int)COLOR.WHITE:
                return "Star_Quality_2";
            case (int)COLOR.GREEN:
                return "Star_Quality_3";
            case (int)COLOR.BLUE:
                return "Star_Quality_4";
            case (int)COLOR.PURPLE:
                return "Star_Quality_5";
            case (int)COLOR.ORANGE:
                return "Star_Quality_6";
            default:
                return "Star_Quality_7";           //异常数据
        }
    }

    public static string GetQuartzStarName(int star)
    {
        int color = GetQuartzStarColor(star);
        switch (color)
        {
            case (int)COLOR.GRAY:
                return StrDictionary.GetDicByID(7853);
            case (int)COLOR.WHITE:
                return StrDictionary.GetDicByID(7854);
            case (int)COLOR.GREEN:
                return StrDictionary.GetDicByID(7855);
            case (int)COLOR.BLUE:
                return StrDictionary.GetDicByID(7856);
            case (int)COLOR.PURPLE:
                return StrDictionary.GetDicByID(7857);
            case (int)COLOR.ORANGE:
                return StrDictionary.GetDicByID(7858);
            default:
                return "";
        }
    }

    public static string GetQuartzIconByClassId(int classId)
    {
        Dictionary<int, List<Tab_Quartz>> dic = TableManager.GetQuartz();
        if (dic == null)
        {
            return "";
        }
        foreach (List<Tab_Quartz> tQuartz in dic.Values)
        {
            if (tQuartz.Count > 0)
            {
                if (tQuartz[0].ClassId == classId)
                {
                    return tQuartz[0].Icon;
                }
            }
        }
        return "";
    }
    //{技能名}（{星宿名}｛n｝件套）：{技能效果}
    public static string GetFormatSetAttr(Tab_QuartzSet tabQuartzSet)
    {
        if (tabQuartzSet == null)
        {
            return "";
        }
        string setName = "";
        Tab_QuartzClass tabQuartzClass = TableManager.GetQuartzClassByID(tabQuartzSet.Id, 0);
        if (tabQuartzClass != null)
        {
            setName = tabQuartzClass.Name;      //套装名
        }
        string ret = "";
        //遍历所有属性   
        for (int i = 0; i < tabQuartzSet.getAttrRefixTypeCount(); i++)
        {
            if (tabQuartzSet.GetAttrRefixTypebyIndex(i) != GlobeVar.INVALID_ID)
            {
                string attrName = Utils.GetAttrRefixName((AttrRefixType)tabQuartzSet.GetAttrRefixTypebyIndex(i));    //属性名
                string value = Utils.GetAttrRefixValueFormatStr(tabQuartzSet.GetAttrRefixTypebyIndex(i), (int)tabQuartzSet.GetAttrRefixValuebyIndex(i));
                if (string.IsNullOrEmpty(ret))
                {
                    ret += StrDictionary.GetDicByID(7762, setName, tabQuartzSet.NeedCount.ToString(), attrName + value);
                }
                else
                {
                    ret += string.Format(",{0}", attrName + value);
                }
            }
        }

        //去impact表读技能名称
        for (int i = 0; i < tabQuartzSet.getSkillIdCount(); i++)
        {
            Tab_SkillEx tSkillEx = TableManager.GetSkillExByID(tabQuartzSet.GetSkillIdbyIndex(i), 0);
            if (tSkillEx == null)
            {
                continue;
            }

            Tab_SkillBase tSkillBase = TableManager.GetSkillBaseByID(tSkillEx.BaseID, 0);
            if (tSkillBase == null)
            {
                continue;
            }
            if (string.IsNullOrEmpty(ret))
            {
                ret += StrDictionary.GetDicByID(7762, setName, tabQuartzSet.NeedCount.ToString(), tSkillEx.Description);
            }
            else
            {
                ret += string.Format(",{0}", tSkillEx.Description);
            }
        }
        return ret;
    }
    //{0}（{1}件套效果）：{2}
    public static string GetFormatSetAttr_Recomand(Tab_QuartzSet tabQuartzSet)
    {
        if (tabQuartzSet == null)
        {
            return "";
        }

        string ret = "";
        //遍历所有属性   
        for (int i = 0; i < tabQuartzSet.getAttrRefixTypeCount(); i++)
        {
            if (tabQuartzSet.GetAttrRefixTypebyIndex(i) != GlobeVar.INVALID_ID)
            {
                string attrName = Utils.GetAttrRefixName((AttrRefixType)tabQuartzSet.GetAttrRefixTypebyIndex(i));    //属性名
                string value = Utils.GetAttrRefixValueFormatStr(tabQuartzSet.GetAttrRefixTypebyIndex(i), (int)tabQuartzSet.GetAttrRefixValuebyIndex(i));
                if (string.IsNullOrEmpty(ret))
                {
                    ret += StrDictionary.GetDicByID(8206, tabQuartzSet.NeedCount.ToString(), attrName + value);
                }
                else
                {
                    ret += string.Format(",{0}", attrName + value);
                }
            }
        }

        //去impact表读技能名称
        for (int i = 0; i < tabQuartzSet.getSkillIdCount(); i++)
        {
            Tab_SkillEx tSkillEx = TableManager.GetSkillExByID(tabQuartzSet.GetSkillIdbyIndex(i), 0);
            if (tSkillEx == null)
            {
                continue;
            }

            Tab_SkillBase tSkillBase = TableManager.GetSkillBaseByID(tSkillEx.BaseID, 0);
            if (tSkillBase == null)
            {
                continue;
            }
            if (string.IsNullOrEmpty(ret))
            {
                ret += StrDictionary.GetDicByID(8206, tabQuartzSet.NeedCount.ToString(), tSkillEx.Description);
            }
            else
            {
                ret += string.Format(",{0}", tSkillEx.Description);
            }
        }
        return ret;
    }
}