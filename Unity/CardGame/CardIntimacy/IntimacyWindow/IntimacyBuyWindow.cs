using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;

//********************************************************************
// 描述:礼物购买窗口 
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//
//********************************************************************
public class IntimacyBuyWindow : MonoBehaviour {
    public GameObject m_Left;
    public GameObject m_Right;

    public UILabel m_Hint;
    private bool m_IsInHide = false;//Item是否处于隐藏装填

    //持有3个Item
    public IntimacyBuyWindowItem[] m_MyItems;
    private List<GiftPair> m_FavoriteGiftList;
    public UIGrid m_Grid;

    //分页算法变量（页数从1开始，最大为PageTotal）
    private int m_PageSize = 3;
    private int m_PageNow = 1;
    private int m_PageTotal=-1;//总页数
    void OnEnable()
    {
        if(m_Grid!=null)
        {
            m_Grid.Reposition();
        }
    }
    /// <summary>
    /// 对外接口
    /// </summary>
    /// <param name="favoriteGiftList">可购买礼物信息List</param>
    public void UpdateAllItem(List<GiftPair> favoriteGiftList)
    {
        if (m_MyItems == null || favoriteGiftList==null)
        {
            return;
        }
        m_PageNow = 1;
        m_FavoriteGiftList = favoriteGiftList;
        if (favoriteGiftList.Count % m_PageSize==0)
        {
            m_PageTotal = (favoriteGiftList.Count / m_PageSize);
        }
        else
        {
            m_PageTotal = (favoriteGiftList.Count / m_PageSize) + 1;
        }
        if (m_PageTotal == 1)
        {
            if (m_Left != null && m_Right != null)
            {
                m_Left.SetActive(false);
                m_Right.SetActive(false);
            }
        }
        else
        {
            if (m_Left != null && m_Right != null)
            {
                m_Left.SetActive(true);
                m_Right.SetActive(true);
            }
        }
        
        UpdateAllItem();
    }

    /// <summary>
    /// 更新所有Item(内部接口，需要先初始化list和分页变量)，在不改变Card的情况下可用
    /// </summary>
    public void UpdateAllItem()
    {
        for (int i = 0; i < m_MyItems.Length; i++)
        {
            //计算下标
            int index=i + (m_PageNow - 1) * m_PageSize;
            if (m_FavoriteGiftList == null || i >= m_FavoriteGiftList.Count || m_FavoriteGiftList[i].ItemId == -1 || index >= m_FavoriteGiftList.Count)
            {
                m_MyItems[i].gameObject.SetActive(false);
                continue;
            }
            if (!m_IsInHide)
            {
                m_MyItems[i].gameObject.SetActive(true);
            }
            //通过ID去取item
            
            Tab_Item item = TableManager.GetItemByID(m_FavoriteGiftList[index].ItemId, 0);
            if(item==null)
            {
                continue;
            }
            Tab_CardIntimacyGiftItem giftItem = TableManager.GetCardIntimacyGiftItemByID(item.Id, 0);
            if(giftItem==null)
            {
                continue;
            }
            //购买限制
            BuyLimitPair pair=IntimacyBuyLimit.GetItemLimit(giftItem.Id);
            m_MyItems[i].InitBuyWindowItem(item.Name, item.Icon, StrDictionary.GetClientDictionaryString("#{6738}", m_FavoriteGiftList[index].IntimacyAdd), item.Id, (CURRENCY_TYPE)giftItem.CurrencyType, giftItem.Univalence, pair);
        }
        //重新排列
        if (m_Grid != null)
        {
            m_Grid.Reposition();
        }
    }

    //点击左翻页
    public void OnClickLeft()
    {
        if(m_PageNow>1)
        {
            m_PageNow--;
            UpdateAllItem();
        }

    }

    //点击右翻页
    public void OnClickRight()
    {
        if (m_PageNow < m_PageTotal)
        {
            m_PageNow++;
            UpdateAllItem();
        }

    }

    //点击某个按键的数字时调用
    public void OnClickBuyItemNum(IntimacyBuyWindowItem item)
    {
        if(item==null)
        {
            return;
        }
        for(int i=0;i<m_MyItems.Length;i++)
        {
            if (item != m_MyItems[i])
            {
                m_MyItems[i].Hide(false);
            }
            else
            {
                m_MyItems[i].Hide(true);
            }
        }
        m_Hint.text = StrDictionary.GetClientDictionaryString("#{6847}");
        m_IsInHide = true;

    }
    //关闭数字键盘时显示所有按键
    public void OnNumClose()
    {
        for(int i=0;i<m_MyItems.Length;i++)
        {
            m_MyItems[i].gameObject.SetActive(true);
            m_MyItems[i].Show();
        }
        m_Hint.text = StrDictionary.GetClientDictionaryString("#{6103}");
        m_IsInHide = false;
    }
}
