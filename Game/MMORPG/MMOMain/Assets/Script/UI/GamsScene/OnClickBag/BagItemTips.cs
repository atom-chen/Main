using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagItemTips : MonoBehaviour
{
    public UILabel m_Name;
    public UISprite m_Icon;
    public UILabel m_Desc;
    public UILabel m_BatchLabel;

    private BagController.BagItemModel m_Data;
    public void Show(BagController.BagItemModel data)
    {
        m_Data = data;
        if (data == null)
        {
            return;
        }
        this.gameObject.SetActive(true);
        Tab_Item tabItem = TabItemManager.GetItemByID(data.itemId);
        if (tabItem != null)
        {
            m_Name.text = tabItem.name;
            m_Icon.spriteName = tabItem.icon;
            m_Desc.text = tabItem.desc;
            m_BatchLabel.text = string.Format("批量使用({0})", data.count);
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
        this.gameObject.SetActive(false);
    }
}
