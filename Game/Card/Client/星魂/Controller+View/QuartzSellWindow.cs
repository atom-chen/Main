using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Item;
using UnityEngine;

public class QuartzSellWindow : MonoBehaviour
{
    private static QuartzSellWindow m_Instance = null;
    public static QuartzSellWindow Instance
    {
        get { return m_Instance; }
    }

    public UILabel m_SellCoinLabel;
    public GameObject m_SellEffect;
    //public GameObject m_SellStoneEffect;

    void OnEnable()
    {
        m_Instance = this;

        m_SellEffect.SetActive(false);
        //m_SellStoneEffect.SetActive(false);
        InitSellCoinLabel();
    }

    void OnDisable()
    {
        m_Instance = null;
    }

    void InitSellCoinLabel()
    {
        if (OrbmentController.Instance == null || OrbmentController.Instance.SellGuidList == null)
        {
            return;
        }

        int nCoin = 0;
        for (int i = 0; i < OrbmentController.Instance.SellGuidList.Count; i++)
        {
            Quartz quartz = GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(OrbmentController.Instance.SellGuidList[i]);
            if (quartz == null)
            {
                continue;
            }

            nCoin += quartz.GetCoin();
        }

        m_SellCoinLabel.text = nCoin.ToString();
    }

    public void OnSellClick()
    {
        if (OrbmentController.Instance == null || OrbmentController.Instance.SellGuidList == null)
        {
            return;
        }

        if (OrbmentController.Instance.SellGuidList.Count <= 0)
        {
            Utils.CenterNotice(6202);
            return;
        }

        for (int i = 0; i < OrbmentController.Instance.SellGuidList.Count; i++)
        {
            Quartz quartz = GameManager.PlayerDataPool.PlayerQuartzBag.GetQuartz(OrbmentController.Instance.SellGuidList[i]);
            if (quartz == null)
            {
                continue;
            }

            if (QuartzTool.GetQuartzStarColor(quartz.Star) >= (int)COLOR.PURPLE)
            {
                MessageBoxController.OpenOKCancel(6203, -1, SellOK);
                return;
            }

            if (quartz.Strengthen >= GlobeVar.QUARTZ_TIPS_LIMIT_LEVEL) 
            {
                MessageBoxController.OpenOKCancel(6864, -1, SellOK);
                return;
            }
        }
        //出售数量超过阈值，给提示
        if(OrbmentController.Instance.SellGuidList.Count >= GlobeVar.QUARTZ_TIPS_LIMIT_NUM)
        {
            string strContent = StrDictionary.GetClientDictionaryString("#{6866}", QuartzTool.GetQuartzStarName(OrbmentController.Instance.SellStar));
            MessageBoxController.OpenOKCancel(strContent, "", SellOK);
            return;
        }
        SellOK();
    }

    void SellOK()
    {
        if (OrbmentController.Instance == null || OrbmentController.Instance.SellGuidList == null)
        {
            return;
        }
        OrbmentController.Instance.HandleTipClose();
        CG_QUARTZ_SELL_PAK pak = new CG_QUARTZ_SELL_PAK();
        for (int i = 0; i < OrbmentController.Instance.SellGuidList.Count; i++)
        {
            pak.data.QuartzGuid.Add(OrbmentController.Instance.SellGuidList[i]);
        }
        pak.SendPacket();
    }

    public void HandleQuartzItemClick()
    {
        InitSellCoinLabel();
    }

    public void HandleSellTypeTabChanged()
    {
        InitSellCoinLabel();
    }

    public void HandleQuartzSell(int nStoneCount)
    {
        InitSellCoinLabel();

        m_SellEffect.SetActive(false);
        m_SellEffect.SetActive(true);
        if (nStoneCount > 0)
        {
            //m_SellStoneEffect.SetActive(false);
            //m_SellStoneEffect.SetActive(true);

            StartCoroutine(ShowStoneItemWindow(nStoneCount));
        }
    }

    IEnumerator ShowStoneItemWindow(int nStoneCount)
    {
        yield return new WaitForSeconds(1.2f);

        Item item = new Item();
        item.DataID = GlobeVar.ORBMENT_ITEMID_STRHENGTHENSTONE;
        item.StackCount = nStoneCount;

        DropListWindow.Show(new List<Item>() { item });
    }
}
