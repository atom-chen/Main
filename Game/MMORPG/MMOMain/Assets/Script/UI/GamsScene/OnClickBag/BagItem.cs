using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagItem : MonoBehaviour {
    private ITEM_FIRST type;
    private int id;
    private UILabel m_Num;
    private UISprite m_Icon;
	void Start () 
    {
        Transform tr = transform.Find("NUM");
        if(tr!=null)
        {
            m_Num = tr.GetComponent<UILabel>();
        }
        tr = transform.Find("Sprite");
        if(tr!=null)
        {
            m_Icon = tr.GetComponent<UISprite>();
        }
	}

    public void Init(ITEM_FIRST type,int id)
    {
        this.type=type;
        this.id=id;
        //根据ID取物品
        switch(type)
        {
            case ITEM_FIRST.DRUG:
                Item item = PlayData.RoleData.bag.GetItemByTabId(id);
                if(item!=null)
                {
                    m_Num.text = item.count.ToString();
                    m_Num.gameObject.SetActive(true);
                    Tab_Item tabItem = item.GetTabItem();
                    if (tabItem != null)
                    {
                        m_Icon.spriteName = tabItem.icon;
                    }
                }
                break;
            case ITEM_FIRST.EQUIP:
                Equip equip = PlayData.RoleData.equipBag.GetEquipById(id);
                m_Num.gameObject.SetActive(false);
                if(equip!=null)
                {
                    Tab_Item tabItem = equip.GetTabItem();
                    if(tabItem!=null)
                    {
                        m_Icon.spriteName = tabItem.icon;
                    }
                }
                break;
        }
    }
	
    
}
