using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
using Games;
using Games.GlobeDefine;

public class CollectionStarController : MonoBehaviour 
{
    public ListWrapController m_Wrap;

    public UITexture m_QuartzIcon;
    public UILabel m_QuartzNameLabel;
    public UILabel m_QuartzDesc;
    public UILabel m_QuartzSetDesc;
    public UILabel m_QuartzDropLabel;

    private List<Tab_QuartzClass> mTabClassList = new List<Tab_QuartzClass>();
    [HideInInspector]
    public int m_ChooseClassId = GlobeVar.INVALID_ID;
    private static CollectionStarController _Ins = null;
    public static CollectionStarController Instance
    {
        get { return _Ins; }
    }
    void Awake()
    {
        _Ins = this;
        var val = TableManager.GetQuartzClass().Values;
        foreach (var temp in val)
        {
            if (temp != null && temp.Count > 0)
            {
                mTabClassList.Add(temp[0]);
            }
        }
    }
    void Start()
    {
        if (mTabClassList.Count > 0)
        {
            m_ChooseClassId = mTabClassList[0].Id;
        }

        m_Wrap.InitList(mTabClassList.Count, UpdateQuartzWrap);

        if (mTabClassList.Count > 0)
        {
            HandleOnChooseItem(mTabClassList[0]);
        }
    }
    void OnDestroy()
    {
        _Ins = null;
    }
    void UpdateQuartzWrap(GameObject obj,int idx)
    {
        if(obj == null)
        {
            return;
        }
        CollectionStarItem item = obj.GetComponent<CollectionStarItem>();
        if(item == null)
        {
            return;
        }

        if (idx >= mTabClassList.Count || idx < 0)
        {
            item.Refresh(null);
        }
        else
        {
            item.Refresh(mTabClassList[idx]);
        }
    }

    public void HandleOnChooseItem(Tab_QuartzClass tClass)
    {
        if (tClass == null)
        {
            return;
        }
        m_ChooseClassId = tClass.Id;
        AssetManager.SetTexture(m_QuartzIcon, tClass.SoulPic);
        m_QuartzNameLabel.text = tClass.Name;
        m_QuartzDesc.text = tClass.Background;
        m_QuartzSetDesc.text = QuartzTool.GetFormatSetAttr_Recomand(tClass);
        m_QuartzDropLabel.text = QuartzTool.GetDropName(tClass);
        m_Wrap.UpdateAllItem();
        //是否拥有
        if (GameManager.PlayerDataPool.CollectionData != null)
        {
            bool bSuc = GameManager.PlayerDataPool.CollectionData.IsQuartzClassGet(tClass.Id);
            if (bSuc)
            {
                m_QuartzIcon.color = GlobeVar.NORMALCOLOR;
            }
            else
            {
                m_QuartzIcon.color = GlobeVar.GRAYCOLOR;
            }
        }
    }
    public void OnClickExit()
    {
        UIManager.CloseUI(UIInfo.CollectionStarRoot);
    }
}
