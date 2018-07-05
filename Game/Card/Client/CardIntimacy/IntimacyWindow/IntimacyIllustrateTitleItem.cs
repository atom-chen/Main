using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimacyIllustrateTitleItem : MonoBehaviour {
    private int m_TitleId = -1;
    public int TitleId
    {
        get
        {
            return m_TitleId;
        }
    }

    public IntimacyIllustrateWindow m_IllustrateWindow;//父物体脚本
    public UISprite m_Icon;
    public GameObject m_PitOn;
    public GameObject m_Line;//和父亲节点的连接线
    private bool m_IsPit = false;//是否选中

    void OnDisable()
    {
        //取消激活时。把所有选中去掉
        if(m_PitOn!=null)
        {
            m_PitOn.gameObject.SetActive(false);
        }
    }

    public void InitTitleItem(int titleId,bool isLink,string spriteName)
    {
        this.m_TitleId = titleId;
        if(m_Line!=null)
        {
            m_Line.SetActive(isLink);
        }
        if(m_Icon!=null)
        {
            m_Icon.spriteName = spriteName;
        }
    }

    /// <summary>
    /// 点击事件方法
    /// </summary>
    public void PitchOn()
    {
        m_IsPit = true;
        if(m_PitOn!=null)
        {
            m_PitOn.gameObject.SetActive(true);
        }
        //选中的是否展示其信息
        if (m_IllustrateWindow != null)
        {
            m_IllustrateWindow.ShowDetail(this.m_TitleId);
        }
    }

    /// <summary>
    /// 去掉勾选
    /// </summary>
    public void CancelPitOn()
    {
        //取消勾选
        m_IsPit = false;
        if(m_PitOn!=null)
        {
            m_PitOn.gameObject.SetActive(false);
        }
    }





}
