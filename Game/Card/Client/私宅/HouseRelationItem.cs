using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using ProtobufPacket;
using UnityEngine;

public class HouseRelationItem : MonoBehaviour
{
    [SerializeField] private UITexture mTexture;
    [SerializeField] private UISprite mIcon;
    [SerializeField] private UILabel mLevel;
    [SerializeField] private UILabel mName;
    [SerializeField] private UISprite mHeadframe;
    [SerializeField] private GameObject mQQSVipIcon;
    [SerializeField] private UILabel mRec;
    [SerializeField] private GameObject mStatePanel;
    [SerializeField] private UILabel mStateDesc;

    private Relation mRelation;

    void OnEnable()
    {
        Yard.msDelOnYardCheckOtherProdState += OnStateUpdate;
    }

    void OnDisable()
    {
        Yard.msDelOnYardCheckOtherProdState -= OnStateUpdate;
    }

    void OnStateUpdate(UInt64 guid, ProtobufPacket.YardProdState state)
    {
        if (this == null || mRelation == null)
            return;

        if (mRelation.Guid == guid)
        {
            mStatePanel.SetActive(true);
            mStateDesc.text = Utils.GetHouseProdStateStr(state);
        }
        else
        {
            mStatePanel.SetActive(false);
        }
    }

    public void Setup(Relation rel, ProtobufPacket.YardOp op, Int64 optime)
    {
        if (rel == null)
            return;

        mRelation = rel;

        mStatePanel.SetActive(false);

        mLevel.text = mRelation.Level.ToString();
        mName.text = mRelation.Name;
        
        if (op == YardOp.YardOp_HELP || op == YardOp.YardOp_STEAL)
        {
            string strtime = "";
            DateTime dtnow = Utils.GetServerAnsiDateTime(GameManager.ServerAnsiTime);
            DateTime dtop = Utils.GetServerAnsiDateTime(optime);
            int deltaDay = dtnow.DayOfYear - dtop.DayOfYear;
            if (deltaDay == 0)
            {
                strtime = string.Format("{0:00}:{1:00}", dtop.Hour, dtop.Minute);
            }
            else
            {
                strtime = StrDictionary.GetDicByID(7976, deltaDay);
            }

            switch (op)
            {
                case YardOp.YardOp_HELP:
                    mRec.text = StrDictionary.GetDicByID(7969, strtime);
                    break;
                case YardOp.YardOp_STEAL:
                    mRec.text = StrDictionary.GetDicByID(7968, strtime);
                    break;
            }
        }
        else
        {
            mRec.text = "";
        }        
        LoadTextureController.LoadTexture(mRelation.Icon, LoadTextureController.BucketType.Head,
             mTexture, mIcon, OnLoadIconFinish, LoadTextureController.LoadImageStyle.MEDIUM);
        HeadFrameHandler.UpdateHeadFrameUI(mHeadframe, mRelation.HeadFrame);
        
        mQQSVipIcon.SetActive(GlobeVar._GameConfig.m_bIsMSDKQQWXOpen && mRelation.IsQQSVip());
        
    }

    public void OnLoadIconFinish(bool bSucc, string textureName)
    {
        if (this == null)
            return;
        if (mTexture == null)
            return;
        mIcon.gameObject.SetActive(false);
    }

    public void OnClickCheck()
    {
        if (mRelation == null)
            return;
        Yard.SendCheckState(mRelation.Guid);
    }

    public void OnClickVisit()
    {
        if (mRelation == null)
            return;
        Yard.SendEnter(mRelation.Guid);
    }

    public void OnClickHideState()
    {
        mStatePanel.SetActive(false);
    }
}
