using Games.GlobeDefine;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  万象会馆红点工具类
///  zhaoyan
///  20180410
/// </summary>
public class CollectionRedTool {


    public static System.Action sUpdateEvent;

    public static void UpdateRedShow()
    {
        if (sUpdateEvent != null)
        {
            sUpdateEvent();
        }
    }

    public static bool IsCanAward_All()
    {
        if (!GlobeVar._GameConfig.m_IsCollectionOpen)
        {
            return false;
        }
        //遍历表格 找到是否有可完成
        var it = TableManager.GetHandbook().GetEnumerator();
        while (it.MoveNext())
        {
            var tab = it.Current.Value[0];
            bool isOk = IsCanAward_Handbook(tab);
            if (isOk)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsCanAward_Card()
    {
        if(!GlobeVar._GameConfig.m_IsCollectionCardOpen)
        {
            return false;
        }
        //遍历表格 找到是否有可完成
        var it = TableManager.GetHandbook().GetEnumerator();
        while (it.MoveNext())
        {
            var tab = it.Current.Value[0];
            if(tab.Style != (int)COLLECTION_TYPE.COLLECTION_CARD)
            {
                continue;
            }
            bool isOk = IsCanAward_Handbook(tab);
            if (isOk)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsCanAward_Handbook(Tab_Handbook handdbook)
    {
        if (handdbook == null) return false;

        bool isAlreadyReceived = GameManager.PlayerDataPool.CollectionData.IsGetGroupAccess(handdbook.Id); //已领取
        if (isAlreadyReceived)
        {
            return false;
        }
        bool isDone = false;  //能够完成
        if (handdbook.Style == (int)COLLECTION_TYPE.COLLECTION_CARD)
        {
            isDone = InitFulingItem(handdbook);
        }
        else if (handdbook.Style == (int)COLLECTION_TYPE.COLLECTION_STAR)
        {
            isDone = InitStarItem(handdbook);
        }
        else if (handdbook.Style == (int)COLLECTION_TYPE.COLLECTION_TALISMAN)
        {
            isDone = InitTalismanItem(handdbook);
        }
        return isDone;
    }

    /// <summary>
    /// 初始化Bag里的符灵Item
    /// </summary>
    /// <param name="handdbook">一条组合数据</param>
    /// <returns>返回该handbook是否完成</returns>
    private static bool InitFulingItem(Tab_Handbook handbook)
    {
        if (handbook == null) return false;
        bool isCover = true;
        //初始化每一个符灵
        for (int i = 0; i < handbook.getGroupIDCount(); i++)
        {
 
            int id = handbook.GetGroupIDbyIndex(i);
            if (id == GlobeVar.INVALID_ID)
            {
                continue;
            }
            Tab_Card card = TableManager.GetCardByID(id, 0);
            if (card == null)
            {
                continue;
            }
            bool isCardGet = GameManager.PlayerDataPool.CollectionData.IsGetCard(card.Id);
            if (isCardGet == false)
            {
                isCover = false;
            }
        }
        return isCover;
    }
    private static bool InitStarItem(Tab_Handbook handbook)
    {
        if (handbook == null)
        {
            return false;
        }

        bool isCover = true;
        for (int i = 0; i < handbook.getGroupIDCount(); i++)
        {
            
            int id = handbook.GetGroupIDbyIndex(i);
            if (id == GlobeVar.INVALID_ID)
            {
                continue;
            }
            Tab_QuartzClass quartzClass = TableManager.GetQuartzClassByID(id, 0);
            if (quartzClass == null)
            {
                isCover = false;
                continue;
            }
            bool isQuartzClassGet = GameManager.PlayerDataPool.CollectionData.IsQuartzClassGet(quartzClass.Id);//判断该星魂类型是否获得
            //只要有一个class没获取，就认为没获取
            if (isQuartzClassGet == false)
            {
                isCover = false;
            }
        }
        return isCover;
    }
    private static bool InitTalismanItem(Tab_Handbook handbook)
    {
        if (handbook == null)
        {
            return false;
        }
        bool isCover = true;
        for (int i = 0; i < handbook.getGroupIDCount(); i++)
        {
            
            int id = handbook.GetGroupIDbyIndex(i);
            if (id == GlobeVar.INVALID_ID)
            {
                continue;
            }
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            if (talisman == null)
            {
                isCover = false;
                continue;
            }
            bool isTalismanGet = GameManager.PlayerDataPool.CollectionData.IsGetTalisman(talisman.Id);
            if (isTalismanGet == false)
            {
                isCover = false;
            }
        }
        return isCover;
    }


    static public int CardSort(int left, int right)
    {
        Tab_Card leftCard = TableManager.GetCardByID(left, 0);
        Tab_Card rightCard = TableManager.GetCardByID(right, 0);

        if (null == leftCard || null == rightCard || null == GameManager.PlayerDataPool)
        {
            return 0;
        }
        //再按稀有度排
        if (leftCard.Rare > rightCard.Rare)
        {
            return -1;
        }
        else if (leftCard.Rare < rightCard.Rare)
        {
            return 1;
        }
        else
        {
            if (left < right)
            {
                return -1;
            }
            else if (left > right)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    static public int GroupSort(Tab_Handbook leftTab, Tab_Handbook rightTab)
    {

        if (null == leftTab || null == rightTab || null == GameManager.PlayerDataPool)
        {
            return 0;
        }
        //是否可领取
        if (IsCanAward_Handbook(leftTab))
        {
            return -1;
        }
        else if (IsCanAward_Handbook(rightTab))
        {
            return 1;
        }
        else
        {
            //是否已完成
            if (GameManager.PlayerDataPool.CollectionData.CollectionGroupFinishList.AlreadyReceived.Contains(leftTab.Id))
            {
                return 1;
            }
            else if (GameManager.PlayerDataPool.CollectionData.CollectionGroupFinishList.AlreadyReceived.Contains(rightTab.Id))
            {
                return -1;
            }
            else
            {
                if (leftTab .Id> rightTab.Id)
                {
                    return -1;
                }
                else if(leftTab.Id < rightTab.Id)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
