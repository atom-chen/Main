using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理当前的每个选项
/// </summary>

public class DrawGridController : MonoBehaviour
{
    public DrawItem[] m_Items;
    public GameObject ItemPrefab;
    public int TotalDraw;
    public string m_Title = "";
    [HideInInspector]
    public int m_Executions = 0;     //当前奖项已经抽了多少人
    void Start()
    {
        //初始化n个孩子
        for(int i=0;i<m_Items.Length;i++)
        {
            m_Items[i] = NGUITools.AddChild(this.gameObject, ItemPrefab).GetComponent<DrawItem>();
        }
        //重排
        this.gameObject.GetComponent<UITable>().Reposition();  
    }
    /// <summary>
    /// 获取当前页数
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        return m_Items.Length;
    }
    public void SetCount(int count)
    {
        for (int i = 0; i < m_Items.Length; i++)
        {
            DestroyImmediate(m_Items[i].gameObject);
        }
        if (count >= 12)
        {
            count = 12;
        }
        //再初始化N个孩子
        for (int i = 0; i < count; i++)
        {
            NGUITools.AddChild(this.gameObject, ItemPrefab);
        }
        m_Items = this.GetComponentsInChildren<DrawItem>();
        //排序
        this.gameObject.GetComponent<UITable>().Reposition();
    }
    public string GetResidue()
    {
        if (TotalDraw != -1 && m_Items.Length > 0)
        {
            return ((TotalDraw - m_Executions) / m_Items.Length) + "";
        } else
        {
            return "";
        }
    }
    public int GetResidueNum()
    {
        if (m_Items.Length > 0)
        {
            return (int)((TotalDraw - m_Executions) / m_Items.Length);
        } else
        {
            return -1;
        }
    }
    public void Begin()
    {
        if (TotalDraw == -1)
        {
            return;
        }
        m_Executions += this.GetCount();
    }
    //展示
    public void ShowName(List<People> luckBoys)
    {
        for (int i = 0; i < m_Items.Length; i++)
        {
            if (!"0".Equals(luckBoys[i].WorkID))
            {
                m_Items[i].m_IDLabel.text = luckBoys[i].WorkID.ToString();
            } else if (!"0".Equals(luckBoys[i].ProjectGroup))
            {
                m_Items[i].m_IDLabel.text = luckBoys[i].ProjectGroup;
            } else
            {
                m_Items[i].m_IDLabel.text = luckBoys[i].Department;
            }
            m_Items[i].m_NameLabel.text = luckBoys[i].Name;
        }
    }





}
