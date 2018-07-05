using UnityEngine;
using System.Collections;
using Games;
using Games.Table;

public class CollectionStarItemLogic : MonoBehaviour {
    public UISprite m_StarSpriteGet;
    public UISprite m_StarSpriteNoGet;
    public UILabel m_StarName = null; // 星魂名称
    private int m_ID = 0; // 星魂ID

    public void InitItem(int id, string spriteName, string name, bool isGet)
    {
        if (string.IsNullOrEmpty(spriteName) || string.IsNullOrEmpty(name))
        {
            return;
        }
        if (m_StarSpriteGet == null || m_StarSpriteNoGet == null || m_StarName == null)
        {
            return;
        }
        //如果已拥有，显示一套UI
        if (isGet)
        {
            m_StarSpriteGet.gameObject.SetActive(true);
            m_StarSpriteNoGet.gameObject.SetActive(false);
            m_StarSpriteGet.spriteName = spriteName;
        }
            //如果未拥有 显示另一套UI
        else
        {
            m_StarSpriteGet.gameObject.SetActive(false);
            m_StarSpriteNoGet.gameObject.SetActive(true);
            m_StarSpriteNoGet.spriteName = spriteName;
        }
        m_ID = id;
        m_StarName.text = name;
    }

    public void OnDetailClick()
    {
        if (null != CollectionRoot.Instance)
        {
            CollectionRoot.Instance.ShowStarDetail(m_ID);
        }
    }
}
