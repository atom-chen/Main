using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseRelationRoot : UIControllerBase<HouseRelationRoot>
{
    public static void Open()
    {
        UIManager.ShowUI(UIInfo.HouseRelationRoot);
    }

    public static void Close()
    {
        UIManager.CloseUI(UIInfo.HouseRelationRoot);
    }

    [SerializeField] private TabController mTabController;
    [SerializeField] private TabButton mTabFriend;
    [SerializeField] private TabButton mTabProdOp;

    void OnEnable()
    {
        if (LoginData.user == null)
        {
            return;
        }

        mTabController.InitData();
        mTabController.SetTabDisabled(mTabController.GetTabButtonIndex(mTabProdOp), !Yard.IsCurOwner(LoginData.user.guid));
        mTabController.ChangeTab(mTabController.GetTabButtonIndex(mTabFriend));
    }

    public void OnClickClose()
    {
        Close();
    }
}
