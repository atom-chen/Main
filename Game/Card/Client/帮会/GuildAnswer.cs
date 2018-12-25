using ProtobufPacket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;
using System;
using Games;
using Games.Table;

public partial class Guild
{
    private _GuildAnswer m_Data = new _GuildAnswer();
    private string m_FirstRightPlayerName = "";
    public bool mIsHaveNewQuestion = false;

    private const float AnswerValidTime = 5.0f;
    public int QuestionIdx
    {
        get 
        { 
            if(m_Data != null && m_Data.has_questionIndex)
            {
                return m_Data.questionIndex + 1;
            }
            return GlobeVar.INVALID_ID;
        }
    }

    public int QuestionId
    {
        get
        {
            if (m_Data != null && m_Data.has_questionId)
            {
                return m_Data.questionId;
            }
            return GlobeVar.INVALID_ID;
        }
    }

    public long AnswerDeadLine
    {
        get
        {
            if (m_Data != null && m_Data.has_deadline)
            {
                return m_Data.deadline;
            }
            return GlobeVar.INVALID_ID;
        }
    }

    public string FirstRightPlayer
    {
        get
        {
            return m_FirstRightPlayerName;
        }
    }
    public void ReceivePacket(GC_GUILD_ANSWER pak)
    {
        m_Data.questionId = pak.info.questionId;
        m_Data.questionIndex = pak.info.questionIndex;
        m_Data.deadline = pak.info.deadline;
        m_FirstRightPlayerName = "";
        //若聊天界面正在打开 则不记录红点
        mIsHaveNewQuestion = true;
        CallUI();
    }
    public void ReceivePacket(GC_GUILD_ANSWER_OVER pak)
    {
        m_Data = new _GuildAnswer();
        m_FirstRightPlayerName = "";
        mIsHaveNewQuestion = false;
        CallUI();
    }
    public void ReceivePacket(GC_GUILD_SYNC_ANSWER pak)
    {
        m_Data.questionId = pak.info.questionId;
        m_Data.questionIndex = pak.info.questionIndex;
        m_Data.deadline = pak.info.deadline;
        m_FirstRightPlayerName = pak.playerName;
        if(IsInAnswer())
        {
            mIsHaveNewQuestion = true;
        }
        CallUI();
    }
    //自己回答正确 播放特效等等
    public void ReceivePacket(GC_GUILD_ANSWER_RIGHT_RET pak)
    {

    }
    public void ReceivePacket(GC_GUILC_ANSWER_HAS_RIGHT pak)
    {
        if(m_Data!=null && m_Data.questionIndex == pak.questionIndex)
        {
            m_FirstRightPlayerName = pak.playerName;
        }
        CallUI();
    }

    public bool IsInAnswer()
    {
        //防止gameconfig可能导致的数据混乱：超时超过5秒未收到同步新问题的包 认为当前不处于答题状态
        return m_Data != null && m_Data.questionId > GlobeVar.INVALID_ID && m_Data.questionIndex > GlobeVar.INVALID_ID && m_Data.deadline != GlobeVar.INVALID_ID
            && (GameManager.ServerAnsiTime - m_Data.deadline) < AnswerValidTime;
    }

    void CallUI()
    {
        if(SocialController.Instance()!=null)
        {
            SocialController.Instance().OnAnswerUpdate();
        }
    }

    public bool HasRightAnswer()
    {
        return !string.IsNullOrEmpty(m_FirstRightPlayerName);
    }

    public bool IsChatRedPoint()
    {
        return IsInAnswer() && mIsHaveNewQuestion;
    }

    public static bool IsInGuildAnswerOpenTime(List<int> openDayList)
    {
        //读表判断哪些天有开启
        if (openDayList == null)
        {
            openDayList = new List<int>();
        }
        else
        {
            openDayList.Clear();
        }
        
        foreach (var temp in TableManager.GetGuildAnswerClass().Values)
        {
            Tab_GuildAnswerClass tClass = temp[0];
            if (tClass == null)
            {
                continue;
            }

            if (!openDayList.Contains(0) && tClass.SundayOpen == 1)
            {
                openDayList.Add(0);
            }
            if (!openDayList.Contains(1) && tClass.MondayOpen == 1)
            {
                openDayList.Add(1);
            }
            if (!openDayList.Contains(2) && tClass.TuesdayOpen == 1)
            {
                openDayList.Add(2);
            }
            if (!openDayList.Contains(3) && tClass.WednesdayOpen == 1)
            {
                openDayList.Add(3);
            }
            if (!openDayList.Contains(4) && tClass.ThursdayOpen == 1)
            {
                openDayList.Add(4);
            }
            if (!openDayList.Contains(5) && tClass.FridayOpen == 1)
            {
                openDayList.Add(5);
            }
            if (!openDayList.Contains(6) && tClass.SaturdayOpen == 1)
            {
                openDayList.Add(6);
            }
        }
        openDayList.Sort();

        //判断当前是否开启
        bool isOpen = GameManager.PlayerDataPool.GuildData.IsInAnswer();
        DateTime nowTime = Utils.GetServerDateTime();

        //判断星期几
        if (openDayList.Contains((int)nowTime.DayOfWeek))
        {
            //开启时间
            DateTime openTime = new DateTime();
            openTime = openTime.AddYears(nowTime.Year - 1);
            openTime = openTime.AddMonths(nowTime.Month - 1);
            openTime = openTime.AddDays(nowTime.Day - 1);
            openTime = openTime.AddHours(GlobeVar._GameConfig.m_GuildAnswerHour);
            openTime = openTime.AddSeconds(GlobeVar._GameConfig.m_GuildAnswerMin * 60 + GlobeVar._GameConfig.m_GuildAnswerDelta);

            //关闭时间
            DateTime closeTime = new DateTime();
            closeTime = closeTime.AddYears(nowTime.Year - 1);
            closeTime = closeTime.AddMonths(nowTime.Month - 1);
            closeTime = closeTime.AddDays(nowTime.Day - 1);
            closeTime = closeTime.AddHours(GlobeVar._GameConfig.m_GuildAnswerHour);
            closeTime = closeTime.AddSeconds(GlobeVar._GameConfig.m_GuildAnswerMin * 60 + GlobeVar._GameConfig.m_GuildAnswerDelta +
                GlobeVar._GameConfig.m_GuildAnswerWaitTime * GlobeVar._GameConfig.m_GuildAnswerCount);
            
            //判断是否在时间内
            if (openTime <= nowTime && nowTime <= closeTime)
            {
                isOpen = true;
            }
        }

        return isOpen;
    }
}
