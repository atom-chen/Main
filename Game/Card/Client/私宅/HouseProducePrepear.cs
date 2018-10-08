using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Games.GlobeDefine;
using Games.Table;

public class HouseProducePrepear : UIControllerBase<HouseProducePrepear>
{
    [SerializeField]
    private UILabel mCardCount;
    [SerializeField]
    private UILabel mTotalIntimacy;
    [SerializeField]
    private TabController mTabProdType;
    [SerializeField]
    private TabController mTabProdTime;
    [SerializeField]
    private StateGroup mBtnStart;

    //(0-金币,1-经验蛋,2-随机)
    [Header("数值须与表格匹配,不可随意修改")]
    [SerializeField]
    int[] mTypeRef = { 1, 0, 2 };
    [SerializeField]
    int[] mTimeRef = { 4, 8, 16 };

    Yard mYard;

    public static void Open()
    {
        UIManager.ShowUI(UIInfo.HouseProducePrepearRoot,null,null,UIStack.StackType.PushAndPop);
    }

    public static void Close()
    {
        UIManager.CloseUI(UIInfo.HouseProducePrepearRoot);
    }

    void Awake()
    {
        SetInstance(this);
    }

    void OnEnable()
    {   
        Setup();
    }

    public void Setup()
    {
        mYard = GameManager.PlayerDataPool.YardData;
        if (mYard == null)
            return;

        mCardCount.text = mYard.GetCardCount().ToString();
        mTotalIntimacy.text = mYard.GetTotalIntimacy().ToString();

        if (mYard.GetTotalIntimacy() == 0)
        {
            mBtnStart.ChangeState("Disabled");
        }
        else
        {
            mBtnStart.ChangeState("Enabled");
        }
    }

    public void OnClickClose()
    {
        Close();
    }

    public void OnClickPreview()
    {
        if (mYard == null)
            return;

        int intlv = Yard.GetIntimacyLevel(mYard.GetTotalIntimacy());
        HouseProdPreview.Open(intlv);
    }

    public void OnClickStart()
    {
        if (GameManager.CDManager.GetCoolDown(COOLDOWN_TYPE.HOUSE_PROD))
            return;

        if (mYard == null)
            return;

        int type = GlobeVar.INVALID_ID;
        TabButton sel = mTabProdType.curHighLightTab;
        if (sel != null)
        {
            int index = mTabProdType.GetTabButtonIndex(sel);
            if (index >= 0 && index < mTypeRef.Length)
            {
                type = mTypeRef[index];
            }
        }

        int hour = GlobeVar.INVALID_ID;
        sel = mTabProdTime.curHighLightTab;
        if (sel != null)
        {
            int index = mTabProdTime.GetTabButtonIndex(sel);
            if (index >= 0 && index < mTimeRef.Length)
            {
                hour = mTimeRef[index];
            }
        }

        if (type < 0 || hour < 0)
            return;

        string strType = Games.Utils.GetHouseProdTypeStr((GlobeVar.HouseProdType)type);
        string title = StrDictionary.GetDicByID(8357);
        MessageBoxController.OpenOKCancel(StrDictionary.GetDicByID(8026, strType, hour), title, () =>
        {
            int intlv = Yard.GetIntimacyLevel(mYard.GetTotalIntimacy());
            var list = Yard.GetProdByParam(type, hour, intlv);
            if (list == null || list.Count == 0)
                return;

            GameManager.CDManager.SetCoolDown(COOLDOWN_TYPE.HOUSE_PROD, 2f);

            Yard.SendStartProd(list[0].Id);
        });
    }

    public void OnClickDisabledStart()
    {
        Games.Utils.CenterNotice(8025);
    }
}
