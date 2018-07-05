using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;
//********************************************************************
// 描述: 图鉴系统各preview的基类
// 作者: wangbiyu
// 创建时间: 2018-3-14
//********************************************************************
public enum PREVIEW_CLOSETYPE
{
    CLOSETYPE_DETAIL,
    CLOSETYPE_LEVEL1,
    CLOSETYPE_GROUP
}

public class CollectionLevel2PreviewBase : MonoBehaviour {
    public GameObject m_Detail = null;
    public GameObject m_GroupObj;//组合界面
    public UILabel m_PageLabel;

    //3个动画
    public TweenAlpha m_Tween1;
    public TweenAlpha m_Tween2;
    public TweenScale m_BGTween;
    public TweenRotation m_BtnTween;

    //6个筛选按钮(选中的)
    public GameObject[] m_PitchBtn;
    //6个筛选按钮（未选中的）
    public GameObject[] m_NotPitchBtn;
    //一开始时默认按哪个筛选
    protected int m_CurPitch = 0;

    protected PREVIEW_CLOSETYPE type = PREVIEW_CLOSETYPE.CLOSETYPE_LEVEL1;

    

    protected CARD_RARE m_CurRare = CARD_RARE.INVALID;

    //总页数
    protected int m_PageTotal=-1;
    protected int page = 1;
    public int PageNow//pageNow最小为1
    {
        set
        {
            page = value;
        }
        get
        {
            return page;
        }
    }   

    //页宽
    protected const int m_PageSize = 10;
    public int PageSize
    {
        get
        {
            return m_PageSize;
        }
    }

    //缓存各款ID
    protected List<int> allId;
    protected List<int> m_NId;
    protected List<int> m_RId;
    protected List<int> m_SRId;
    protected List<int> m_SSRId;
    protected List<int> m_URId;

    protected List<int> m_CurList;//持有当前ID列表的引用
    public List<int> CurList
    {
        get
        {
            return m_CurList;
        }
    }

    //返回动画是否正在播放
    public bool IsTweenPlayNow()
    {
        if(m_Tween1==null || m_Tween2==null || m_BGTween==null || m_BtnTween==null)
        {
            return true;
        }
        //如果动画正在播放
        if (m_Tween1.isActiveAndEnabled || m_Tween2.isActiveAndEnabled || m_BtnTween.isActiveAndEnabled || m_BGTween.isActiveAndEnabled)
        {
            return true;
        }
        return false;
    }

    //翻页、切换筛选项时更新显示
    protected void UpdateBtn()
    {
        //总页数=(总数量/页宽)+1
        if(m_PageLabel!=null)
        {
            m_PageLabel.text = (PageNow) + "/" + m_PageTotal;
        }
    }

    protected void OnEnable()
    {
        if (m_GroupObj!=null)
        {
            m_GroupObj.SetActive(false);
        }
        if(m_Detail!=null)
        {
            m_Detail.gameObject.SetActive(false);
        }
        if (m_BtnTween != null)
        {
            m_BtnTween.ResetToBeginning();
            m_BtnTween.gameObject.SetActive(true);
        }
        //播放动画链的第一个
        if(m_Tween1!=null)
        {
            m_Tween1.PlayForward();
        }
        //将全部的筛选项不激活
        for (int i = 0; i < m_PitchBtn.Length; i++)
        {
            m_PitchBtn[i].SetActive(false);
        }
        for (int i = 0; i < m_NotPitchBtn.Length; i++)
        {
            m_NotPitchBtn[i].SetActive(true);
        }
    }


    #region 由动画触发的切换逻辑
    public void OnClickGroupBtn()
    {
        if (m_BtnTween == null)
        {
            return;
        }
        if(Close(PREVIEW_CLOSETYPE.CLOSETYPE_GROUP))
        {
            m_BtnTween.PlayForward();
        }
    }
    //tween1播放完的回调
    public void OnTween1Finish()
    {
        if (m_Tween1 == null)
        {
            return;
        }
        //如果是出
        if (m_Tween1.value == 1)
        {
            m_BGTween.PlayForward();
        }
        //如果是收（触发关闭事件）
        else
        {
            m_BtnTween.gameObject.SetActive(false);
            //如果要切换到detail
            if (type == 0 && null != m_Detail)
            {
                m_Detail.SetActive(true);
                this.gameObject.SetActive(false);
            }
            //如果要切换到level1
            else if (type == PREVIEW_CLOSETYPE.CLOSETYPE_LEVEL1 && CollectionLevel2View.Instance != null)
            {
                CollectionLevel2View.Instance.gameObject.SetActive(false);
            }
            //如果要切换到GROUP
            else if (type == PREVIEW_CLOSETYPE.CLOSETYPE_GROUP)
            {
                m_GroupObj.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }

    //tween2播放完的回调
    public void OnTween2Finish()
    {
        if (m_Tween2 == null)
        {
            return;
        }
        //如果是收
        if (m_Tween2.value == 0)
        {
            m_BGTween.PlayReverse();
        }
        //如果是出...
    }

    //bg动画播放完的回调
    public void OnBGTweenFinish()
    {
        if (m_BGTween == null)
        {
            return;
        }
        //如果是出
        if (m_BGTween.value.x == 1)
        {
            m_Tween2.PlayForward();
        }
        //如果是收
        else
        {
            m_Tween1.PlayReverse();
        }
    }

    /// <summary>
    /// 关闭Preview
    /// </summary>
    /// <param name="type">告诉Preview你要去哪</param>
    /// <returns>如果动画正在播放，则切换失败</returns>
    public bool Close(PREVIEW_CLOSETYPE type)
    {
        //如果动画正在播放
        if (IsTweenPlayNow())
        {
            return false;
        }
        m_Tween2.PlayReverse();
        this.type = type;
        return true;
    }

    #endregion


 
}
