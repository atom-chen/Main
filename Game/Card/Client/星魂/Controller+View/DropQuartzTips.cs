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
    public static void Show(ulong quartzGuid)
    {
        UIManager.ShowUI(UIInfo.StarTips,Init, quartzGuid);
    }
    public static void Init(bool success,object param)
    {
        if (success == false|| m_Instace == null)
        {
            return;
        }
        ulong quartzGuid = (ulong)param;
         m_Instace.InitTips(quartzGuid);
    }
    public void InitTips(ulong quartzGuid)
    {
        if (GameManager.PlayerDataPool.PlayerQuartzBag==null)
        {
            return;
        }

        m_Quartz = GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(quartzGuid);
        if (m_Quartz==null)
        {
            return;
        }
        m_QuartzIcon.spriteName = m_Quartz.GetListIcon();
        m_SlotIcon.spriteName = QuartzTool.GetQuartzSlotTypeIcon(m_Quartz.GetSlotType());
        m_StarIcon.spriteName = QuartzTool.GetQuartzStarIconName(m_Quartz.Star);
        m_NameLabel.text = m_Quartz.GetName();
        m_LevelLabel.gameObject.SetActive(m_Quartz.Strengthen > 0);
        m_LevelLabel.text = StrDictionary.GetClientDictionaryString("#{5160}", m_Quartz.Strengthen);
        m_MainAttr.Init(m_Quartz.MainAttr.RefixType, m_Quartz.MainAttr.AttrValue);
        for (int i = 0; i < m_AttachAttr.Length && i < m_Quartz.AttachAttr.Length; i++)
        {
            m_AttachAttr[i].InitAttach(m_Quartz.AttachAttr[i].RefixType, m_Quartz.AttachAttr[i].AttrValue);
        }

        m_SetAttrLabel.text = "";
        int nClassId = m_Quartz.GetClassId();
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
