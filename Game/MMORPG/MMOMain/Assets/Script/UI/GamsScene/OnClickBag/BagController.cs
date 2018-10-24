using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour 
{
    public class BagItemModel
    {
        public ITEM_FIRST type;
        public int itemId;
        public int count;
    }
    private static BagController _ins;
    public static BagController Instance{get { return _ins; }}

    public BagEquipItem m_Weapon;
    public BagEquipItem m_Shuku;
    public BagEquipItem m_Cloth;
    public BagEquipItem m_Head;
    public BagEquipItem m_Trousers;
    public BagEquipItem m_Shoes;
    public BagEquipItem m_Ring;
    public BagEquipItem m_Wing;

    public UILabel m_DamageLabel;
    public UILabel m_LefeLabel;
    public UISlider m_ExpSlider;
    public UILabel m_ExpLabel;

    public ListWrapController m_Wrap;
    private List<Item> m_ItemList;
    private List<Equip> m_EquipList;
    void Awake()
    {
        _ins = this;
    }
    void OnDestroy()
    {
        _ins = null;
    }
    void OnEnable()
    {
        InitWindow();
    }


    /// <summary>
    /// 从Player中取出装备背包和物品背包显示
    /// </summary>
    void InitWindow()
    {
        RefreshBattleInfo();
        RefreshBag();
    }

    //左侧装备
    void RefreshBattleInfo()
    {
        m_Weapon.Refresh(GameManager.PlayerData.RoleData.equipInfo.Weapon);
        m_Shuku.Refresh(GameManager.PlayerData.RoleData.equipInfo.Shuko);
        m_Cloth.Refresh(GameManager.PlayerData.RoleData.equipInfo.Cloth);
        m_Head.Refresh(GameManager.PlayerData.RoleData.equipInfo.Helm);
        m_Trousers.Refresh(GameManager.PlayerData.RoleData.equipInfo.Trousers);
        m_Shoes.Refresh(GameManager.PlayerData.RoleData.equipInfo.Shoes);
        m_Wing.Refresh(GameManager.PlayerData.RoleData.equipInfo.Wing);
        m_Ring.Refresh(GameManager.PlayerData.RoleData.equipInfo.Ring);
        //m_DamageLabel.text= PlayData.RoleData.
    }

    //右侧背包
    void RefreshBag()
    {
        m_ItemList = GameManager.PlayerData.RoleData.bag.GetList();
        m_EquipList = GameManager.PlayerData.RoleData.equipBag.GetList();
        m_Wrap.InitList(m_ItemList.Count + m_EquipList.Count, OnWrapSlide);
    }





    /// <summary>
    /// 滑动时被调用
    /// </summary>
    void OnWrapSlide(GameObject obj, int dataIndex)
    {
        BagItem item = obj.GetComponent<BagItem>();
        if(item!=null)
        {
            BagItemModel model=new BagItemModel();
            int id;
            ITEM_FIRST type;
            if(GetDataItem(dataIndex,out type,out id))
            {
                switch(type)
                {
                    case ITEM_FIRST.EQUIP:
                        Equip equip = m_EquipList[id];
                        model.itemId = equip.GetTabItem().id;
                        model.count = 1;
                        model.type = type;
                        break;
                    case ITEM_FIRST.DRUG:
                        Item temp = m_ItemList[id];
                        model.itemId = temp.GetTabItem().id;
                        model.count = temp.count;
                        model.type = type;
                        break;
                }
                item.Init(model);
            }
            else
            {
                item.Init(null);
            }
        }
    }

    //取出背包+装备背包拼接起来的容器 index指定信息
    private bool GetDataItem(int dataIndex, out ITEM_FIRST type, out int bagIndex)
    {
        if (dataIndex < 0)
        {
            type = ITEM_FIRST.INVALID;
            bagIndex = Define._INVALID_ID;
            return false;
        }
        //如果在物品背包的范围
        if (dataIndex < m_ItemList.Count)
        {
            type = ITEM_FIRST.DRUG;
            bagIndex = dataIndex;
            return true;
        }
        //如果在装备背包的范围
        else if (dataIndex < (m_ItemList.Count + m_EquipList.Count))
        {
            dataIndex -= m_ItemList.Count;
            type = ITEM_FIRST.EQUIP;
            bagIndex = dataIndex;
            return true;
        }
        else
        {
            //越界
            type = ITEM_FIRST.INVALID;
            bagIndex = Define._INVALID_ID;
            return false;
        }
    }

    public void HandleOnItemClick(BagItem item)
    {
        if(item.m_Data == null)
        {
            return;
        }
        UIManager.CloseUI(UIInfo.ShowEquip);
        BagItemTips.Show(item.m_Data);
    }

    public void HandleOnEquipClick(BagEquipItem equip)
    {
        //m_ItemTips.OnClickMask();
        //BagEquipTips.Show(equip.);
        UIManager.CloseUI(UIInfo.ShowItem);
        BagEquipTips.Show(equip.EquipData);
    }
}
