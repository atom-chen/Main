using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;

//********************************************************************
// 描述: FulingPreview界面
// 作者: 王必宇
// 创建时间: 2018-02-24
//
//********************************************************************
public class CollectionLevel2FulingPreview : CollectionLevel2PreviewBase
{
    public CollectionFuLingItemLogic[] m_Item;
    void Start()
    {
        if(m_Item==null)
        {
            m_Item = this.GetComponentsInChildren<CollectionFuLingItemLogic>();  
        }

    }

    #region 更新符灵列表相关方法
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
            Dictionary<int, List<Tab_Card>> allCard = TableManager.GetCard();
            if(allCard==null)
            {
                return;
            }
            foreach (var item in allCard)
            {
                allId.Add(item.Key);
            }
            // 根据显示规则对显示的card列表进行排序
            m_PageTotal = ((int)(allCard.Count / PageSize)) + 1;
        }
        allId.Sort(CardSort);
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
            }
            UpdateFulingList(m_CurRare);
        }
        UpdateBtn();
    }
    public void UpdateFulingList(CARD_RARE rare)
    {
        m_CurRare = rare;
        //将上一个筛选项gameobject的off激活
        if (m_CurPitch < m_NotPitchBtn.Length && 0<=m_CurPitch)
        {
            if (m_NotPitchBtn[m_CurPitch]!=null)
            {
                m_NotPitchBtn[m_CurPitch].SetActive(true);
            }
        }
        if (m_CurPitch < m_PitchBtn.Length && 0<=m_CurPitch)
        {
            if (m_PitchBtn[m_CurPitch]!=null)
            {
                m_PitchBtn[m_CurPitch].SetActive(false);
            }
        }

        switch (rare)
        {
            case CARD_RARE.INVALID:
                UpdateAllCardList();
                m_CurPitch = 0;
                m_CurList = allId;
                break;
            case CARD_RARE.N:
                UpdateNCardList();
                m_CurPitch = 5;
                m_CurList = m_NId;
                break;
            case CARD_RARE.R:
                UpdateRCardList();
                m_CurPitch = 4;
                m_CurList = m_RId;
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
        if (0 <= m_CurPitch &&m_CurPitch < m_PitchBtn.Length)
        {
            if(m_PitchBtn[m_CurPitch]!=null)
            {
                m_PitchBtn[m_CurPitch].SetActive(true);
            }

        }
        if (0 <= m_CurPitch && m_CurPitch < m_NotPitchBtn.Length)
        {
            if (m_NotPitchBtn[m_CurPitch] != null)
            {
                m_NotPitchBtn[m_CurPitch].SetActive(false);
            }
        }
    }

    private void UpdateAllCardList()
    {
        if(m_Item==null || allId==null)
        {
            return;
        }
        int j = PageSize * (PageNow - 1);     //符灵ID的下标
        //显示当前页数下的10个符灵
        for (int i = 0; i < m_Item.Length; )
        {
            if(m_Item[i]==null)
            {
                break;
            }
            //如果当前卡片已经达到上限
            if (j>= allId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = allId[j];
            //取出这张卡
            Tab_Card card = TableManager.GetCardByID(id, 0);
            if(card==null)
            {
                break;
            }
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)allId.Count / m_PageSize);
        UpdateBtn();
    }
    // 更新预览列表为N卡列表
    private void UpdateNCardList()
    {
        if (m_Item == null || allId == null)
        {
            return;
        }
        //如果为空，就获取
        if(m_NId==null)
        {
            m_NId = new List<int>();
            for(int i=0;i<allId.Count;i++)
            {
                Tab_Card card = TableManager.GetCardByID(allId[i],0);
                if(card.Rare==(int)CARD_RARE.N)
                {
                    m_NId.Add(allId[i]);
                }
            }
        }
        //开始读取id的下标
        int j =PageSize*(PageNow-1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            //越界检测
            if(j>=m_NId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = m_NId[j];
            //取出这张卡
            Tab_Card card = TableManager.GetCardByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            i++;
            j++;
        }

        m_PageTotal = (int)Mathf.Ceil((float)m_NId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateRCardList()
    {
        if(m_Item==null || allId==null)
        {
            return;
        }
        //如果为空，就获取
        if (m_RId == null)
        {
            m_RId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Card card = TableManager.GetCardByID(allId[i], 0);
                if(card==null)
                {
                    continue;
                }
                if (card.Rare == (int)CARD_RARE.R)
                {
                    m_RId.Add(allId[i]);
                }
            }
        }
        if (m_RId == null)
        {
            return;
        }
        //开始读取id的下标
        int j = PageSize * (PageNow - 1);
        //加载完了 开始贴
        for (int i = 0; i < m_Item.Length; )
        {
            if(m_Item[i]==null)
            {
                break;
            }
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
            Tab_Card card = TableManager.GetCardByID(id, 0);
            if(card==null)
            {
                break;
            }
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            if(baseAttr!=null)
            {
                m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            }
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_RId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateSRCardList()
    {
        if (m_Item == null || allId == null)
        {
            return;
        }
        //如果为空，就获取
        if (m_SRId == null)
        {
            m_SRId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Card card = TableManager.GetCardByID(allId[i], 0);
                if(card==null)
                {
                    continue;
                }
                if (card.Rare == (int)CARD_RARE.SR)
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
            Tab_Card card = TableManager.GetCardByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_SRId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateSSRCardList()
    {
        if (m_Item == null || allId == null)
        {
            return;
        }
        //如果为空，就获取
        if (m_SSRId == null)
        {
            m_SSRId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Card card = TableManager.GetCardByID(allId[i], 0);
                if (card.Rare == (int)CARD_RARE.SSR)
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
            Tab_Card card = TableManager.GetCardByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_SSRId.Count / m_PageSize);
        UpdateBtn();
    }
    private void UpdateURCardList()
    {
        if (m_Item == null || allId == null)
        {
            return;
        }
        //如果为空，就获取
        if (m_URId == null)
        {
            m_URId = new List<int>();
            for (int i = 0; i < allId.Count; i++)
            {
                Tab_Card card = TableManager.GetCardByID(allId[i], 0);
                if(card==null)
                {
                    return;
                }
                if (card.Rare == (int)CARD_RARE.UR)
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
            Tab_Card card = TableManager.GetCardByID(id, 0);
            m_Item[i].gameObject.SetActive(true);
            Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
            m_Item[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id), (CARD_RARE)card.Rare);
            i++;
            j++;
        }
        m_PageTotal = (int)Mathf.Ceil((float)m_URId.Count / m_PageSize);
        UpdateBtn();
    }



    static public int CardSort(int left, int right)
    {
        Tab_Card leftCard = TableManager.GetCardByID(left, 0);
        Tab_Card rightCard = TableManager.GetCardByID(right, 0);

        if (null == leftCard || null == rightCard || null == GameManager.PlayerDataPool)
        {
            return 0;
        }

        // 是否拥有＞稀有度＞符灵ID
        //如果a已拥有，b未拥有
        if (GameManager.PlayerDataPool.IsCardGet(left) && !GameManager.PlayerDataPool.IsCardGet(right))
        {
            return -1;
        }
        //如果a未拥有，b已拥有
        else if (!GameManager.PlayerDataPool.IsCardGet(left) && GameManager.PlayerDataPool.IsCardGet(right))
        {
            return 1;
        } 
        //如果都拥有或都不拥有
        else
        {
            //再按稀有度排
            if (leftCard.Rare > rightCard.Rare)
            {
                return -1;
            } 
            else if (leftCard.Rare < rightCard.Rare)
            {
                return 1;
            } 
            else
            {
                if (left < right)
                {
                    return -1;
                } 
                else if (left > right)
                {
                    return 1;
                } 
                else
                {
                    return 0;
                }
            }
        }
    }

    #region 点击事件
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
    /// <summary>
    /// 点击左按钮后
    /// </summary>
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
    #endregion
#endregion


}
