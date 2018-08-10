using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagItem : MonoBehaviour 
{
    private UILabel m_Num;
    private UISprite m_Icon;

    public BagController.BagItemModel m_Data; 
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

    public void Init(BagController.BagItemModel data)
    {
        if(data == null)
        {
            this.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(true);
        this.m_Data = data;
        Tab_Item tabItem = TabItemManager.GetItemByID(data.itemId);
        if(tabItem!=null)
        {
            m_Icon.spriteName = tabItem.icon;
        }
        m_Num.text = data.count.ToString();
    }
	
    public void OnClickItem()
    {
        if(BagController.Instance!=null)
        {
            BagController.Instance.HandleOnItemClick(this);
        }
    }
    
}
