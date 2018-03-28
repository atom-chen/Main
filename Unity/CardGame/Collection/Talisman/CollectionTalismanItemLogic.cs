using UnityEngine;
using System.Collections;
using Games;

public class CollectionTalismanItemLogic : MonoBehaviour {

    public UISprite m_TalismanSpriteGet = null; // 符灵图片
    public UISprite m_TalismanSpriteNoGet = null; // 符灵图片
    public UILabel m_TalismanName = null; // 符灵名称
    private int m_ID = 0; // 符灵ID

    public void InitItem(int id, string spriteName, string name,bool isGet)
    {
        if (string.IsNullOrEmpty(spriteName) || string.IsNullOrEmpty(name))
        {
            return;
        }
        if (m_TalismanSpriteGet == null || m_TalismanSpriteNoGet == null || m_TalismanName == null)
        {
            return;
        }
        //如果已拥有，显示一套UI
        if (isGet)
        {
            m_TalismanSpriteGet.gameObject.SetActive(true);
            m_TalismanSpriteNoGet.gameObject.SetActive(false);
            m_TalismanSpriteGet.spriteName = spriteName;
        }
            //如果未拥有 显示另一套UI
        else
        {
            m_TalismanSpriteGet.gameObject.SetActive(false);
            m_TalismanSpriteNoGet.gameObject.SetActive(true);
            m_TalismanSpriteNoGet.spriteName = spriteName;
        }
        m_ID = id;
        m_TalismanName.text = name;
    }

    public void OnDetailClick()
    {
        if (null != CollectionRoot.Instance)
        {
            CollectionRoot.Instance.ShowTalismanDetail(m_ID);
        }
    }
}
