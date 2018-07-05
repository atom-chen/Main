using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;

public class CollectionLevel2TalismanPreview : CollectionLevel2PreviewBase
{
    public CollectionTalismanItemLogic[] m_Item;

    void OnEnable()
    {
        base.OnEnable();
        if (allId == null || allId.Count == 0)
        {
            if (null == GameManager.PlayerDataPool)
            {
                return;
            }
            if (null == allId)
            {
                allId = new List<int>();//存储所有符灵ID
            }
            Dictionary<int, List<Tab_Talisman>> allTalisman = TableManager.GetTalisman();
            if(allTalisman==null)
            {
                return;
            }
            foreach (var item in allTalisman)
            {
                allId.Add(item.Key);
            }
            // 根据显示规则对显示的card列表进行排序
            m_PageTotal = ((int)(allTalisman.Count / PageSize)) + 1;
        }
        allId.Sort(TalismanSort);
        //初始化预览列表
        if (m_Item != null)
        {
            switch (m_CurPitch)
            {
                case 0:
                    m_CurRare = CARD_RARE.INVALID;
                    break;
                case 1:
                    m_CurRare = CARD_RARE.UR;
                    break;
                case 2:
                    m_CurRare = CARD_RARE.SSR;
                    break;
                case 3:
                    m_CurRare = CARD_RARE.SR;
                    break;
                case 4:
                    m_CurRare = CARD_RARE.R;
                    break;
                case 5:
                    m_CurRare = CARD_RARE.N;
                    break;
                default:
                    m_CurRare = CARD_RARE.INVALID;
                    break;
            }
            UpdateFulingList(m_CurRare);
        }
        UpdateBtn();
    }

    public void UpdateFulingList(CARD_RARE rare)
    {
        m_CurRare = rare;
        //将上一个筛选项gameobject的off激活
        m_NotPitchBtn[m_CurPitch].SetActive(true);
        m_PitchBtn[m_CurPitch].SetActive(false);
        switch (rare)
        {
            case CARD_RARE.INVALID:
                UpdateAllTalismanList();
                m_CurPitch = 0;
                m_CurList = allId;
                break;
            case CARD_RARE.N:
                UpdateNTalismanList();
                m_CurPitch = 5;
                m_CurList = m_NId;
                break;
            case CARD_RARE.R:
                UpdateRCardList();
                m_CurPitch = 4;
                m_CurList = m_RId;;
                break;
            case CARD_RARE.SR:
                UpdateSRCardList();
                m_CurPitch = 3;
                m_CurList = m_SRId;
                break;
            case CARD_RARE.SSR:
                UpdateSSRCardList();
                m_CurPitch = 2;
                m_CurList = m_SSRId;
                break;
            case CARD_RARE.UR:
                UpdateURCardList();
                m_CurPitch = 1;
                m_CurList = m_URId;
                break;
        }
        m_PitchBtn[m_CurPitch].SetActive(true);
        m_NotPitchBtn[m_CurPitch].SetActive(false);

    }
    private void UpdateAllTalismanList()
    {
        int j = PageSize * (PageNow - 1);     //ID的下标
        //显示当前页数下的10个物华
        for (int i = 0; i < m_Item.Length; )
        {
            //如果当前卡片已经达到上限
            if (j >= allId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = allId[j];
            //取出这个物华
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)allId.Count / m_PageSize);
        UpdateBtn();
    }
    public void OnClickLeft()
    {
        if (PageNow <= 1)
        {
            return;
        }
        PageNow--;
        UpdateFulingList(m_CurRare);
    }
    public void OnClickRight()
    {
        if (PageNow >= m_PageTotal)
        {
            return;
        }
        PageNow++;
        UpdateFulingList(m_CurRare);
    }
    /// <summary>
    /// 更新左右按钮、页数Label的显示
    /// </summary>

