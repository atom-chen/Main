using Games.GlobeDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 奖励图片Item
/// </summary>

public class CollcetionGroupBagGoodsListItem : MonoBehaviour {
    private const string m_SpriteName = "CommonNew_quality";
    public UISprite m_On;
    public UISprite m_Off;
    public UISprite m_BG;//背景框
    
    public void InitItem(string spriteName,bool isGet,CARD_RARE rare)
    {
        if(isGet)
        {
            m_Off.gameObject.SetActive(false);
            m_On.gameObject.SetActive(true);
            m_On.spriteName = spriteName;
        }
        else
        {
            m_On.gameObject.SetActive(false);
            m_Off.gameObject.SetActive(true);
            m_Off.spriteName = spriteName;
        }
        //默认给一个背景框
        if(rare==CARD_RARE.INVALID)
        {
            m_BG.spriteName = m_SpriteName + 4;
        }
        else
        {
            m_BG.spriteName = m_SpriteName + ((int)rare + 1);
        }
    }

}
