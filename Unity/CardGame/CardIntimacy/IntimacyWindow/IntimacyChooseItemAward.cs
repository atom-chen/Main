using Games.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimacyChooseItemAward : MonoBehaviour {
    public UISprite m_Icon;
    public UILabel m_Label;

    private int m_ItemID;
    public void InitAward(UIAtlas atlas,string spriteName,int Count,int ItemId)
    {
        if(m_Icon!=null)
        {
            m_Icon.atlas = atlas;
            m_Icon.spriteName = spriteName;
        }
        if(m_Label!=null)
        {
            m_Label.text = Count + "";
        }
        m_ItemID = ItemId;
    }


    public void ShowAward()
    {
        ItemTipsController.Show(m_ItemID, false);
    }
}
