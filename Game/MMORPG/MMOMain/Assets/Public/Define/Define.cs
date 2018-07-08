using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 物品一级分类
/// </summary>
public enum ITEM_FIRST
{
    INVALID = -1,
    DRUG = 0,//物品
    EQUIP = 1,//装备
}
//二级分类
public enum ITEM_TYPE
{
    INVALID = -1,
    DEFAULT = 0,//默认
    MEDICINE = 1,//药品
}

//装备类型
public enum EQUIP_TYPE
{
    INVALID = -1,
    HELM = 0,
    CLOTH = 1,
    WEAPON = 2,
    SHOES = 3,
    NECKLACE = 4,
    BRACELET = 5,
    RING = 6,
    WING = 7,
}

//属性类型
public enum ATTR_TYPE
{
    INVALID = -1,
    HP = 0,
    MP = 1,
}

//角色信息类型
public enum INFO_TYPE
{
    INVALID,
    NAME,
    SEX,
    USERID,
    HEADICON,
    EXP,
    COIN,
    YUANBAO,
    ENERGY,
    TOUGHEN,
}

class Define
{
    public const int _INVALID_ID = (0);
}

