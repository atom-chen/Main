using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagEquipItem : MonoBehaviour 
{
    public UISprite m_EquipIcon;
    public UILabel m_EquipNameLabel;

    public Equip EquipData;

    public void Refresh(Equip equipData)
    {
        if(equipData==null)
        {
            m_EquipIcon.gameObject.SetActive(false);
            m_EquipNameLabel.text = "";
            return;
        }
        m_EquipIcon.gameObject.SetActive(true);
        Tab_Item tabItem = equipData.GetTabItem();
        m_EquipIcon.spriteName = tabItem.icon;
        m_EquipNameLabel.text = string.Format(tabItem.name + "+ {0}", equipData.starLevel);
    }


    public void OnClickEquip()
    {
        if(BagController.Instance!=null)
        {
            BagController.Instance.HandleOnEquipClick(this);
        }
    }
}
