using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//********************************************************************
// 描述: 管理所有二级界面
// 作者: wangbiyu
// 创建时间: 2018-3-3
//
//
//********************************************************************
public class CollectionLevel2View : MonoBehaviour {
    public CollectionLevel1View m_Level1;
    private static CollectionLevel2View _Instance;
    public static CollectionLevel2View Instance
    {
        get
        {
            return _Instance;
        }
    }
    public TweenAlpha[] m_TweenAlpha;
    public TweenPosition[] m_TweenPosition;

    public UILabel m_TitileLabel = null; // 标题
    public CollectionLevel2Fuling m_Level2Fuling = null; // 二级界面符灵界面
    public CollectionLevel2Star m_Level2Star = null; // 二级界面星魂界面
    public CollectionLevel2Talisman m_Level2Talisman = null; // 二级界面法宝界面
    void Awake()
    {
        _Instance=this;
    }

    /// <summary>
    /// 激活时播放展开动画
    /// </summary>
    void OnEnable()
    {
        if(m_Level2Fuling!=null)
        {
            m_Level2Fuling.gameObject.SetActive(false);
        }
        if (m_Level2Star!=null)
        {
            m_Level2Star.gameObject.SetActive(false);
        }
        if(m_Level2Talisman!=null)
        {
            m_Level2Talisman.gameObject.SetActive(false);
        }
        if(m_TweenAlpha!=null)
        {
            for(int i=0;i<m_TweenAlpha.Length;i++)
            {
                if(m_TweenAlpha[i]!=null)
                {
                    m_TweenAlpha[i].PlayForward();
                }
            }
        }
        if (m_TweenPosition != null)
        {
            for (int i = 0; i < m_TweenPosition.Length; i++)
            {
                if (m_TweenPosition[i] != null)
                {
                    m_TweenPosition[i].PlayForward();
                }
            }
        }
    }
    void OnDisable()
    {
        if(m_Level1!=null)
        {
            m_Level1.gameObject.SetActive(true);
        }
    }

    private void PlayReverse()
    {
        if (m_TweenAlpha != null)
        {
            for (int i = 0; i < m_TweenAlpha.Length; i++)
            {
                if (m_TweenAlpha[i] != null)
                {
                    m_TweenAlpha[i].PlayReverse();
                }
            }
        }
        if (m_TweenPosition != null)
        {
            for (int i = 0; i < m_TweenPosition.Length; i++)
            {
                if (m_TweenPosition[i] != null)
                {
                    m_TweenPosition[i].PlayReverse();
                }
            }
        }
    }


    public void OpenFuling()
    {
        if(m_Level2Fuling!=null)
        {
            m_Level2Fuling.gameObject.SetActive(true);
        }
    }

    public void OpenTalisman()
    {
        if(m_Level2Talisman!=null)
        {
            m_Level2Talisman.gameObject.SetActive(true);
        }
    }
    public void OpenStar()
    {
        if(m_Level2Star!=null)
        {
            m_Level2Star.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Level2通用的退出方法
    /// </summary>
    public void Exit()
    {
        if (CollectionLevel2Fuling.Instance != null)
        {
            //如果符灵界面处于打开状态
            if (CollectionLevel2Fuling.Instance.gameObject.activeInHierarchy)
            {
                //为真时，表示在Preview要退出到Level1
                if (CollectionLevel2Fuling.Instance.OnCloseClick())
                {
                    this.PlayReverse();
                    if (m_TitileLabel != null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5563);
                    }
                }
            }
        }
        if (CollectionLevel2Star.Instance != null)
        {
            if (CollectionLevel2Star.Instance.gameObject.activeInHierarchy)
            {
                //为真时，表示在Preview要退出到Level1
                if (CollectionLevel2Star.Instance.OnCloseClick())
                {
                    this.PlayReverse();
                    if (m_TitileLabel != null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5563);
                    }
                }
            }
        }
        if (CollectionLevel2Talisman.Instance != null)
        {
            if (CollectionLevel2Talisman.Instance.gameObject.activeInHierarchy)
            {
                //为真时，表示在Preview要退出到Level1
                if (CollectionLevel2Talisman.Instance.OnCloseClick())
                {
                    this.PlayReverse();
                    if (m_TitileLabel != null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5563);
                    }
                }
            }
        }
    }

    
}
