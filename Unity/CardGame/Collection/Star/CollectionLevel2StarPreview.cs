using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;

//********************************************************************
// 描述: StarPreview界面
// 作者: 王必宇
// 创建时间: 2018-02-26
//
//********************************************************************
public class CollectionLevel2StarPreview : CollectionLevel2PreviewBase
{
    public CollectionStarItemLogic[] m_Item;



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
            Dictionary<int, List<Tab_QuartzClass>> allQuartzClass = TableManager.GetQuartzClass();
            foreach (var item in allQuartzClass)
            {
                allId.Add(item.Key);
            }
            // 根据显示规则对显示的card列表进行排序
            m_PageTotal = ((int)(allQuartzClass.Count / PageSize)) + 1;
        }
        allId.Sort(StarSort);
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
            UpdateStarList(m_CurRare);
        }
        UpdateBtn();
    }



    public void UpdateStarList(CARD_RARE rare)
    {
        m_CurRare = rare;
        //将上一个筛选项gameobject的off激活
        m_NotPitchBtn[m_CurPitch].SetActive(true);
        m_PitchBtn[m_CurPitch].SetActive(false);
        switch (rare)
        {
            case CARD_RARE.INVALID:
                UpdateAllStarList();
                m_CurPitch = 0;
                m_CurList = allId;
                break;
            //case CARD_RARE.N:
            //    UpdateNCardList();
            //    m_CurPitch = 5;
            //    m_CurList = m_NId;
            //    break;
            //case CARD_RARE.R:
            //    UpdateRCardList();
            //    m_CurPitch = 4;
            //    m_CurList = m_RId;
            //    break;
            //case CARD_RARE.SR:
            //    UpdateSRCardList();
            //    m_CurPitch = 3;
            //    m_CurList = m_SRId;
            //    break;
            //case CARD_RARE.SSR:
            //    UpdateSSRCardList();
            //    m_CurPitch = 2;
            //    m_CurList = m_SSRId;
            //    break;
            //case CARD_RARE.UR:
            //    UpdateURCardList();
            //    m_CurPitch = 1;
            //    m_CurList = m_URId;
            //    break;
        }
        m_PitchBtn[m_CurPitch].SetActive(true);
        m_NotPitchBtn[m_CurPitch].SetActive(false);

    }

    private void UpdateAllStarList()
    {
        int j = PageSize * (PageNow - 1);     //在符灵ID数组的下标
        //显示当前页数下的10个星魂
        for (int i = 0; i < m_Item.Length; )
        {
            //如果当前卡片已经达到上限
            if (j>= allId.Count)
            {
                m_Item[i].gameObject.SetActive(false);
                i++;
                j++;
                continue;
            }
            int id = allId[j];
            //取出这个星魂
            Tab_QuartzClass starClass = TableManager.GetQuartzClassByID(id, 0);
            if(starClass==null)
            {
                return;
            }
            m_Item[i].gameObject.SetActive(true);
            Tab_Quartz star = TableManager.GetQuartzByID((id) * 3 - 1, 0);
            if (star!=null)
            {
                m_Item[i].InitItem(id, star.Icon, starClass.Name, GameManager.PlayerDataPool.IsQuartzClassGet(id));
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
        UpdateStarList(m_CurRare);
    }

    public void OnClickRight()
    {
        if (PageNow >= m_PageTotal)
        {
            return;
        }
        PageNow++;
        UpdateStarList(m_CurRare);
    }







    //// 更新预览列表为N卡列表
    //private void UpdateNCardList()
    //{
    //    //如果为空，就获取
    //    if (m_NStarId == null)
    //    {
    //        m_NStarId = new List<int>();
    //        for (int i = 0; i < allStarId.Count; i++)
    //        {
    //            Tab_Quartz star = TableManager.GetQuartzByID(allStarId[i], 0);
    //            if (star == (int)CARD_RARE.N)
    //            {
    //                m_NStarId.Add(allStarId[i]);
    //            }
    //        }
    //    }
    //    //开始读取id的下标
    //    int j = PageSize * (PageNow - 1);
    //    //加载完了 开始贴
    //    for (int i = 0; i < m_StarItem.Length; )
    //    {
    //        //越界检测
    //        if (j >= m_NStarId.Count)
    //        {
    //            m_StarItem[i].gameObject.SetActive(false);
    //            i++;
    //            j++;
    //            continue;
    //        }
    //        int id = m_NStarId[j];
    //        //取出这张卡
    //        Tab_Card card = TableManager.GetCardByID(id, 0);
    //        m_StarItem[i].gameObject.SetActive(true);
    //        Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
    //        m_StarItem[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id));
    //        i++;
    //        j++;
    //    }

    //    m_PageTotal = (int)Mathf.Ceil((float)m_NStarId.Count / m_PageSize);
    //    UpdateBtn();
    //}
    //private void UpdateRCardList()
    //{
    //    //如果为空，就获取
    //    if (m_RStarId == null)
    //    {
    //        m_RStarId = new List<int>();
    //        for (int i = 0; i < allStarId.Count; i++)
    //        {
    //            Tab_Card card = TableManager.GetCardByID(allStarId[i], 0);
    //            if (card.Rare == (int)CARD_RARE.R)
    //            {
    //                m_RStarId.Add(allStarId[i]);
    //            }
    //        }
    //    }
    //    //开始读取id的下标
    //    int j = PageSize * (PageNow - 1);
    //    //加载完了 开始贴
    //    for (int i = 0; i < m_StarItem.Length; )
    //    {
    //        //越界检测
    //        if (j >= m_RStarId.Count)
    //        {
    //            m_StarItem[i].gameObject.SetActive(false);
    //            i++;
    //            j++;
    //            continue;
    //        }
    //        int id = m_RStarId[j];
    //        //取出这张卡
    //        Tab_Card card = TableManager.GetCardByID(id, 0);
    //        m_StarItem[i].gameObject.SetActive(true);
    //        Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
    //        m_StarItem[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id));
    //        i++;
    //        j++;
    //    }
    //    m_PageTotal = (int)Mathf.Ceil((float)m_RStarId.Count / m_PageSize);
    //    UpdateBtn();
    //}
    //private void UpdateSRCardList()
    //{
    //    //如果为空，就获取
    //    if (m_SRStarId == null)
    //    {
    //        m_SRStarId = new List<int>();
    //        for (int i = 0; i < allStarId.Count; i++)
    //        {
    //            Tab_Card card = TableManager.GetCardByID(allStarId[i], 0);
    //            if (card.Rare == (int)CARD_RARE.SR)
    //            {
    //                m_SRStarId.Add(allStarId[i]);
    //            }
    //        }
    //    }
    //    //开始读取id的下标
    //    int j = PageSize * (PageNow - 1);
    //    //加载完了 开始贴
    //    for (int i = 0; i < m_StarItem.Length; )
    //    {
    //        //越界检测
    //        if (j >= m_SRStarId.Count)
    //        {
    //            m_StarItem[i].gameObject.SetActive(false);
    //            i++;
    //            j++;
    //            continue;
    //        }
    //        int id = m_SRStarId[j];
    //        //取出这张卡
    //        Tab_Card card = TableManager.GetCardByID(id, 0);
    //        m_StarItem[i].gameObject.SetActive(true);
    //        Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
    //        m_StarItem[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id));
    //        i++;
    //        j++;
    //    }
    //    m_PageTotal = (int)Mathf.Ceil((float)m_SRStarId.Count / m_PageSize);
    //    UpdateBtn();
    //}
    //private void UpdateSSRCardList()
    //{
    //    //如果为空，就获取
    //    if (m_SSRStarId == null)
    //    {
    //        m_SSRStarId = new List<int>();
    //        for (int i = 0; i < allStarId.Count; i++)
    //        {
    //            Tab_Card card = TableManager.GetCardByID(allStarId[i], 0);
    //            if (card.Rare == (int)CARD_RARE.SSR)
    //            {
    //                m_SSRStarId.Add(allStarId[i]);
    //            }
    //        }
    //    }
    //    //开始读取id的下标
    //    int j = PageSize * (PageNow - 1);
    //    //加载完了 开始贴
    //    for (int i = 0; i < m_StarItem.Length; )
    //    {
    //        //越界检测
    //        if (j >= m_SSRStarId.Count)
    //        {
    //            m_StarItem[i].gameObject.SetActive(false);
    //            i++;
    //            j++;
    //            continue;
    //        }
    //        int id = m_SSRStarId[j];
    //        //取出这张卡
    //        Tab_Card card = TableManager.GetCardByID(id, 0);
    //        m_StarItem[i].gameObject.SetActive(true);
    //        Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
    //        m_StarItem[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id));
    //        i++;
    //        j++;
    //    }
    //    m_PageTotal = (int)Mathf.Ceil((float)m_SSRStarId.Count / m_PageSize);
    //    UpdateBtn();
    //}
    //private void UpdateURCardList()
    //{
    //    //如果为空，就获取
    //    if (m_URStarId == null)
    //    {
    //        m_URStarId = new List<int>();
    //        for (int i = 0; i < allStarId.Count; i++)
    //        {
    //            Tab_Card card = TableManager.GetCardByID(allStarId[i], 0);
    //            if (card.Rare == (int)CARD_RARE.UR)
    //            {
    //                m_URStarId.Add(allStarId[i]);
    //            }
    //        }
    //    }
    //    //开始读取id的下标
    //    int j = PageSize * (PageNow - 1);
    //    //加载完了 开始贴
    //    for (int i = 0; i < m_StarItem.Length; )
    //    {
    //        //越界检测
    //        if (j >= m_URStarId.Count)
    //        {
    //            m_StarItem[i].gameObject.SetActive(false);
    //            i++;
    //            j++;
    //            continue;
    //        }
    //        int id = m_URStarId[j];
    //        //取出这张卡
    //        Tab_Card card = TableManager.GetCardByID(id, 0);
    //        m_StarItem[i].gameObject.SetActive(true);
    //        Tab_RoleBaseAttr baseAttr = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(0), 0);
    //        m_StarItem[i].InitItem(id, card.GetHeadIconbyIndex(0), baseAttr.Name, GameManager.PlayerDataPool.IsCardGet(id));
    //        i++;
    //        j++;
    //    }
    //    m_PageTotal = (int)Mathf.Ceil((float)m_URStarId.Count / m_PageSize);
    //    UpdateBtn();
    //}
    ///// <summary>
    ///// 点击左按钮后
    ///// </summary>


    // 星魂预览界面排序规则
    static public int StarSort(int left, int right)
    {
        Tab_QuartzClass leftStarClass = TableManager.GetQuartzClassByID(left, 0);
        Tab_QuartzClass rightStarClass = TableManager.GetQuartzClassByID(right, 0);

        if (null == leftStarClass || null == rightStarClass || null == GameManager.PlayerDataPool)
        {
            return 0;
        }
        // 是否拥有＞星魂ID
        if (GameManager.PlayerDataPool.IsQuartzClassGet(left) && !GameManager.PlayerDataPool.IsQuartzClassGet(right))
        {
            return -1;
        } 
        else if (!GameManager.PlayerDataPool.IsQuartzClassGet(left) && GameManager.PlayerDataPool.IsQuartzClassGet(right))
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
