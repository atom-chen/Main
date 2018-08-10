using UnityEngine;

public class SpriteNum : MonoBehaviour
{
    public string prefix;

    private int m_Num = 1;
    public int num
    {
        get { return m_Num; }
        set { m_Num = value; UpdateSprite(); }
    }

    public UISprite sprite;

    public void UpdateSprite()
    {
        if (sprite == null)
        {
            sprite = GetComponent<UISprite>();
        }
        if (sprite == null)
        {
            return;
        }
        sprite.spriteName = prefix + num;
    }

    public int textValue = 0;
    [ContextMenu("测试")]
    public void Test()
    {
        num = textValue;
    }
}