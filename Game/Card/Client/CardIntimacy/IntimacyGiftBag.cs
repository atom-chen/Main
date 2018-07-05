using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Item;
using Games.Table;
using ProtobufPacket;
using Games;
//********************************************************************
// 描述: 亲密度界面背包
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//
//********************************************************************
public class IntimacyGiftBag : MonoBehaviour {
    public GameObject m_SendObj;//赠送按钮

    public GameObject m_Default;//空空如也
    public IntimacyGiftItem[] m_GiftItem;//礼物Item
    public IntimacyItemDesc m_Desc;//物品描述

    private List<Item> m_GiftList = new List<Item>();//礼物背包List（Model层）
    private IntimacyGiftItem m_CurPitchItem = null;//当前选中的Item
    private bool m_IsPit = true;//是否有物品被选中
    private bool m_IsOver = false;//物品是否被用完了
   
    private static IntimacyGiftBag _Instance;
    public static IntimacyGiftBag Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
    }
    void OnEnable()
    {
        if(m_GiftItem==null && this.transform.childCount>0)
        {
            m_GiftItem = this.transform.GetChild(0).GetComponentsInChildren<IntimacyGiftItem>();
        }
        if (m_Desc != null)
        {
            m_Desc.gameObject.SetActive(false);//默认关闭描述
        }
        UpdateAll();
        //默认选中第一个
        if (m_GiftList != null && m_GiftList.Count > 0)
        {
            OnClickItem(m_GiftItem[0].DataID);
        }
    }
    void OnDisable()
    {
        m_CurPitchItem = null;
        m_IsPit = false;
    }

    /// <summary>
    /// 更新所有界面显示
    /// </summary>
    public void UpdateAll()
    {
        UpdateGiftBagList();
        UpdateGiftBagView();
        //如果上一个没了
        if(m_IsOver)
        {
            //如果还有数据，则默认选到第一个
            if (m_GiftItem != null && m_GiftList.Count >= 1)
            {
                OnClickItem_true(m_GiftItem[0]);
            }
            //如果没有物品了，则把第一个取消勾选
            else
            {
                OnClickItem_false();
            }
            m_IsOver = false;
        }
    }

    //更新礼物背包List（数据层）
    private void UpdateGiftBagList()
    {
        if(GameManager.PlayerDataPool==null)
        {
            return;
        }
        ItemPack totalPack = GameManager.PlayerDataPool.CommonPack;//道具背包
        if(totalPack==null)
        {
            return;
        }
        List<Item> itemList = totalPack.GetList();
        if(itemList==null)
        {
            return;
        }
        m_GiftList.Clear();
        for(int i=0;i<itemList.Count;i++)
        {
            if(itemList[i]==null || IntimacyRoot.Instance==null)
            {
                continue;
            }
            Tab_Item tabItem = TableManager.GetItemByID(itemList[i].DataID, 0);
            if(tabItem==null)
            {
                continue;
            }
            //如果类型正确 且该物品是当前卡牌想要的，则添加到集合
            if(tabItem.ClassID==IntimacyRoot.Instance.GiftClassID && tabItem.SubClassID==IntimacyRoot.Instance.GiftSubClassID && IntimacyRoot.Instance.IsCardCare(tabItem.Id))
            {
                m_GiftList.Add(itemList[i]);
            }
        }
        m_GiftList.Sort(GiftSort);
    }

    //更新显示背包显示
    private void UpdateGiftBagView()
    {
        if(m_GiftList==null || m_GiftList.Count==0)
        {
            if (m_Default!=null)
            {
                m_Default.SetActive(true);//空空如也
                m_SendObj.SetActive(false);
                m_Desc.gameObject.SetActive(false);//关闭描述框
            }
            this.gameObject.SetActive(false);
            return;
        }
        m_SendObj.SetActive(true);
        this.gameObject.SetActive(true);
        m_Desc.gameObject.SetActive(true);//打开描述框
        //从表格取图片信息以及类型信息
        //从内存的Item类取当前信息
        for(int i=0;i<m_GiftItem.Length;i++)
        {
            if(m_GiftItem[i]==null)
            {
                continue;
            }
            if(i>=m_GiftList.Count)
            {
                m_GiftItem[i].gameObject.SetActive(false);
                continue;
            }
            m_GiftItem[i].gameObject.SetActive(true);
            //如果是选中的数据，则打开其选中框，否则关闭
            if(m_CurPitchItem==m_GiftItem[i])
            {
                m_GiftItem[i].PitOn(true);
            }
            else
            {
                m_GiftItem[i].PitOn(false);
            }

            //取出一条数据
            Tab_Item tabItem = TableManager.GetItemByID(m_GiftList[i].DataID, 0);
            if(tabItem!=null)
            {
                m_GiftItem[i].InitGiftItem(tabItem.Icon, m_GiftList[i].StackCount, tabItem.Name, m_GiftList[i].DataID, m_GiftList[i].Guid, tabItem.Tips);
            }

        }
        if(m_Default!=null)
        {
            m_Default.SetActive(false);//关闭空空如也
        }
    }

    //选中/反勾选新的Item
    public void OnClickItem(int ItemDataId)
    {
        for(int i=0;i<m_GiftItem.Length;i++)
        {
            //找到被选中的Item
            if (m_GiftItem[i].DataID==ItemDataId)
            {
                int intimacyAdd = -1;
                if(IntimacyRoot.Instance!=null)
                {
                    intimacyAdd = IntimacyRoot.Instance.GetIntimacyAddWithGift(ItemDataId);//该礼物增加多少亲密度
                }
                
                //如果是勾选(前一个为空 或者前一个不等于当前的)
                if (m_CurPitchItem == null || m_GiftItem[i].Guid != m_CurPitchItem.Guid)
                {
                    //反勾选之前的（如果之前有）
                    if (m_CurPitchItem != null)
                    {
                        m_CurPitchItem.Pitch();
                    }
                    m_IsPit = true;
                    m_CurPitchItem =m_GiftItem[i];
                    m_CurPitchItem.Pitch();                  //勾选现在的
                    UpdateDesc(intimacyAdd);
                }
                //如果是反选
                else
                {
                    //m_IsPit = false;
                    //m_CurPitchItem = null;
                    //UpdateDesc();
                }
                break;
            }
        }
    }

    //勾选新的Item，只关心把新的点亮，不关心旧的
    public void OnClickItem_true(IntimacyGiftItem itemData)
    {
        int intimacyAdd = -1;
        if (IntimacyRoot.Instance != null)
        {
            intimacyAdd = IntimacyRoot.Instance.GetIntimacyAddWithGift(itemData.DataID);//该礼物增加多少亲密度
        }
        m_CurPitchItem = itemData;
        m_IsPit = true;
        m_CurPitchItem.PitOn(true);
        UpdateDesc(intimacyAdd);
    }
    //反勾选之前的Item
    public void OnClickItem_false()
    {
        m_IsPit = false;
        //先取消勾选状态
        if(m_CurPitchItem!=null)
        {
            m_CurPitchItem.PitOn(false);
            m_CurPitchItem = null;
        }
        UpdateDesc();
    }


    //更新界面Desc显示（DESC），在选中的东西变更时调用
    private void UpdateDesc(int intimacyAdd=-1)
    {
        //如果是勾选状态
        if (m_IsPit)
        {
            if (m_Desc != null && m_CurPitchItem!=null)
            {
                if (intimacyAdd != -1)
                {
                    if(m_Desc!=null)
                    {
                        m_Desc.gameObject.SetActive(true);//弹出提示
                        m_Desc.InitDesc(m_CurPitchItem.Name, m_CurPitchItem.Desc, intimacyAdd);
                    }
                }
                //如果因为策划配表失误，没有在表中找到喜好情况
                else
                {
                    if(m_Desc!=null)
                    {
                        m_Desc.gameObject.SetActive(true);
                        m_Desc.InitDesc(m_CurPitchItem.Name, m_CurPitchItem.Desc, 0);
                    }
                }
            }
        }
        //如果是反勾选
        else
        {
            m_CurPitchItem = null;
            if(m_Desc!=null)
            {
                m_Desc.gameObject.SetActive(false);
            }
        }
    }


    //赠送按键的回调
    public void Give()
    {
        if(m_CurPitchItem==null)
        {
            return;
        }
        if(!IntimacyRoot.Instance.IsCanAddIntimacy() && IntimacyRoot.Instance!=null)
        {
            //弹出提示
            Utils.CenterNotice(StrDictionary.GetClientDictionaryString("#{6807}", IntimacyRoot.Instance.card.GetName()));
            return;      
        }
        //如果当前符灵亲密度等级未满
        if(m_CurPitchItem.Count>=1)
        {
            CG_INTIMACY_ADD_PAK pak = new CG_INTIMACY_ADD_PAK();
            pak.data.AddType = (int)INTIMACY_ADD_TYPE.INTIMACY_ADD_GIVE_GIFT;
            if(IntimacyRoot.Instance!=null && IntimacyRoot.Instance.card!=null)
            {
                pak.data.CardGuid = IntimacyRoot.Instance.card.Guid;
            }
            _IntimacyGift gift = new _IntimacyGift();//物品信息
            gift.GiftItemId = m_CurPitchItem.DataID;
            gift.GiftItemGuid = m_CurPitchItem.Guid;
            gift.GiftItemCount = 1;
            pak.data.PayID = gift;
            pak.SendPacket();

            //如果最后一个没有了，则默认让它选到导第一个
            m_CurPitchItem.Count--;
            if(m_CurPitchItem.Count<=0)
            {
                m_IsOver = true;
            }
            else
            {
                m_IsOver = false;
            }

            //展示喜好等级
            if(IntimacyRoot.Instance!=null)
            {
                IntimacyRoot.Instance.ShowLikeDegree(gift.GiftItemId);
            }
        }
    }

    //排序算法，直接按在Gift表的顺序排
    static int GiftSort(Item left,Item right)
    {
        if(left==null || right==null)
        {
            return 0;
        }
        int leftGiftIndex = -1;
        int rightGiftIndex = -1;

        Tab_CardIntimacyGift tGift = null;
 
        if(IntimacyRoot.Instance!=null)
        {
            tGift = IntimacyRoot.Instance.Gift;
        }
        if(tGift!=null)
        {
            for(int i=0;i<tGift.getItemIDCount();i++)
            {
                if (left.DataID== tGift.GetItemIDbyIndex(i))
                {
                    leftGiftIndex = i;
                }
                if(right.DataID==tGift.GetItemIDbyIndex(i))
                {
                    rightGiftIndex = i;
                }
            }
            //如果left在right前面
            if(leftGiftIndex<rightGiftIndex)
            {
                return -1;
            }
            else if(leftGiftIndex>rightGiftIndex)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

}
