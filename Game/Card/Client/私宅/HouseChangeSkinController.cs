using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseChangeSkinController : MonoBehaviour
{
    private static HouseChangeSkinController _Ins;
    public static HouseChangeSkinController Instance { get { return _Ins; } }


    public ListWrapController m_Wrap;
    public UIEventTrigger m_ExitEvent;
    private List<Tab_HouseSkin> m_HouseSkinList=new List<Tab_HouseSkin>();

    public static void Show()
    {
        UIManager.ShowUI(UIInfo.HouseChangeSkinRoot);
    }

    public static void Close()
    {
        UIManager.CloseUI(UIInfo.HouseChangeSkinRoot);
    }
    void Awake()
    {
        _Ins = this;
    }
    void OnDestroy()
    {
        Yard.msDelOnYardUnlockSkin -= UpdateSkinUnLock;
        _Ins = null;
    }
    void Start()
    {
        var temp = TableManager.GetHouseSkin().Values;
        foreach(List<Tab_HouseSkin> list in temp)
        {
            if(list.Count > 0)
            {
                m_HouseSkinList.Add(list[0]);
            }
        }
        m_Wrap.InitList(m_HouseSkinList.Count, UpdateHouseSkinItem, false);
        Yard.msDelOnYardUnlockSkin += UpdateSkinUnLock;
        m_ExitEvent.onClick.Add(new EventDelegate(Close));
    }
    private void UpdateHouseSkinItem(GameObject obj, int index)
    {
        HouseChangeSkinItem houseSkinItem = obj.GetComponent<HouseChangeSkinItem>();
        if (houseSkinItem != null)
        {
            if(index < m_HouseSkinList.Count)
            {
                houseSkinItem.Refresh(m_HouseSkinList[index]);
            }
            else
            {
                houseSkinItem.Refresh(null);
            }
        }
    }


    public void UpdateSkinUnLock(bool suc, int skin)
    {
        if(suc)
        {
            m_Wrap.InitList(m_HouseSkinList.Count, UpdateHouseSkinItem, false);
        }
    }
}
