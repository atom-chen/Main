using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games.GlobeDefine;

public class GuildAnswerController : MonoBehaviour 
{
    public UILabel m_QAIdxLabel;          //第几题
    public UILabel m_QAContentLabel;      //题目
    public UILabel m_FirstRightLabel;     //第一个正确的玩家
    public UILabel m_RightAnswerLabel;      //正确答案
    public UILabel m_CountDownLabel;       //剩余时间
    //打开该界面 消除红点标记
    void OnEnable()
    {
        StartCoroutine(CountDown());
        Refresh();
        if (GameManager.PlayerDataPool == null) return;
        GameManager.PlayerDataPool.GuildData.mIsHaveNewQuestion = false;
    }
    public void Refresh()
    {
        if (GameManager.PlayerDataPool == null) return;
        if(!GameManager.PlayerDataPool.GuildData.IsInAnswer())
        {
            this.gameObject.SetActive(false);
            return;
        }
        Tab_GuildAnswerQuestion tQuestion = TableManager.GetGuildAnswerQuestionByID(GameManager.PlayerDataPool.GuildData.QuestionId, 0);
        if(tQuestion == null)
        {
            return;
        }
        Tab_GuildAnswerClass tClass = TableManager.GetGuildAnswerClassByID(tQuestion.ClassId, 0);
        if (tClass == null)
        {
            return;
        }
        m_QAIdxLabel.text = StrDictionary.GetDicByID(8556, GameManager.PlayerDataPool.GuildData.QuestionIdx);
        m_QAContentLabel.text = StrDictionary.GetDicByID(8557, tClass.Name, tQuestion.Content);
        if (GameManager.PlayerDataPool.GuildData.HasRightAnswer())
        {
            m_RightAnswerLabel.text = StrDictionary.GetDicByID(8558, tQuestion.KeyWord);
            m_FirstRightLabel.text = StrDictionary.GetDicByID(8559, GameManager.PlayerDataPool.GuildData.FirstRightPlayer);
        }
        else
        {
            m_RightAnswerLabel.text = StrDictionary.GetDicByID(8558, "");
            m_FirstRightLabel.text = StrDictionary.GetDicByID(8559, "");
        }
        //如果该界面打开 则表示已阅 取消红点
        if(this.gameObject.activeInHierarchy)
        {
            GameManager.PlayerDataPool.GuildData.mIsHaveNewQuestion = false;
        }
    }

    //刷新冷却时间
    IEnumerator CountDown()
    {
        //获取一下时间
        if (GameManager.PlayerDataPool == null || !GameManager.PlayerDataPool.GuildData.IsInAnswer())
        {
            yield break;
        }
        while(true)
        {
            long LeftSecond = GameManager.PlayerDataPool.GuildData.AnswerDeadLine - GameManager.ServerAnsiTime;
            m_CountDownLabel.text = StrDictionary.GetDicByID(8560, (LeftSecond<0?0:LeftSecond).ToString());
            yield return new WaitForSeconds(0.2f);
        }

    }
}
