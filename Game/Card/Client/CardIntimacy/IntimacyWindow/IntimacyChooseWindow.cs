using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
//********************************************************************
// 描述: 亲密度升级选择窗口
// 作者: wangbiyu
// 创建时间: 2018-3-9
//
//
//********************************************************************
public class IntimacyChooseWindow : MonoBehaviour
{
    public UIGrid m_Grid;
    public IntimacyChooseItem[] m_Item;

    void OnEnable()
    {
        if (m_Grid != null)
        {
            m_Grid.Reposition();
        }
    }

    /// <summary>
    /// 更新选择称号界面信息
    /// </summary>
    /// <param name="nextTitle">几个可选的Title信息</param>
    public void InitChooseWindow(List<Tab_CardIntimacyTitle> nextTitle, int CardId)
    {
        if (m_Item == null)
        {
            return;
        }
        for (int i = 0; i < m_Item.Length; i++)
        {
            if(m_Item[i]==null)
            {
                continue;
            }
            if ( i >= nextTitle.Count || nextTitle[i] == null)
            {
                m_Item[i].gameObject.SetActive(false);
                continue;
            }
            m_Item[i].gameObject.SetActive(true);
            
            List<UIAtlas> Atlas = IntimacyRoot.Instance.GetUIAtlas(nextTitle[i].Id);
            List<string> Icon = IntimacyRoot.Instance.GetSpriteName(nextTitle[i].Id);
            List<AwardItem> Count = IntimacyRoot.Instance.GetItem(nextTitle[i].Id);

            m_Item[i].InitChooseItem(nextTitle[i].Title, nextTitle[i].IntimacyLevel, nextTitle[i].Desc, nextTitle[i].Icon, nextTitle[i].Id, Atlas, Icon, Count);
        }
        if (m_Grid != null)
        {
            m_Grid.Reposition();
        }
    }


}
