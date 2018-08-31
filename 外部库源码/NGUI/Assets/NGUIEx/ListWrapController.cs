﻿using System;
using UnityEngine;
using System.Collections;

public class ListWrapController : MonoBehaviour
{

    public delegate void OnUpdateItem(GameObject item, int index);
    public OnUpdateItem m_delOnUpdateItem = null;

    public UIWrapContent m_WrapContent;
    private UIPanel m_UIPanel = null;
    protected UIScrollView m_UIScrollView = null;

    protected int m_ItemCount = 0;
    public int ItemCount
    {
        get { return m_ItemCount; }
    }
    private bool m_DirectionDown = true;
    private Transform m_Trans = null;
    //private Vector2 _initPanelClipOffset;
    //private Vector3 _initPanelLocalPos;
    private bool m_bFirst = true;
    public bool First
    {
        get { return m_bFirst; }
    }

    protected Vector3 m_StartPos;
    private Transform tl;
    private Transform br;

    //void OnEnable()
    //{
    //    Init();
    //}

    void OnDisable()
    {
        if (m_UIScrollView == null)
        {
            m_UIScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
        }
        if (m_UIScrollView != null)
        {
            m_UIScrollView.DisableSpring();
        }
    }

    void Init()
    {
        m_WrapContent.cullContent = false;
        m_WrapContent.enabled = true;
        m_WrapContent.onInitializeItem = OnInitializeItem;

        m_UIPanel = NGUITools.FindInParents<UIPanel>(gameObject);
        m_UIScrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
        m_Trans = m_WrapContent.transform;

        for (int i = 0; i < m_Trans.childCount; i++)
        {
            Transform child = m_Trans.GetChild(i);
            child.gameObject.SetActive(false);
        }

        //if (tl == null)
        //{
        //    GameObject go = new GameObject("__tl");
        //    go.layer = gameObject.layer;
        //    UIWidget w = go.AddComponent<UIWidget>();
        //    w.width = 2;
        //    w.height = 2;
        //    tl = go.transform;
        //    tl.SetParent(m_UIScrollView.transform);
        //    tl.localScale = Vector3.one;
        //}
        //if (br == null)
        //{
        //    GameObject go = new GameObject("__br");
        //    go.layer = gameObject.layer;
        //    UIWidget w = go.AddComponent<UIWidget>();
        //    w.width = 2;
        //    w.height = 2; br = go.transform;
        //    br.SetParent(m_UIScrollView.transform);
        //    tl.localScale = Vector3.one;
        //}
    }

    private void ResetScroll()
    {
        if (m_UIPanel == null || m_Trans == null)
        {
            return;
        }

        m_WrapContent.SortBasedOnScrollMovement();
        m_WrapContent.UpdateAllItem();

        m_UIScrollView.ResetPosition();
        m_UIScrollView.RestrictWithinBounds(true);
    }

    void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        if (go == null)
        {
            return;
        }

