/********************************************************************************
 *	文件名：	RollNoticeManager.cs
 *	全路径：	\Script\GlobalSystem\Manager\RollNoticeManager.cs
 *	创建人：	李嘉
 *	创建时间：2017-05-22
 *
 *	功能说明：游戏滚屏公告管理器
 *	         内部队列维护游戏内所有的滚屏公告
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtobufPacket;
using Games.GlobeDefine;

//具体某条滚屏公告类
public class RollNotice
{
    public RollNotice(GC_ROLLING_NOTICE packet)
    {
        if (null != packet)
        {
            m_szContent = packet.notice;
            m_IsNormalNotice = true;
        }
    }

    public RollNotice(GC_NEW_MARQUEE packet)
    {
        if (null != packet)
        {
            m_szContent = packet.Content;
            m_IsNormalNotice = false;
        }
    }

    private string m_szContent = "";
    public string Content
    {
        get { return m_szContent; }
        set { m_szContent = value; }
    }
    
    private float m_fCreateTime = 0.0f;
    public float CreateTime
    {
        get { return m_fCreateTime; }
        set { m_fCreateTime = value; }
    }

    private float m_fEndTime = 0.0f;
    public float EndTime
    {
        get { return m_fEndTime; }
        set { m_fEndTime = value; }
    }

    public bool isValid()
    {
        if (m_szContent.Length <= 0)
        {
            return false;
        }

        return true;
    }

    public bool IsNormalNotice
    {
        get
        {
            return m_IsNormalNotice;
        }
    }

    public bool IsMarquee { get { return !IsNormalNotice; } }

    private bool m_IsNormalNotice = false;
}

public class RollNoticeManager : MonoBehaviour
{
    private Queue<RollNotice>  m_RollingNoticeQueue = new Queue<RollNotice>();
    private Queue<RollNotice> m_MarqueeQueue = new Queue<RollNotice>();
    private RollNotice m_ShowingNotice = null;

    public void Add(GC_ROLLING_NOTICE packet)
    {
        //判断是否已经到上限
        if (m_RollingNoticeQueue.Count >= GlobeVar.ROLLING_NOTICE_COUNT)
        {
            return;
        }

        if (null != packet)
        {
            RollNotice _rm = new RollNotice(packet);
            if (null != _rm && _rm.isValid())
            {
                m_RollingNoticeQueue.Enqueue(_rm);
            }
        }
    }

    public void Add(GC_NEW_MARQUEE packet)
    {
        //判断是否已经到上限
        if (m_MarqueeQueue.Count >= GlobeVar.ROLLING_NOTICE_COUNT)
        {
            return;
        }

        if (null != packet)
        {
            RollNotice _rm = new RollNotice(packet);
            if (null != _rm && _rm.isValid())
            {
                m_MarqueeQueue.Enqueue(_rm);
            }
        }
    }

    public void Clear()
    {
        m_RollingNoticeQueue.Clear();
        m_MarqueeQueue.Clear();
    }

    //UI显示新的信息
    public void ShowNotice(RollNotice rollNotice)
    {
        if (null == rollNotice)
        {
            return;
        }

        if (null == RollNoticeController.GetInstance())
        {
            UIManager.ShowUI(UIInfo.RollNotice, onRollNoticeShowOver);
        }
        else
        {
            RollNoticeController.GetInstance().ShowNotice(StrDictionary.GetServerDictionaryFormatString(rollNotice.Content));
        }
    }

    public void ShowMarquee(RollNotice rollNotice)
    {
        if (null == rollNotice)
        {
            return;
        }

        if (null == RollNoticeController.GetInstance())
        {
            UIManager.ShowUI(UIInfo.RollNotice, onMarqueeShowOver);
        }
        else
        {
            RollNoticeController.GetInstance().ShowMarquee(StrDictionary.GetServerDictionaryFormatString(rollNotice.Content));
        }
    }

    private void onRollNoticeShowOver(bool bSuccess, object param)
    {
        if (bSuccess)
        {
            if (null != RollNoticeController.GetInstance() && null != m_ShowingNotice)
            {
                RollNoticeController.GetInstance().ShowNotice(StrDictionary.GetServerDictionaryFormatString(m_ShowingNotice.Content));
            }
        }
    }

    private void onMarqueeShowOver(bool bSuccess, object param)
    {
        if (bSuccess)
        {
            if (null != RollNoticeController.GetInstance() && null != m_ShowingNotice)
            {
                RollNoticeController.GetInstance().ShowMarquee(StrDictionary.GetServerDictionaryFormatString(m_ShowingNotice.Content));
            }
        }
    }

    //关闭UI显示信息
    public void HideNotice()
    {
        if (null != RollNoticeController.GetInstance())
        {
            RollNoticeController.GetInstance().HideNotice();
        }
    }

    //将当前最前面的一条弹出，并且开始下一条
    public void DequeueNotice()
    {
        m_ShowingNotice = PopNextNotice();
        if (m_ShowingNotice != null)
        {
            m_ShowingNotice.CreateTime = Time.time;

            if (m_ShowingNotice.IsNormalNotice)
            {
                m_ShowingNotice.EndTime = Time.time + GlobeVar.ROLLING_NOTICE_MAXTIME;
                ShowNotice(m_ShowingNotice);
            }
            else
            {
                // 系统公告可能比较长，需要按长度缩放一下显示时间
                string content = StrDictionary.GetServerDictionaryFormatString(m_ShowingNotice.Content);
                m_ShowingNotice.EndTime = Time.time + content.Length / 2.0f;
                ShowMarquee(m_ShowingNotice);
            }
        }
        else
        {
            HideNotice();
        }
    }
    
    private int GetLeftNoticeCount()
    {
        return m_RollingNoticeQueue.Count + m_MarqueeQueue.Count;
    }

    private RollNotice PopNextNotice()
    {
        if (m_MarqueeQueue.Count > 0)
        {
            return m_MarqueeQueue.Dequeue();
        }
        else if (m_RollingNoticeQueue.Count > 0)
        {
            return m_RollingNoticeQueue.Dequeue();
        }
        else
        {
            return null;
        }
    }

    void FixedUpdate()
    {
        if (null != m_ShowingNotice)
        {
            // 系统公告 或者 最后一条普通公告显示最长时间
            if (m_ShowingNotice.IsMarquee || GetLeftNoticeCount() == 0)
            {
                if (/*(Time.time - m_ShowingNotice.CreateTime >= GlobeVar.ROLLING_NOTICE_MAXTIME) &&*/ Time.time >= m_ShowingNotice.EndTime)
                {
                    DequeueNotice();
                }
            }
            else
            {
                if (Time.time - m_ShowingNotice.CreateTime >= GlobeVar.ROLLING_NOTICE_MINTIME)
                {
                    DequeueNotice();
                }
            }
        }
        else if (GetLeftNoticeCount() > 0)
        {
            DequeueNotice();
        }
    }   
}
    