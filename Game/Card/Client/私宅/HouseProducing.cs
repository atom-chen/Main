using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Table;
using UnityEngine;

public class HouseProducing : UIControllerBase<HouseProducing>
{
    [SerializeField] private UILabel mProdType;
    [SerializeField] private UILabel mCardCnt;
    [SerializeField] private UILabel mTotalIntimacy;

    [SerializeField] private GameObject mTimeObj;
    [SerializeField] private UILabel mCountDown;
    [SerializeField] private UILabel mFinished;

    [SerializeField] private StateGroup mStateGroup;

    public static void Open()
    {
        UIManager.ShowUI(UIInfo.HouseProducingRoot);
    }

    public static void Close()
    {
        UIManager.CloseUI(UIInfo.HouseProducingRoot);
    }

    void Awake()
    {
        SetInstance(this);
    }

    void OnEnable()
    {
        if (GameManager.PlayerDataPool == null ||
            GameManager.PlayerDataPool.YardData == null ||
            GameManager.PlayerDataPool.YardData.ProtoYard == null)
        {
            return;
        }

        var prod = GameManager.PlayerDataPool.YardData.ProtoYard.YardProd;
        if (prod == null)
            return;

        var tab = TableManager.GetHouseProdByID(prod.ProdId, 0);
        if (tab == null)
            return;

        mProdType.text = Utils.GetHouseProdTypeStr((GlobeVar.HouseProdType) tab.Type);
        mCardCnt.text = prod.ProdCardCnt.ToString();
        mTotalIntimacy.text = prod.ProdIntimacyValue.ToString();

        UpdateTime(prod);
    }

    void UpdateTime(ProtobufPacket._YardProd prod)
    { 
        long LeftSecond = prod.FinishTime - GameManager.ServerAnsiTime;
        if (LeftSecond > 0)
        {
            mTimeObj.SetActive(true);
            mFinished.gameObject.SetActive(false);
            mStateGroup.ChangeState("Disabled");

            TimeSpan span = TimeSpan.FromSeconds(LeftSecond);
            mCountDown.text = span.ToString();
        }
        else
        {
            mTimeObj.SetActive(false);
            mFinished.gameObject.SetActive(true);
            mStateGroup.ChangeState("Enabled");
        }
    }

    void FixedUpdate()
    {
        if (GameManager.PlayerDataPool == null ||
            GameManager.PlayerDataPool.YardData == null ||
            GameManager.PlayerDataPool.YardData.ProtoYard == null)
        {
            return;
        }

        var prod = GameManager.PlayerDataPool.YardData.ProtoYard.YardProd;
        if (prod == null)
            return;

        UpdateTime(prod);
    }

    public void OnClickClose()
    {
        Close();
    }

    public void OnClickHarvest()
    {
        if (GameManager.CDManager.GetCoolDown(COOLDOWN_TYPE.HOUSE_PROD))
            return;
        GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.HOUSE_PROD, 2f);

        Yard.SendHarvest();
    }
}
