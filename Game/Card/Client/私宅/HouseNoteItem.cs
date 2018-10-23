using Games;
using ProtobufPacket;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseNoteItem : HyperlinkBase
{
    public UISprite m_PlayerIconSprite;        //头像
    public UILabel m_PlayerNameLabel;          //名称
    public UILabel m_TimeLabel;                //时间
    public GameObject m_WonderfulObj;          //精彩评论标记
    public UILabel m_LikeCount;                //点赞数
    public GameObject m_DelObj;                //删除评论
    public GameObject m_ShowAllObj;            //评论详情

    public GameObject m_LikeBtnEnable;         //点赞btn
    public GameObject m_LikeBtnDisable;        //点过赞了btn

    [HideInInspector]
    public _YardNote m_NoteData = null;
    public void Refresh(_YardNote info)
    {
        if(info == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        m_NoteData = info;
        Refresh();
    }
    public void Refresh()
    {
        if(m_NoteData == null)
        {
            return;
        }
        HouseScene hs = HouseTool.GetHouseScene();
        if(hs == null)
        {
            return;
        }
        if(m_DelObj!=null)
        {
            m_DelObj.SetActive(hs.IsOwner(LoginData.user.guid));
        }
        if(m_WonderfulObj!=null)
        {
            m_WonderfulObj.SetActive(HouseTool.IsWonderfulNote(m_NoteData.guid));
        }

        var content = m_NoteData.content.Trim().Trim('\r').Trim('\n');
        content = ChatEmotionPage.ReplaceBigEmojName(content);
        HyperlinkBase.FillLinkContent(m_NoteData.linkObjs, ref content);
        word.text = content;

        m_PlayerIconSprite.spriteName = Utils.GetIconStrByID(m_NoteData.playerIcon, true);
        m_PlayerNameLabel.text = m_NoteData.playerName;

        if(m_LikeCount!=null)
        {
            m_LikeCount.text = m_NoteData.likePlayer.Count.ToString();
        }

        if(m_LikeBtnEnable!=null && m_LikeBtnDisable!=null)
        {
            bool isAlreadyLike = m_NoteData.likePlayer.Contains(LoginData.user.guid);
            m_LikeBtnEnable.SetActive(!isAlreadyLike);
            m_LikeBtnDisable.SetActive(isAlreadyLike);
        }
        DateTime dt = Utils.GetServerAnsiDateTime(m_NoteData.time);
        m_TimeLabel.text = StrDictionary.GetDicByID(8634, dt.Year, dt.Month, dt.Day);

        if(m_ShowAllObj!=null)
        {
            var tmp = word.processedText;
            var len = word.processedText.Length;
            if (tmp.IndexOf('\n') != -1)
            {
                len--;
            }
            m_ShowAllObj.SetActive(len != word.text.Length);
        }
    }
    public void OnClickLike()
    {
        CG_YARD_NOTE_LIKE_PAK pak = new CG_YARD_NOTE_LIKE_PAK();
        pak.data.guid = m_NoteData.guid;
        pak.SendPacket();
    }

    public void OnClickDel()
    {
        CG_YARD_NOTE_DEL_PAK pak = new CG_YARD_NOTE_DEL_PAK();
        pak.data.guid = m_NoteData.guid;
        pak.SendPacket();
    }

    public void OnClickDetail()
    {
        if(HouseNoteController.Instance!=null)
        {
            HouseNoteController.Instance.HandleShowDetail(this);
        }
    }

    public void OnClickLable()
    {
        var url = word.GetUrlAtPosition(UICamera.lastWorldPosition);
        if (string.IsNullOrEmpty(url) == false)
        {
            if (null == m_NoteData) return;
            var linkObjs = m_NoteData.linkObjs;
            if (null == linkObjs || null == linkObjs.objs || linkObjs.objs.Count <= 0)
            {
                Utils.CenterNotice(StrDictionary.GetClientDictionaryString("#{5704}"));
                return;
            }

            int index = -1;
            if (int.TryParse(url, out index))
            {
                HandleLinkObjs(index, linkObjs);
            }
            else
            {
                //LogModule.DebugLog("Clicked on url:" + url + ",but condition fail!");
            }

        }
    }
}
