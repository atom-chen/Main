/********************************************************************************
 *	文件名：	DropQuartzTips.cs
 *	创建人：	郝俊期
 *	创建时间：2018-04-23
 *
 *	功能说明：战斗结束查看星魂Tips
 *	修改记录：
*********************************************************************************/
using Games;
using Games.GlobeDefine;
using Games.Table;
using ProtobufPacket;
using UnityEngine;

public class DropQuartzTips : MonoBehaviour {

    public UISprite m_QuartzIcon;
    public UISprite m_SlotIcon;
    public UISprite m_StarIcon;
    public UILabel m_NameLabel;
    public UILabel m_LevelLabel;
    public QuartzTipsAttrItem m_MainAttr;
    public QuartzTipsAttrItem[] m_AttachAttr;
    public UILabel m_SetAttrLabel;
    private Quartz m_Quartz = null;

    private static DropQuartzTips m_Instace;
    public static DropQuartzTips Instance
    {
        get { return m_Instace; }
    }
    void OnDisable()
    {
        m_Instace = null;
        m_Quartz = null;
    }
    private void Awake()
    {
        m_Instace = this;
    }
    public static void Show(_QUARTZ pbQuartz)
    {
        if(pbQuartz == null)
        {
            return;
        }
        UIManager.ShowUI(UIInfo.StarTips, InitByPB, pbQuartz);
    }    public static void Show(ulong quartzGuid)
    {
        UIManager.ShowUI(UIInfo.StarTips, InitByGuid, quartzGuid);
    }
    static void InitByPB(bool success,object para)
    {
        if (success == false || m_Instace == null)
        {
            return;
        }
        _QUARTZ quartz = para as _QUARTZ;
        m_Instace.InitTips(quartz);
    }
    static void InitByGuid(bool success,object param)
    {
        if (success == false|| m_Instace == null)
        {
            return;
        }
        ulong quartzGuid = (ulong)param;
         m_Instace.InitTips(quartzGuid);
    }
    void InitTips(_QUARTZ pbQuartz)
    {
        if(pbQuartz == null)
        {
            return;
        }
        Quartz quartz = new Quartz();
        quartz.BuildFromProtoQuartz(pbQuartz);
        Refresh(quartz);
    }
    void InitTips(ulong quartzGuid)
    {
        if (GameManager.PlayerDataPool.PlayerQuartzBag==null)
        {
            return;
        }

        m_Quartz = GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(quartzGuid);
        Refresh(m_Quartz);
    }

    public void Refresh(Quartz nQuartz)
    {
        if (nQuartz == null)
        {
            return;
        }
        m_Quartz = nQuartz;
        m_QuartzIcon.spriteName = nQuartz.GetListIcon();
        m_SlotIcon.spriteName = QuartzTool.GetQuartzSlotTypeIcon(nQuartz.GetSlotType());
        m_StarIcon.spriteName = QuartzTool.GetQuartzStarIconName(nQuartz.Star);
        m_NameLabel.text = nQuartz.GetFullName();
        m_LevelLabel.gameObject.SetActive(nQuartz.Strengthen > 0);
        m_LevelLabel.text = StrDictionary.GetClientDictionaryString("#{5160}", nQuartz.Strengthen);
        m_MainAttr.Init(nQuartz.MainAttr.RefixType, nQuartz.MainAttr.AttrValue);
        for (int i = 0; i < m_AttachAttr.Length && i < nQuartz.AttachAttr.Length; i++)
        {
            m_AttachAttr[i].InitAttach(nQuartz.AttachAttr[i].RefixType, nQuartz.AttachAttr[i].AttrValue);
        }

        m_SetAttrLabel.text = "";
        int nClassId = nQuartz.GetClassId();
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

            if (tSet.NeedClassId != nClassId)
            {
                continue;
            }
            m_SetAttrLabel.text = QuartzTool.GetFormatSetAttr(tSet);
        }
    }
    public void CloseOption()
    {
        UIManager.CloseUI(UIInfo.StarTips);
    }
}
