using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageWrapController : MonoBehaviour
{
    public delegate void OnUpdateItem(GameObject item, int index);
    public OnUpdateItem m_delOnUpdateItem = null;

    public delegate void OnUpdatePage(int curPage, int totalPage);
    public OnUpdatePage m_delOnUpdatePage = null;

    public UIGrid m_Grid;

    private int m_ItemCount = 0;
    private int m_CurPage = 1;

    private BetterList<GameObject> m_Children = new BetterList<GameObject>();

    public int CurPage
    {
        get { return m_CurPage; }
    }

    public int TotalPage
    {
        get
        {
            if (ItemPerPage == 0)
                return 1;
            return m_ItemCount/ItemPerPage + 1;
        }
    }

    public int ItemPerPage
    {
        get { return m_Children.size; }
    }

    public enum PageDir
    {
        Vertical
    }
    private PageDir mDirection = PageDir.Vertical;
    public PageDir Direction { get { return mDirection; } }

    private Transform m_Trans = null;

    public void Init(int itemCount, OnUpdateItem delUpdateItem, OnUpdatePage delUpdatePage)
    {
        m_ItemCount = itemCount;
        m_CurPage = 1;
        mDirection = PageDir.Vertical;

        m_delOnUpdateItem = delUpdateItem;
        m_delOnUpdatePage = delUpdatePage;

        InitChildren();

        Refresh();

        if (m_delOnUpdatePage != null)
        {
            m_delOnUpdatePage(CurPage, TotalPage);
        }
    }

    void InitChildren()
    {
        m_Children.Clear();
        var children = m_Grid.GetChildList();
        foreach (var child in children)
        {
            m_Children.Add(child.gameObject);
        }
    }

    public void Refresh()
    {
        if (CurPage < 1 || CurPage > TotalPage)
            return;

        int start = (CurPage - 1)*ItemPerPage;
        for (int i = start; i < start + ItemPerPage; ++i)
        {
            if (i < m_ItemCount)
            {
                m_Children[i-start].SetActive(true);
                if (m_delOnUpdateItem != null)
                {
                    m_delOnUpdateItem(m_Children[i - start], i);
                }
            }
            else
            {
                m_Children[i-start].SetActive(false);
            }
        }

        m_Grid.Reposition();
    }

    public void PageUp()
    {
        if (CurPage < 1 || CurPage >= TotalPage)
            return;
        m_CurPage++;
        Refresh();
        if (m_delOnUpdatePage != null)
        {
            m_delOnUpdatePage(CurPage, TotalPage);
        }
    }

    public void PageDown()
    {
        if (CurPage <= 1 || CurPage > TotalPage)
            return;
        m_CurPage--;
        Refresh();
        if (m_delOnUpdatePage != null)
        {
            m_delOnUpdatePage(CurPage, TotalPage);
        }
    }
}
