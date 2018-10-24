using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum UIType
{
    BASE,         //0 底层UI
    POP,          //1，遮挡
    MESSAGE,     //2，不互斥，遮挡
    TIPS,         //3，不互斥，不遮挡
}

public class UIInfoData
{
    public UIInfoData(string path,UIType uiType=UIType.POP)
    {
        UIPath=path;
        _UIType = uiType;
    }
    public string UIPath="";
    public UIType _UIType;
}

public class UIInfo
{
    public static UIInfoData _MainUI = new UIInfoData("mainPanel", UIType.BASE);
    public static UIInfoData LaunchUI = new UIInfoData("StartPanel", UIType.POP);

    public static UIInfoData RoleStatus = new UIInfoData("RoleState", UIType.POP);
    public static UIInfoData BagUI = new UIInfoData("Knapsack", UIType.POP);

    public static UIInfoData LoadingUI = new UIInfoData("Loading", UIType.TIPS);
    public static UIInfoData ShowEquip = new UIInfoData("EquipTips", UIType.MESSAGE);
    public static UIInfoData ShowItem = new UIInfoData("ItemTips", UIType.MESSAGE);

    public static UIInfoData TipsUI = new UIInfoData("tip", UIType.TIPS);
}

