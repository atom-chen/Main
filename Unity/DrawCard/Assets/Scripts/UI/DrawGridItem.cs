using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理当前的每个选项
/// </summary>

public class DrawGridItem : MonoBehaviour
{
    [HideInInspector]
    public DRAW_TYPE mType = DRAW_TYPE.INVALID;
    public NameItem[] m_Items;
    public GameObject ItemPrefab;
    public int TotalDraw;
    public int EveryDrawCount
    {
        set
        {
            if (value >= GameDefine.PageMax)
            {
                m_EveryDrawCount = GameDefine.PageMax;
            }
            else
            {
                m_EveryDrawCount = value;
            }
        }
        get
        {
            return m_EveryDrawCount;
        }
    }
    private int m_EveryDrawCount;
    public string m_Title = "";
    [HideInInspector]
    public int m_Executions = 0;     //当前奖项已经抽了多少人
    public void Init(int total,int every,int alreadyDraw,DRAW_TYPE type)
    {
        mType = type;
        m_Executions = every * alreadyDraw;
        SetCount(total, every);
    }
    /// <summary>
    /// 本次抽几个
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        if (TotalDraw - m_Executions >= m_EveryDrawCount)
        {
            return m_EveryDrawCount;
        }
        else
        {
            return TotalDraw - m_Executions;
        }
    }

    //设置
    public void SetCount(int total,int every)
    {
        if(total <= 0 || every <= 0)
        {
            return;
        }
        DestroyChildObj();
        if (every >= GameDefine.PageMax)
        {
            every = GameDefine.PageMax;
        }
        TotalDraw = total;
        m_EveryDrawCount = every;
        //再初始化N个孩子
        for (int i = 0; i < every; i++)
        {
            NGUITools.AddChild(this.gameObject, ItemPrefab);
        }
        m_Items = GetComponentsInChildren<NameItem>();
        //排序
        if (gameObject.GetComponent<UITable>()!=null)
        {
            this.gameObject.GetComponent<UITable>().Reposition();
        }
    }

    private void DestroyChildObj()
    {
        if (m_Items == null)
        {
            return;
        }
        for (int i = 0; i < m_Items.Length; i++)
        {
            if (m_Items[i] != null)
            {
                DestroyImmediate(m_Items[i].gameObject);
                m_Items[i] = null;
            }
        }
    }

    public string GetResidue()
    {
        int ret = GetResidueNum();
        if(ret!= -1)
        {
            return ret.ToString();
        }
        else
        {
            return "";
        }
    }
    //还要抽多少次
    public int GetResidueNum()
    {
        if (m_EveryDrawCount > 0)
        {
            return Mathf.CeilToInt((TotalDraw - m_Executions) / (float)m_EveryDrawCount);
        } 
        else
        {
            return -1;
        }
    }
    public void DrawEnd()
    {
        if (TotalDraw == -1)
        {
            return;
        }
        m_Executions += GetCount();
    }
    //展示
    public void ShowName(List<People> luckBoys)
    {
        for (int i = 0; i < m_Items.Length; i++)
        {
            if(i>=luckBoys.Count)
            {
                m_Items[i].m_IDLabel.text = "???";
                m_Items[i].m_NameLabel.text = "???";
            }
            else
            {
                if (!"0".Equals(luckBoys[i].WorkID))
                {
                    m_Items[i].m_IDLabel.text = luckBoys[i].WorkID.ToString();
                }
                else if (!"0".Equals(luckBoys[i].ProjectGroup))
                {
                    m_Items[i].m_IDLabel.text = luckBoys[i].ProjectGroup;
                }
                else
                {
                    m_Items[i].m_IDLabel.text = luckBoys[i].Department;
                }
                m_Items[i].m_NameLabel.text = luckBoys[i].Name;
            }
        }
    }





}
