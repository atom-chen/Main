using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameOrderView : MonoBehaviour
{
    private static GameOrderView m_Instance;
    public static GameOrderView Instance()
    {
        return m_Instance;
    }
    public UILabel m_Help;
    public UILabel m_Title;
    public DrawGridController m_PageThree;
    public DrawGridController m_PageTwo;
    public DrawGridController m_PageOne;
    public DrawGridController m_PageBest;
    public DrawGridController m_PageFree;
    public GameObject m_PageFreeMain;
    public UILabel m_FreeCommitLabel;


    private int m_CurCount = 8;
    public DrawGridController m_CurTage;
    private bool m_bBeginDraw = false;
    private List<People> luckBoy;
    void Awake()
    {
        m_Instance = this;
    }
    void Start()
    {
        m_CurTage = m_PageThree;
        m_CurCount = m_CurTage.GetCount();
        m_PageThree.TotalDraw = 88;
        m_PageTwo.TotalDraw = 50;
        m_PageOne.TotalDraw = 30;
        m_PageBest.TotalDraw = 20;
        m_Title.text = "三等奖";
        UpdateView();
    }
    /// <summary>
    /// 开始抽奖后，每物理时间0.05s 随机当前数量个中奖者ID 按下停时停止随机
    /// </summary>
    void FixedUpdate()
    {
        if (m_bBeginDraw && m_CurCount >= 0)
        {
            luckBoy = GameOrderController.Instance().GetLuckBoys(m_CurCount);
            //到当前面板上去展示
            if (m_CurTage == null)
            {
                return;
            }
            m_CurTage.ShowName(luckBoy);
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
        if (m_CurTage != null)
        {
            m_CurCount = m_CurTage.GetCount();
        } else
        {
            m_CurCount = 8;
        }
        if (m_CurTage.GetResidueNum() <= 0 && m_CurTage != m_PageFree)
        {
            //不允许操作
            return;
        }
        m_bBeginDraw = true;
        GameOrderController.Instance().Begin();
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
        GameOrderController.Instance().Stop(luckBoy);
    }
    public void OnClickPageThree()
    {
        if (m_CurTage == m_PageThree)
        {
            return;
        }
        Leave();
        m_PageThree.gameObject.SetActive(true);
        m_CurTage = m_PageThree;
        UpdateView();
    }
    public void OnClickPageTwo()
    {
        if (m_CurTage == m_PageTwo)
        {
            return;
        }
        Leave();
        m_PageTwo.gameObject.SetActive(true);
        m_CurTage = m_PageTwo;
        UpdateView();
    }
    public void OnClickPageOne()
    {
        if (m_CurTage == m_PageOne)
        {
            return;
        }
        Leave();
        m_PageOne.gameObject.SetActive(true);
        m_CurTage = m_PageOne;
        UpdateView();
    }
    public void OnClickPageBest()
    {
        if (m_CurTage == m_PageBest)
        {
            return;
        }
        Leave();
        m_PageBest.gameObject.SetActive(true);
        m_CurTage = m_PageBest;
        UpdateView();
    }
    public void OnClickTitle()
    {
        if (m_PageFree == null)
        {
            m_PageFree = m_PageFreeMain.transform.GetChild(0).GetComponent<DrawGridController>();
        }
        Leave();
        m_PageFreeMain.gameObject.SetActive(true);
        m_CurTage = m_PageFree;
        UpdateView();
    }

    public void OnCommitFreeCount()
    {
        m_bBeginDraw = false;
        //获取输入
        if (m_FreeCommitLabel == null)
        {
            m_PageFreeMain.transform.GetChild(1).GetComponentInChildren<UILabel>();
        }
        if (m_FreeCommitLabel != null)
        {
            int count = Convert.ToInt32(m_FreeCommitLabel.text);
            GameOrderController.Instance().OnFreeUpdateCount(count);
            m_FreeCommitLabel.text = "";
        }


    }

    void Leave()
    {
        m_bBeginDraw = false;
        if (m_CurTage == m_PageFree)
        {
            m_PageFreeMain.gameObject.SetActive(false);
        } else
        {
            m_CurTage.gameObject.SetActive(false);
        }
    }

    public void UpdateView()
    {
        m_CurCount = m_CurTage.GetCount();
        m_Help.text = m_CurTage.GetResidue();
        m_Title.text = m_CurTage.m_Title;
    }

}















