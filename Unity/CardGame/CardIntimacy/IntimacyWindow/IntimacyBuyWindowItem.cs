using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
//********************************************************************
// 描述: 购买窗口Item
// 作者: wangbiyu
// 创建时间: 2018-3-8
//
//这里出现亲密度礼物的ADD逻辑
//********************************************************************
public class IntimacyBuyWindowItem : MonoBehaviour {
    public IntimacyBuyWindow m_Parent;

    public UILabel m_GiftName;//礼品名称
    public UISprite m_Icon;//图标
    public UILabel m_Desc;//描述
    public UILabel m_Count;//数量
    public UILabel m_TotalMoney;//购买总价
    public UILabel m_GrayMoney;//灰色的时候显示单价

    public GameObject m_CanBuy;
    public GameObject m_CannotBuy;

    [HideInInspector]
    public int m_ItemID = -1;//物品ID
    [HideInInspector]
    public int m_ItemMoney = 288;//单价
    [HideInInspector]
    public int m_Quantity=1;//购买数量

    private BuyLimitPair m_ItemLimitPair;//限购
    public UILabel m_Limit;//限购Label

    private TweenPosition m_TweenPos;
    private TweenAlpha m_TweenAlpha;
    private UIWidget m_Widget;
    /// <summary>
    /// 初始化Item
    /// </summary>
    public void InitBuyWindowItem(string Name, string itemName, string Desc, int itemId, CURRENCY_TYPE currencyType, int ItemMoney,BuyLimitPair limit)
    {
        m_ItemLimitPair = limit;
        m_ItemID = itemId;
        m_ItemMoney = ItemMoney;
        if (m_ItemLimitPair.CurNum <= 0)
        {
            m_Quantity = 0;
        }
        else
        {
            m_Quantity = 1;
        }
        if(m_GiftName!=null)
        {
            m_GiftName.text = Name;
        }
        if(m_Icon!=null)
        {
            m_Icon.spriteName = itemName;
        }
        if(m_Desc!=null)
        {
            m_Desc.text = Desc;
        }
        if(m_Limit!=null)
        {
            m_Limit.text = StrDictionary.GetClientDictionaryString("#{5430}",m_ItemLimitPair.CurNum);
        }
        if(limit.CurNum>0)
        {
            if(m_CanBuy!=null)
            {
                m_CanBuy.SetActive(true);
            }
            if(m_CannotBuy!=null)
            {
                m_CannotBuy.SetActive(false);
            }
        }
        else
        {
            if (m_CanBuy != null)
            {
                m_CanBuy.SetActive(false);
            }
            if (m_CannotBuy != null)
            {
                m_CannotBuy.SetActive(true);
            }
        }
        if (m_GrayMoney != null)
        {
            m_GrayMoney.text = m_ItemMoney + "";
        }
        UpdateView();
    }

    //更新数字显示
    private void UpdateView()
    {
        if(m_Count!=null)
        {
            m_Count.text = m_Quantity + "";
        }
        if(m_TotalMoney!=null)
        {
            m_TotalMoney.text = m_ItemMoney * m_Quantity + "";
        }
    }

    //点击增加
    public void OnClickAdd()
    {
        if (m_Quantity < m_ItemLimitPair.CurNum)
        {
            m_Quantity++;
            UpdateView();
        }
    }

    //点击减少
    public void OnClickDecrease()
    {
        if(m_Quantity>1)
        {
            m_Quantity--;
            UpdateView();
        }
    }

    //输入框提交的回调
    public void OnInputCommit(int num)
    {
        //如果在界限内
        if (num >= 1 && num <= m_ItemLimitPair.CurNum)
        {
            m_Quantity = num;
            UpdateView();
        }
        else
        {
            m_Quantity = 1;
            UpdateView();
        }

    }

    //呼出小键盘
    public void OnEditNumClick()
    {
        if(m_ItemLimitPair.CurNum<=0)
        {
            Utils.CenterNotice(6805);
            return;
        }
        //通知BuyWindow隐藏其他两个Item
        m_Parent.OnClickBuyItemNum(this);
        DigitalKeyboard.Show(m_ItemLimitPair.CurNum, DigitalKeyboard.KeyboardPosPointEnum.ENUM_RIGHT, OnInputCommit);
        DigitalKeyboard.OnDigitalKeyboardClose += OnEditNumClose;
    }
    //键盘退出的回调
    public void OnEditNumClose()
    {
        //通知BuyWindow显示其他两个Item
        m_Parent.OnNumClose();
    }
    void Start()
    {
        m_TweenPos = this.GetComponent<TweenPosition>();
        m_TweenAlpha = this.GetComponent<TweenAlpha>();
        m_Widget = this.GetComponent<UIWidget>();
        if(m_TweenAlpha!=null)
        {
            m_TweenAlpha.AddOnFinished(new EventDelegate(OnTweenFinish));
        }
    }
    void OnEnable()
    {
        if(m_Widget!=null)
        {
            m_Widget.alpha = 1;
        }

    }

    
    //隐藏
    public void Hide(bool isPit)
    {
        if(m_TweenAlpha!=null && !isPit)
        {
            m_TweenAlpha.PlayForward();
        }
        if(m_TweenPos!=null && isPit)
        {
            m_TweenPos.PlayForward();
        }
    }
    
    //展示
    public void Show()
    {
        if (m_TweenAlpha != null)
        {
            m_TweenAlpha.PlayReverse();
        }
        if (m_TweenPos != null)
        {
            m_TweenPos.PlayReverse();
        }
    }

    //alpha动画播放结束的回调
    private void OnTweenFinish()
    {
        if(m_TweenAlpha!=null && m_TweenAlpha.value==0)
        {
            this.gameObject.SetActive(false);
        }
    }


    //点击购买
    public void BuyGift()
    {
        if (m_Quantity<=0)
        {
            return;
        }
        //检测当前的钱，不够则弹出提出。
        if (GameManager.PlayerDataPool.GetYuanBao() < m_Quantity * m_ItemMoney)
        {
            Utils.CenterNotice(5346);
            return;
        }  
        //把当前id和数量发出去
        CG_CARD_INTIMACY_BUYGIFT_PAK pak = new CG_CARD_INTIMACY_BUYGIFT_PAK();
        pak.data.ItemId = this.m_ItemID;
        pak.data.Count = this.m_Quantity;
        pak.SendPacket();
    }

    //不能买的东西的回调
    public void CanNotBuyGift()
    {
        Utils.CenterNotice(6805);
        return;
    }

}
