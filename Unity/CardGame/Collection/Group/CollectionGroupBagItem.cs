using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;
using Games.Item;
//********************************************************************
// 描述: 组合界面 7个BagItem之一。其上是Bag
// 作者: wangbiyu
// 创建时间: 2018-3-1
//********************************************************************
public class CollectionGroupBagItem : MonoBehaviour {
    public UILabel m_Title;//二级标题

    public UIGrid m_GroupGrid;//所需物品的网格

    public CollcetionGroupBagGoodsListItem[] m_BagGoodsListItem;
    private int id = -1;

    //3个状态下的游戏物体
    public GameObject m_AcceptButton;
    public GameObject m_CannotAcceptButton;
    public GameObject m_GotButton;
    
    void OnEnable()
    {
        if(m_BagGoodsListItem==null)
        {
            m_BagGoodsListItem = this.GetComponentsInChildren<CollcetionGroupBagGoodsListItem>();
        }
    }
/// <summary>
/// 初始化Bag下的一个Item
/// </summary>
/// <param name="handdbook"></param>
/// <returns>返回该Item是否已经领取过奖励</returns>
    public bool InitItem(Tab_Handbook handdbook)
    {
        if(m_Title!=null)
        {
            m_Title.text = handdbook.Group2Name;  
        }
        bool isAlreadyReceived = GameManager.PlayerDataPool.IsGroupAccess(handdbook.Id); //已领取
        bool isDone = false;  //能够完成
        //更新奖励图标
        if (handdbook.Style == (int)COLLECTIONGROUPTYPE.COLLECTIONGROUPTYPE_FULING)
        {
            isDone = InitFulingItem(handdbook);
        } 
        else if (handdbook.Style == (int)COLLECTIONGROUPTYPE.COLLECTIONGROUPTYPE_STAR)
        {
            isDone = InitStarItem(handdbook);
        } 
        else if (handdbook.Style == (int)COLLECTIONGROUPTYPE.COLLECTIONGROUPTYPE_TALISMAN)
        {
            isDone = InitTalismanItem(handdbook);
        }
        //如果已经做完了
        if (isAlreadyReceived)
        {
            m_CannotAcceptButton.SetActive(false);
            m_GotButton.SetActive(true);
            m_AcceptButton.SetActive(false);
        }
        //如果没做完，且可以完成
        else if (isDone)
        {
            m_CannotAcceptButton.SetActive(false);
            m_GotButton.SetActive(false);
            m_AcceptButton.SetActive(true);
        }
        //如果没做完，且不能完成
        else
        {
            m_CannotAcceptButton.SetActive(true);
            m_GotButton.SetActive(false);
            m_AcceptButton.SetActive(false);
        }
        this.id = handdbook.Id;
        return isAlreadyReceived;
    }
    /// <summary>
    /// 初始化Bag里的符灵Item
    /// </summary>
    /// <param name="handdbook">一条组合数据</param>
    /// <returns>返回该handbook是否完成</returns>
    private bool InitFulingItem(Tab_Handbook handdbook)
    {
        bool isCover=true;
        //初始化每一个符灵
        for (int i = 0; i < m_BagGoodsListItem.Length; i++)
        {
            //越界
            if (i >= handdbook.getGroupIDCount())
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            int id=handdbook.GetGroupIDbyIndex(i);
            if(id==-1)
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            m_BagGoodsListItem[i].gameObject.SetActive(true);
            Tab_Card card = TableManager.GetCardByID(id, 0);
            if(card==null)
            {
                continue;
            }
            bool isCardGet = GameManager.PlayerDataPool.IsCardGet(card.Id);
            if(isCardGet==false)
            {
                isCover = false;
            }
            m_BagGoodsListItem[i].InitItem(card.GetLittleIconbyIndex(0), isCardGet,(CARD_RARE)card.Rare);
        }
        return isCover;
    }
    private bool InitStarItem(Tab_Handbook handdbook)
    {
        bool isCover = true;
        //初始化每一个符灵
        for (int i = 0; i < m_BagGoodsListItem.Length; i++)
        {
            //越界
            if (i >= handdbook.getGroupIDCount())
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            int id = handdbook.GetGroupIDbyIndex(i);
            if (id == -1)
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            m_BagGoodsListItem[i].gameObject.SetActive(true);
            Tab_QuartzClass quartzClass = TableManager.GetQuartzClassByID(id, 0);
            if (quartzClass == null)
            {
                isCover = false;
                continue;
            }
            bool isQuartzClassGet = GameManager.PlayerDataPool.IsQuartzClassGet(quartzClass.Id);//判断该星魂类型是否获得
            //只要有一个class没获取，就认为没获取
            if (isQuartzClassGet == false)
            {
                isCover = false;
            }
            m_BagGoodsListItem[i].InitItem(TableManager.GetQuartzByID((quartzClass.Id) * 3 - 1, 0).Icon, isQuartzClassGet, CARD_RARE.INVALID);
        }
        return isCover;
    }
    private bool InitTalismanItem(Tab_Handbook handdbook)
    {
        bool isCover = true;
        //初始化每一个符灵
        for (int i = 0; i < m_BagGoodsListItem.Length; i++)
        {
            //越界
            if (i >= handdbook.getGroupIDCount())
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            int id = handdbook.GetGroupIDbyIndex(i);
            if (id == -1)
            {
                //不激活游戏物体
                m_BagGoodsListItem[i].gameObject.SetActive(false);
                continue;
            }
            m_BagGoodsListItem[i].gameObject.SetActive(true);
            Tab_Talisman talisman = TableManager.GetTalismanByID(id, 0);
            if (talisman == null)
            {
                isCover = false;
                continue;
            }
            bool isTalismanGet = GameManager.PlayerDataPool.IsTalismanGet(talisman.Id);
            if (isTalismanGet == false)
            {
                isCover = false;
            }
            //拿到它的外观方案ID
            Tab_TalismanVisual tabTalismanVisual = TableManager.GetTalismanVisualByID(talisman.VisualId,0);
            if(tabTalismanVisual!=null)
            {
                m_BagGoodsListItem[i].InitItem(tabTalismanVisual.Icon, isTalismanGet,(CARD_RARE) talisman.Rare);
            }
        }
        return isCover;
    }


    //领奖点击事件，发一个CG包
    public void OnClickReceiveAward()
    {
        CG_CollectionGroup_ReceiveAward_PAK pak = new CG_CollectionGroup_ReceiveAward_PAK();
        pak.data.GroupId = id;
        pak.SendPacket();
    }

    //已经领取过奖励的
    public void OnClickGotButton()
    {
        Utils.CenterNotice(6718);
    }

    //点击还不能领取的组合，预览所有物品
    public void OnClickCannotAccept()
    {
        List<int> Items = new List<int>();
        List<int> Counts = new List<int>();
        //把所有奖励物品的ID添加到集合
        Tab_Handbook tHandBook=TableManager.GetHandbookByID(this.id,0);
        if(tHandBook==null)
        {
            return;
        }
        for (int i = 0; i < tHandBook.getAwardIDCount() && i<tHandBook.getAwardCountCount(); i++)
        {
            if (tHandBook.GetAwardIDbyIndex(i) != -1)
            {
                Items.Add(tHandBook.GetAwardIDbyIndex(i));
                Counts.Add(tHandBook.GetAwardCountbyIndex(i));
            }

        }
        ItemBagTipsController.Show(Items,Counts);
    }


}

