using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagEquipTips : MonoBehaviour
{
    private static BagEquipTips _Ins;
    public static BagEquipTips Instance
    {
        get { return _Ins; }
    }

    public UISprite m_IconSprite;
    public UILabel m_NameLabel;

    public UILabel m_QualityLabel;
    public UILabel m_MainAttrLabel;
    public UILabel m_LifeLabel;
    public UILabel m_BattleNumLabel;

    public UILabel m_DescLabel;


    private static Equip m_EquipData;

    void Awake()
    {
        _Ins = this;
    }
    void OnDestroy()
    {
        _Ins = null;
    }
    public static void Show(Equip equipData)
    {
        if (equipData == null)
        {
            return;
        }
        m_EquipData = equipData;
        UIManager.ShowUI(UIInfo.ShowEquip);
    }

    void OnEnable()
    {
        this.gameObject.SetActive(false);
        if (m_EquipData == null)
        {
            return;
        }
        this.gameObject.SetActive(true);
        Tab_Equip tabEquip = m_EquipData.GetTabEquip();
        Tab_Item tabItem = m_EquipData.GetTabItem();
        if (tabEquip == null || tabItem == null)
        {
            return;
        }
        Tab_EquipAttr tabMainAttr = TabEquipAttrManager.GetEquipByID(tabEquip.mainAttrid);
        Tab_EquipAttr tabAssistAttr = TabEquipAttrManager.GetEquipByID(tabEquip.assistAttrid);

        m_IconSprite.spriteName = tabItem.icon;
        m_NameLabel.text = tabItem.name;
        m_DescLabel.text = tabItem.desc;
    }

}