    // 法宝预览界面排序规则
    static public int TalismanSort(int left, int right)
    {
        Tab_Talisman leftTalisman = TableManager.GetTalismanByID(left, 0);
        Tab_Talisman rightTalisman = TableManager.GetTalismanByID(right, 0);

        if (null == leftTalisman || null == rightTalisman || null == GameManager.PlayerDataPool)
        {
            return 0;
        }

        // 是否拥有＞稀有度＞法宝ID
        if (GameManager.PlayerDataPool.IsTalismanGet(left) && !GameManager.PlayerDataPool.IsTalismanGet(right))
        {
            return -1;
        } else if (!GameManager.PlayerDataPool.IsTalismanGet(left) && GameManager.PlayerDataPool.IsTalismanGet(right))
        {
            return 1;
        } else
        {
            if (leftTalisman.Rare > rightTalisman.Rare)
            {
                return -1;
            } else if (leftTalisman.Rare < rightTalisman.Rare)
            {
                return 1;
            } else
            {
                if (left < right)
                {
                    return -1;
                } else if (left > right)
                {
                    return 1;
                } else
                {
                    return 0;
                }
            }
        }
    }

    public void OnClickAll()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.INVALID);
    }
    public void OnClicN()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.N);
    }
    public void OnClickR()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.R);
    }
    public void OnClickSR()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.SR);
    }
    public void OnClickSSR()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.SSR);
    }
    public void OnClickUR()
    {
        PageNow = 1;
        UpdateFulingList(CARD_RARE.UR);
    }

    // 更新预览列表为N卡列表
    private void UpdateNTalismanList()
    {
        //如果为空，就获取
        if (m_NId == null)
        {
            m_NId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Talisman talisman = TableManager.GetTalismanByID(allId[i], 0);
                if (talisman.Rare == (int)CARD_RARE.N)
                {
                    m_NId.Add(allId[i]);
                }
            }
        }
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if (j >= m_NId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_NId[j];
            //取出这张卡
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            i++;
            j++;
        }

        m_PageTotal = (int)Mathf.Ceil((float)m_NId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateRCardList()
    {
        //如果为空，就获取
        if (m_RId == null)
        {
            m_RId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Talisman talisman = TableManager.GetTalismanByID(allId[i], 0);
                if (talisman.Rare == (int)CARD_RARE.R)
                {
                    m_RId.Add(allId[i]);
                }
            }
        }
        m_RId.Sort(TalismanSort);
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if (j >= m_RId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_RId[j];
            //取出这张卡
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_RId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateSRCardList()
    {
        //如果为空，就获取
        if (m_SRId == null)
        {
            m_SRId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Talisman talisman = TableManager.GetTalismanByID(allId[i], 0);
                if (talisman.Rare == (int)CARD_RARE.SR)
                {
                    m_SRId.Add(allId[i]);
                }
            }
        }
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if (j >= m_SRId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_SRId[j];
            //取出这张卡
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_SRId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateSSRCardList()
    {
        //如果为空，就获取
        if (m_SSRId == null)
        {
            m_SSRId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Talisman talisman = TableManager.GetTalismanByID(allId[i], 0);
                if (talisman.Rare == (int)CARD_RARE.SSR)
                {
                    m_SSRId.Add(allId[i]);
                }
            }
        }
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if (j >= m_SSRId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_SSRId[j];
            //取出这张卡
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_SSRId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateURCardList()
    {
        //如果为空，就获取
        if (m_URId == null)
        {
            m_URId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Talisman talisman = TableManager.GetTalismanByID(allId[i], 0);
                if (talisman.Rare == (int)CARD_RARE.UR)
                {
                    m_URId.Add(allId[i]);
                }
            }
        }
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if (j >= m_URId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_URId[j];
            //取出这张卡
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId, 0);
            if (tabTalismanVisual != null)
            {
                m_Item[i].InitItem(id, tabTalismanVisual.Icon, talisman.Name, GameManager.PlayerDataPool.IsTalismanGet(id));
            }
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_URId.Count / m_PageSize);
        UpdateBtn();
    }
}
