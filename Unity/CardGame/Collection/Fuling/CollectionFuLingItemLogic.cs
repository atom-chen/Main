using UnityEngine;
using System.Collections;
using Games;
using Games.GlobeDefine;

public class CollectionFuLingItemLogic : MonoBehaviour {
    private const string m_RareSpriteName = "CommonNew_quality";//稀有度SpriteName前缀

    public UISprite m_FulingSpriteGet = null; // 符灵图片
    public UISprite m_FulingSpriteNoGet = null; // 符灵图片
    public UILabel m_FulingName = null; // 符灵名称
    public UISprite m_BG;//符灵稀有度
    private int m_ID = 0; // 符灵ID

    public void InitItem(int id, string spriteName, string name, bool isGet, CARD_RARE rare)
    {
        if (string.IsNullOrEmpty(spriteName) || string.IsNullOrEmpty(name))
        {
            return;
        }
        if(m_FulingSpriteGet==null || m_FulingSpriteNoGet==null || m_FulingName==null || m_BG==null)
        {
            return;
        }
        //如果已拥有，显示一套UI
        if(isGet)
        {
            m_FulingSpriteGet.gameObject.SetActive(true);
            m_FulingSpriteNoGet.gameObject.SetActive(false);
            m_FulingSpriteGet.spriteName = spriteName;
        }
        //如果未拥有 显示另一套UI
        else
        {
            m_FulingSpriteGet.gameObject.SetActive(false);
            m_FulingSpriteNoGet.gameObject.SetActive(true);
            m_FulingSpriteNoGet.spriteName = spriteName;
        }
        m_ID = id;
        m_FulingName.text = name;
        m_BG.spriteName = m_RareSpriteName + ((int)rare + 1);
    }

    public void OnDetailClick()
    {
        if (null != CollectionRoot.Instance)
        {
            CollectionRoot.Instance.ShowCardDetail(m_ID);
        }
    }
}
