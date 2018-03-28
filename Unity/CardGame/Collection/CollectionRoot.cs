using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games;
using Games.Table;
using ProtobufPacket;
using System.Collections.Generic;
using Games.Item;

public class CollectionRoot : MonoBehaviour {
    private const bool m_IsOpenFulingCollection=true;
    private const bool m_IsOpenStarCollection = false;
    private const bool m_IsOpenTalismanCollection = false;

    static public string FulingShadowPath = "Texture/FuLing/Card_Shadow/";
    static public string StarPicPath = "Texture/T_StarSoul/";
    static public string StarShadowPath = FulingShadowPath;
    static public string TalismanPortraitPath = "Texture/Common/";
    static public string TalismanShadowPath = FulingShadowPath;

    public UILabel m_TitileLabel = null; // 标题
    public CollectionLevel1View m_Level1 = null;//一级界面
    public CollectionLevel2View m_Level2 = null; // 二级界面

    private static CollectionRoot _Instance;

    public static CollectionRoot Instance
    {
        get
        {
            return _Instance;
        }
    }
    private void Awake()
    {
        _Instance = this;
        m_TitileLabel.text = StrDictionary.GetDicByID(5563);
        // 打开界面时向服务器请求一下最新数据
        CG_REQ_COLLECTION_PAK pak = new CG_REQ_COLLECTION_PAK();
        if (null != pak)
        {
            pak.SendPacket();
        }
        //向服务器同步GroupList信息
        CG_SYS_COLLECTIONGROUPLIST_PAK pak2 = new CG_SYS_COLLECTIONGROUPLIST_PAK();
        if(pak2!=null)
        {
            pak2.SendPacket();
        }
        
    }
    void OnEnable()
    {
        if(m_Level1!=null)
        {
            m_Level1.gameObject.SetActive(true);
        }
        if(m_Level2!=null)
        {
            m_Level2.gameObject.SetActive(false);
        }
    }

    public void OnCloseClick()
    {
        if (m_Level1!=null && m_Level1.gameObject.activeInHierarchy)
        {
            UIManager.CloseUI(UIInfo.Collection); // 只有一级界面了，关闭
            return;
        }
        //走二级界面的关闭逻辑
        else
        {
            if (m_Level2 != null)
            {
                m_Level2.Exit();
            }
        }
    }
    // 打开符灵二级界面
    public void OnOpenFulingSubWindow()
    {
        if(m_Level1==null)
        {
            return;
        }
        if (m_IsOpenFulingCollection)
        {
            m_Level1.Fade(COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_FULING);
        }
        else
        {
            //提示
            Utils.CenterNotice(6719);
        }
    }

    // 打开星魂二级界面
    public void OnOpenStarSubWindow()
    {
        if(m_Level1==null)
        {
            return;
        }
        if(m_IsOpenStarCollection)
        {
            m_Level1.Fade(COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_STAR);
        }
        else
        {
            //提示
            Utils.CenterNotice(6719);
        }
    }

    // 打开法宝二级界面
    public void OnOpenTalismanSubWindow()
    {
        if (m_Level1 == null)
        {
            return;
        }
        if(m_IsOpenTalismanCollection)
        {
            m_Level1.Fade(COLLECTION_LEVEL1_CLOSE_TYPE.COLLECTION_LEVEL1_CLOSE_TO_TALISMAN);
        }
        else
        {
            //提示
            Utils.CenterNotice(6719);
        }
    }

    



    //展示详情
    public void ShowCardDetail(int cardid)
    {
        if (CollectionLevel2Fuling.Instance!=null)
        {
            CollectionLevel2Fuling.Instance.ShowCardDetail(cardid);
        }
        
    }

    public void ShowStarDetail(int starid)
    {
        if (CollectionLevel2Star.Instance!=null)
        {
            CollectionLevel2Star.Instance.ShowStarDetail(starid);
        }
        
    }

    public void ShowTalismanDetail(int talismanid)
    {
        if (CollectionLevel2Talisman.Instance!=null)
        {
            CollectionLevel2Talisman.Instance.ShowTalismanDetail(talismanid);
        } 
    }


    public void ShowGroupItem(string itemName)
    {
        if (CollectionLevel2Fuling.Instance != null && CollectionLevel2Fuling.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Fuling.Instance.ShowGroupItem(itemName);
        } 
        else if (CollectionLevel2Star.Instance != null && CollectionLevel2Star.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Star.Instance.ShowGroupItem(itemName);
        } 
        else if (CollectionLevel2Talisman.Instance != null && CollectionLevel2Talisman.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Talisman.Instance.ShowGroupItem(itemName);
        }
    }



    /// <summary>
    /// GC_更新界面显示
    /// </summary>
    public void GC_ReceiveAwardUpdateView(GC_COLLECTIONGROUP_RECEIVEAWARD packet)
    {
        if(packet==null)
        {
            return;
        }
        //弹出奖励提示框
        List<Item> itemList = new List<Item>();
        //把所有奖励物品的ID添加到集合
        Tab_Handbook tHandBook = TableManager.GetHandbookByID(packet.GroupId, 0);
        if(tHandBook==null)
        {
            return;
        }
        for (int i = 0; i < tHandBook.getAwardIDCount(); i++)
        {
            if (tHandBook.GetAwardIDbyIndex(i) != -1)
            {
                int ItemId = tHandBook.GetAwardIDbyIndex(i);
                Tab_Item tItem = TableManager.GetItemByID(ItemId, 0);
                if(tItem!=null)
                {
                    Item item = new Item();
                    item.Atlas = tItem.Atlas;
                    item.Icon = tItem.Icon;
                    item.Name = tItem.Name;
                    item.StackCount = tHandBook.GetAwardCountbyIndex(i);
                    itemList.Add(item);
                }
            }
        }
        DropListWindow.Show(itemList);

        //更新组合界面显示
        if (CollectionLevel2Fuling.Instance != null && CollectionLevel2Fuling.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Fuling.Instance.GC_UpdateGroupView();
        } 
        else if (CollectionLevel2Star.Instance != null && CollectionLevel2Star.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Star.Instance.GC_UpdateGroupView();
        } 
        else if (CollectionLevel2Talisman.Instance != null && CollectionLevel2Talisman.Instance.gameObject.activeInHierarchy)
        {
            CollectionLevel2Talisman.Instance.GC_UpdateGroupView();
        }
    }

    
}
