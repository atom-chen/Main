using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//********************************************************************
// 描述: 图鉴一级界面脚本
// 作者: 王必宇
// 创建时间: 2018-02-23
//
//在动画结束后，若是收回动画则令该游戏物体不激活。
//
//********************************************************************
public enum COLLECTION_LEVEL1_CLOSE_TYPE
{
    COLLECTION_LEVEL1_CLOSE_TO_FULING=0,
    COLLECTION_LEVEL1_CLOSE_TO_TALISMAN = 1,
    COLLECTION_LEVEL1_CLOSE_TO_STAR = 2,
}
public class CollectionLevel1View : MonoBehaviour {
    private TweenAlpha m_LevelViewTween;
    private COLLECTION_LEVEL1_CLOSE_TYPE m_CloseType = COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_FULING;//关闭类型 决定要跳到哪个界面

    public UILabel m_TitileLabel = null; // 标题
    public CollectionLevel2View m_Level2;

    void Start()
    {
        if (m_LevelViewTween == null)
        {
            m_LevelViewTween = this.GetComponent<TweenAlpha>();
        }
        m_LevelViewTween.AddOnFinished(new EventDelegate(OnTweenFinish));
    }
    /// <summary>
    /// 三个入口淡入
    /// </summary>
    public void OnEnable()
    {
        if(m_LevelViewTween!=null)
        {
            m_LevelViewTween.PlayForward();//显示
        }
    }
    
    /// <summary>
    /// 播放动画：三个入口渐隐消失
    /// </summary>
    public void Fade(COLLECTION_LEVEL1_CLOSE_TYPE type)
    {
        this.m_CloseType = type;
        if (m_LevelViewTween == null)
        {
            m_LevelViewTween = this.GetComponent<TweenAlpha>();
        }
        if(m_LevelViewTween!=null)
        {
            if (m_LevelViewTween.value > 0)
            {
                m_LevelViewTween.PlayReverse();//隐藏
            }
        }
    }
    /// <summary>
    /// 动画播放结束后的委托：关闭当前界面
    /// </summary>
    private void OnTweenFinish()
    {
        if (m_LevelViewTween.value == 0)
        {
            //打开level2
            if (m_Level2 != null)
            {
                m_Level2.gameObject.SetActive(true);
                //打开星魂界面
                if (m_CloseType == COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_STAR)
                {
                    m_Level2.OpenStar();
                    if(m_TitileLabel!=null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5564);
                    }
                }
                //符灵
                else if (m_CloseType == COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_FULING)
                {
                    m_Level2.OpenFuling();
                    if (m_TitileLabel != null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5565);
                    }
                }
                //物华
                else if (m_CloseType == COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_TALISMAN)
                {
                    m_Level2.OpenTalisman();
                    if (m_TitileLabel != null)
                    {
                        m_TitileLabel.text = StrDictionary.GetDicByID(5566);
                    }
                }
            }
            this.gameObject.SetActive(false);
        }
    }
}
