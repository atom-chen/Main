using ProtobufPacket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.GlobeDefine;

public class ShareController : MonoBehaviour 
{
    public enum SHARE_TYPE
    {
        INVALID = -1,
        QUARTZ = 0,
    }

    public UIEventTrigger m_MaskEvent;
    public UIEventTrigger m_CloseBtn;
    public GameObject m_MainPanel;
    public GameObject m_FriendListObj;
    public GameObject m_EmptyFriendObj;
    public ListWrapController m_FriendList;

    private static Quartz m_ShareQuartz = null;
    private static SHARE_TYPE m_Type = SHARE_TYPE.INVALID;
    private List<ulong> m_AlreadyShareList = new List<ulong>();
    public static void Show(Quartz nQuartz)
    {
        if(nQuartz == null || nQuartz.Star<GlobeVar._GameConfig.m_nShareQuartzNeedStar)
        {
            return;
        }
        m_ShareQuartz = nQuartz;
        m_Type = SHARE_TYPE.QUARTZ;
        UIManager.ShowUI(UIInfo.ShareRoot);
    }
    public static void Close()
    {
        m_ShareQuartz = null;
        m_Type = SHARE_TYPE.INVALID;
        UIManager.CloseUI(UIInfo.ShareRoot);
    }
    
    void Start()
    {
        m_MaskEvent.onClick.Add(new EventDelegate(Close));
        m_CloseBtn.onClick.Add(new EventDelegate(Close));
        m_MainPanel.SetActive(true);
        m_FriendListObj.SetActive(false);
    }

    public void OnClickFriend()
    {
        m_FriendListObj.SetActive(true);
        m_MainPanel.SetActive(false);
        if(GameManager.PlayerDataPool.Friends.SortedList.Count<=0)
        {
            m_EmptyFriendObj.SetActive(true);
            m_FriendList.gameObject.SetActive(false);
        }
        else
        {
            m_FriendList.gameObject.SetActive(true);
            m_FriendList.InitList(GameManager.PlayerDataPool.Friends.SortedList.Count, UpdateFriendList);
        }

    }

    public void OnClickCloseFriendList()
    {
        m_MainPanel.SetActive(true);
        m_FriendListObj.SetActive(false);
    }
    //分享到世界
    public void OnClickShareToWorld()
    {
        switch (m_Type)
        {
            case SHARE_TYPE.QUARTZ:
                if(HyperlinkTool.ShareQuartz(m_ShareQuartz, CHANNELTYPE.CHANNEL_WORLD) == false)
                {
                    return;
                }
                break;
        }
        Utils.CenterNotice(6742);
        Close();
    }

    //分享到帮会
    public void OnClickShareToGuild()
    {
        if (!Guild.InGuild())
        {
            Utils.CenterNotice(8448);
            return;
        }
        switch(m_Type)
        {
            case SHARE_TYPE.QUARTZ:
                if(HyperlinkTool.ShareQuartz(m_ShareQuartz, CHANNELTYPE.CHANNEL_GUILD) == false)
                {
                    return;
                }
                break;
        }
        Utils.CenterNotice(6742);
        Close();
    }
    void UpdateFriendList(GameObject obj,int index)
    {
        if(obj == null || index <0)
        {
            return;
        }
        InviteItem item = obj.GetComponent<InviteItem>();
        if(item == null)
        {
            return;
        }
        //取出好友数据
        if(index >= GameManager.PlayerDataPool.Friends.SortedList.Count)
        {
            return;
        }
        Relation relation = GameManager.PlayerDataPool.Friends.SortedList[index];
        item.InitShareToFriend(relation,!m_AlreadyShareList.Contains(relation.Guid), OnChooseFriend);
    }

    //选择一个好友进行分享
    void OnChooseFriend(InviteItem item,object param)
    {
        Relation relation = param as Relation;
        if(relation == null)
        {
            return;
        }
        if(m_AlreadyShareList.Contains(relation.Guid))
        {
            return;
        }
        //发送分享数据
        if(HyperlinkTool.ShareQuartz(m_ShareQuartz, relation.Guid) == false)
        {
            return;
        }
        m_AlreadyShareList.Add(relation.Guid);
        m_FriendList.UpdateAllItem();
        Utils.CenterNotice(6742);
    }
}
