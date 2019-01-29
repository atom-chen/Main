using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameOrderController : MonoBehaviour
{
    private static GameOrderController m_Instance;
    public static GameOrderController Instance()
    {
        return m_Instance;
    }
    //Top
    public UISprite m_HelpSprite;
    public UILabel m_HelpLabel;
    public UILabel m_PeopleHelpLabel;
    //Center
    public DrawGridItem m_PageThree;
    public DrawGridItem m_PageTwo;
    public DrawGridItem m_PageOne;
    public DrawGridItem m_PageBest;
    public DrawGridItem m_PageFree;
    public GameObject m_PageFreeMain;
    public UIInput m_FreeInput;
    [HideInInspector]
    public DrawGridItem m_CurTage;

    private DRAW_TYPE m_CurDraw;
    private int m_CurCount = 11;
    private bool m_bBeginDraw = false;
    private List<People> luckBoy;
    void Awake()
    {
        m_Instance = this;
    }
    void Start()
    {
        m_PageThree.Init(GameDefine.Total_Three, GameDefine.ThreeEveryDraw,GameDefine.ThreeAlreadyDraw,DRAW_TYPE.THREE);
        m_PageTwo.Init(GameDefine.Total_Two, GameDefine.TwoEveryDraw, GameDefine.TwoAlreadyDraw, DRAW_TYPE.TWO);
        m_PageOne.Init(GameDefine.Total_One, GameDefine.OneEveryDraw, GameDefine.OneAlreadyDraw, DRAW_TYPE.ONE);
        m_PageBest.Init(GameDefine.Total_Best, GameDefine.BestEveryDraw, GameDefine.BestAlreadyDraw, DRAW_TYPE.BEST);
        m_PageFree.Init(-1,-1,0,DRAW_TYPE.FREE);

        m_PageTwo.gameObject.SetActive(false);
        m_PageOne.gameObject.SetActive(false);
        m_PageBest.gameObject.SetActive(false);
        m_PageFreeMain.SetActive(false);
        OnClickPageThree();
    }
    /// <summary>
    /// 开始抽奖后，每物理时间0.05s 随机当前数量个中奖者ID 按下停时停止随机
    /// </summary>
    void FixedUpdate()
    {
        if (m_bBeginDraw && m_CurCount >= 0)
        {
            luckBoy = GameManager.GetLuckBoys(m_CurCount);
            //到当前面板上去展示
            if (m_CurTage == null)
            {
                return;
            }
            m_CurTage.ShowName(luckBoy);
        }
    }
    public void OnClickOn()
    {
        //如果当前在抽奖 那就结束
        if(m_bBeginDraw)
        {
            OnClickStop();
        }
        //如果当前没有在抽奖 那就开始
        else
        {
            OnClickBegin();
        }
    }
    /// <summary>
    /// 点击开始按钮
    /// </summary>
    public void OnClickBegin()
    {
        if (m_bBeginDraw == true)
        {
            return;
        }
        if (m_CurTage == null)
        {
            return;
        }
        //做修正
        if (m_CurTage == m_PageFree)
        {
            //点开始，如果还未完成输入，则将输入值进行转换
            if (!string.IsNullOrEmpty(m_FreeInput.value))
            {
                OnCommitFreeCount();
            }
            m_CurCount = m_CurTage.EveryDrawCount;
        }
        else
        {
            m_CurCount = m_CurTage.GetCount();
        }
        if (m_CurTage.GetResidueNum() <= 0 && m_CurTage != m_PageFree)
        {
            //不允许操作
            return;
        }
        if(GameManager.GetDrawRange() <=0)
        {
            return;
        }
        m_bBeginDraw = true;
    }
    /// <summary>
    /// 点击结束按钮
    /// </summary>
    public void OnClickStop()
    {
        if (m_bBeginDraw == false)
        {
            return;
        }
        m_bBeginDraw = false;
        //更新Help显示
        m_CurTage.DrawEnd();
        GameManager.Stop(luckBoy, m_CurTage.mType);
        UpdateView();
    }
    public void OnClickPageThree()
    {
        if (m_CurTage == m_PageThree || m_bBeginDraw)
        {
            return;
        }
        Leave();
        m_PageThree.gameObject.SetActive(true);
        m_CurTage = m_PageThree;
        m_CurCount = m_CurTage.GetCount();
        SwitchMarkView.Instance().SwitchMark(0);
        UpdateView();
    }
    public void OnClickPageTwo()
    {
        if (m_CurTage == m_PageTwo || m_bBeginDraw)
        {
            return;
        }
        Leave();
        m_PageTwo.gameObject.SetActive(true);
        m_CurTage = m_PageTwo;
        m_CurCount = m_CurTage.GetCount();
        SwitchMarkView.Instance().SwitchMark(1);
        UpdateView();
    }
    public void OnClickPageOne()
    {
        if (m_CurTage == m_PageOne || m_bBeginDraw)
        {
            return;
        }
        Leave();
        m_PageOne.gameObject.SetActive(true);
        m_CurTage = m_PageOne;
        m_CurCount = m_CurTage.GetCount();
        SwitchMarkView.Instance().SwitchMark(2);
        UpdateView();
    }
    public void OnClickPageBest()
    {
        if (m_CurTage == m_PageBest || m_bBeginDraw)
        {
            return;
        }
        Leave();
        m_PageBest.gameObject.SetActive(true);
        m_CurTage = m_PageBest;
        m_CurCount = m_CurTage.GetCount();
        SwitchMarkView.Instance().SwitchMark(3);
        UpdateView();
    }
    public void OnClickTitle()
    {
        if (m_CurTage == m_PageFree || m_bBeginDraw)
        {
            return;
        }
        if (m_PageFree == null)
        {
            m_PageFree = m_PageFreeMain.transform.GetChild(0).GetComponent<DrawGridItem>();
        }
        Leave();
        m_PageFreeMain.SetActive(true);
        m_CurTage = m_PageFree;
        SwitchMarkView.Instance().SwitchMark(-1);
        UpdateView();
    }

    public void OnCommitFreeCount()
    {
        m_bBeginDraw = false;
        int count = 0;
        int.TryParse(m_FreeInput.value, out count);
        m_PageFree.SetCount(count, count);
        m_FreeInput.value = "";
    }

    void Leave()
    {
        m_bBeginDraw = false;
        if (m_CurTage == m_PageFree)
        {
            m_PageFreeMain.gameObject.SetActive(false);
        } 
        else
        {
            if (m_CurTage != null)
            {
                m_CurTage.gameObject.SetActive(false);
            }
        }
    }

    public GameObject m_HelpObj;
    /// <summary>
    /// 切换奖励级别->更新help数字以及奖励级别sprite
    /// </summary>
    public void UpdateView()
    {
        if(m_CurTage == m_PageFree)
        {
            m_HelpObj.SetActive(false);
        }
        else
        {
            m_HelpObj.SetActive(true);
        }
        m_HelpLabel.text =string.Format("剩余次数：{0}",m_CurTage.GetResidue());
        m_PeopleHelpLabel.text = string.Format("剩余人数：{0}", GameManager.GetDrawRange());
    }

}















