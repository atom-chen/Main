using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagItemTips : MonoBehaviour
{
    public UILabel m_NameLabel;
    public UISprite m_Icon;
    public UILabel m_Desc;
    public UILabel m_BatchLabel;

    private static BagController.BagItemModel m_Data;
    public static void Show(BagController.BagItemModel data)
    {
        m_Data = data;
        if (data == null)
        {
            return;
        }
        UIManager.ShowUI(UIInfo.ShowItem);
    }
    
    void OnEnable()
    {
        Tab_Item tabItem = TabItemManager.GetItemByID(m_Data.itemId);
        if (tabItem != null)
        {
            m_NameLabel.text = tabItem.name;
            m_Icon.spriteName = tabItem.icon;
            m_Desc.text = tabItem.desc;
            m_BatchLabel.text = string.Format("批量使用({0})", m_Data.count);
        }
    }
    public void OnClickUse()
    {
        //todo 使用物品
    }

    public void OnClickBatchUse()
    {
        //todo 使用物品
    }

    public void OnClickMask()
    {
        m_Data = null;
        UIManager.CloseUI(UIInfo.ShowItem);
    }
}