        if (m_delOnUpdateItem != null)
        {
            realIndex *= (m_DirectionDown ? -1 : 1);
            if (0 <= realIndex && realIndex < m_ItemCount)
            {
                go.SetActive(true);
                m_delOnUpdateItem(go, realIndex);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    public void InitList(int itemcount, OnUpdateItem delOnUpdateItem, bool bDownward = true)
    {
        if (m_bFirst)
        {
            Init();
        }

        m_DirectionDown = bDownward;
        m_delOnUpdateItem = delOnUpdateItem;
        UpdateItemCount(itemcount,false);

        //gameObject.SetActive(true);

        if (m_bFirst)
        {
            m_WrapContent.Init();
        }

        ResetScroll();
        m_StartPos = m_UIScrollView.transform.localPosition;

        m_bFirst = false;

        //Invoke("OpenWrap", 0.05f);
    }

    //void OpenWrap()
    //{
    //    m_WrapContent.Init();
    //}

    public void UpdateItemCount(int count,bool refresh = true)
    {
        bool needRestrict = m_ItemCount > count;

        m_ItemCount = count;

        if (m_WrapContent == null)
        {
            return;
        }

        if (m_DirectionDown)
        {
            m_WrapContent.minIndex = 1 - m_ItemCount;
            m_WrapContent.maxIndex = 0;
        }
        else
        {
            m_WrapContent.minIndex = 0;
            m_WrapContent.maxIndex = m_ItemCount - 1;
        }


        //计算用来撑包围盒的widget位置
        //if (tl != null)
        //{
        //    tl.localPosition = Vector3.zero;
        //}
        //if (br != null)
        //{
        //    if (m_UIScrollView.movement == UIScrollView.Movement.Horizontal)
        //    {
        //        br.localPosition = new Vector3(m_ItemCount * m_WrapContent.itemSize / m_WrapContent.column, 0f, 0f);
        //    }
        //    else
        //    {
        //        br.localPosition = new Vector3(0f, -m_ItemCount * m_WrapContent.itemHeight / m_WrapContent.column, 0f);
        //    }
        //}

        if (refresh)
        {
            UpdateAllItem();
            if (needRestrict)
            {
                m_UIScrollView.RestrictWithinBounds(true);
            }
        }
    }

    public void UpdateAllItem()
    {
        if (m_WrapContent != null)
        {
            m_WrapContent.UpdateAllItem();
        }
    }

    public void MoveRelative(float x, float y)
    {
        m_UIScrollView.MoveRelative(new Vector3(x, y, 0f));
        m_UIScrollView.RestrictWithinBounds(true);
    }

    //切换到指定的位置
    public void CenterOn(int index, bool immediate, bool bStrictCenter = false)
    {
        if (m_WrapContent.column == 0)
        {
            return;
        }

        //数据小于等于一页最大显示数，不动
        int nPageShowNum = GetPageShowNum();
        if (nPageShowNum <= 0)
            return;

        //数据小于一页，不动
        if (m_ItemCount < nPageShowNum) //等于时，可能最后条显示不全 需要上拉/右拉
            return;

        if (index >= m_ItemCount)
            index = m_ItemCount - 1;

        if (index < 0)
            index = 0;

        int nPageShowLine = nPageShowNum / m_WrapContent.column;
        int centerLine = (index / m_WrapContent.column) - Mathf.CeilToInt(nPageShowLine * 0.5f) + 1;
        centerLine = Math.Max(0, centerLine);

        MoveScrollView(index, centerLine, nPageShowNum, immediate, bStrictCenter);
    }

    public void SetPageByItemIndex(int index, bool immediate, bool bStrictCenter = false)
    {
        if (m_WrapContent.column == 0)
        {
            return;
        }

        //数据小于等于一页最大显示数，不动
        int nPageShowNum = GetPageShowNum();
        if (nPageShowNum <= 0)
            return;

        //数据小于一页，不动
        if (m_ItemCount < nPageShowNum) //等于时，可能最后条显示不全 需要上拉/右拉
            return;

        if (index >= m_ItemCount)
            index = m_ItemCount - 1;

        if (index < 0)
            index = 0;

        int moveLine = index / m_WrapContent.column;
        moveLine = Math.Max(0, moveLine);

        MoveScrollView(index, moveLine, nPageShowNum, immediate, bStrictCenter);
    }

    int GetPageShowNum()
    {
        if (m_WrapContent.column == 0)
        {
            return 0;
        }

        int childCount = m_WrapContent.GetChildCount();
        if (childCount == 0)
        {
            return 0;
        }
        if (m_WrapContent.itemSize == 0)
        {
            return 0;
        }
        //数据小于等于一页最大显示数，不动
        int nPageShowNum = 0;//一页能显示的数量
        if (m_UIScrollView.movement == UIScrollView.Movement.Horizontal)
            nPageShowNum = Mathf.CeilToInt(m_UIScrollView.panel.width / m_WrapContent.itemSize) * m_WrapContent.column;
        else
            nPageShowNum = Mathf.CeilToInt(m_UIScrollView.panel.height / m_WrapContent.itemHeight) * m_WrapContent.column;
        nPageShowNum = Math.Min(childCount, nPageShowNum);

        return nPageShowNum;
    }

    void MoveScrollView(int nMoveIndex, int nMoveLine, int nPageShowNum, bool immediate, bool bStrictCenter)
    {
        if (m_WrapContent.column == 0)
        {
            return;
        }

        float offset = 0f;
        if (false == bStrictCenter)
        {
            //定位到最后半页 最后条数据对齐最下/最右 以免显示不全
            //eg: 实际能显示下3条半数据，数据有4条，定位到最后条数据时，最后条数据显示不全
            if (m_ItemCount - nMoveIndex <= nPageShowNum / 2)
            {
                offset = Mathf.CeilToInt(m_UIScrollView.panel.height / m_WrapContent.itemHeight) * m_WrapContent.itemHeight - m_UIScrollView.panel.height;
            }
            //数据多余一页的，保证最后页数据铺满，不出现最后个数据出现在中间，后半页空白的情况
            nMoveLine = Mathf.Clamp(nMoveLine, 0, (m_ItemCount - nPageShowNum) / m_WrapContent.column);
        }

        m_UIScrollView.DisableSpring();

        Vector3 localOffset = new Vector3(0f, 0f, 0f);
        if (m_UIScrollView.movement == UIScrollView.Movement.Horizontal)
        {
            localOffset.x = -nMoveLine * m_WrapContent.itemSize - offset;
        }
        else
        {
            localOffset.y = nMoveLine * m_WrapContent.itemHeight + offset;
        }

        if (!m_UIScrollView.canMoveHorizontally) localOffset.x = 0;
        if (!m_UIScrollView.canMoveVertically) localOffset.y = 0;
        localOffset.z = 0f;

        if (immediate)
        {
            //修正item总数大于wrapCountent.childCount的2倍时，单次跳转超过 wrapCountent.GetChildCount * 1.5，显示不正确
            //scrollview move前，预设值wrapcontent items坐标，保证能切到指定项
            m_WrapContent.SetItemOnPage(nMoveLine);

            //直接出发move，panel会自动出发onMove，触发刷新
            m_UIScrollView.ResetPosition();
            m_UIScrollView.MoveRelative(localOffset);
            m_UIScrollView.InvalidateBounds();
            m_UIScrollView.RestrictWithinBounds(true);
            UpdateAllItem();
        }
        else
        {
            // Spring the panel to this calculated position
            SpringPanel.Begin(m_UIScrollView.panel.cachedGameObject
                 , m_StartPos + localOffset, 13f).strength = 8f;
        }
    }
}
