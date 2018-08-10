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
public enum ITEM_SECOND
{
    INVALID = -1,
    DEFAULT = 0,//默认
    MEDICINE = 1,//药品
}

//装备部位
public enum EQUIP_TYPE
{
    INVALID = -1,
    HELM = 0,           //头
    CLOTH = 1,          //甲
    WEAPON = 2,         //武器
    SHOES = 3,          //鞋
    NECKLACE = 4,       //项链
    BRACELET = 5,       //手镯
    RING = 6,           //戒指
    WING = 7,           //翅膀
}

//属性类型
public enum ATTR_TYPE
{
    INVALID = -1,
    DAMAGE = 0,
    HP = 1,
}

//装备稀有度
public enum EQUIP_RARE
{
    INVALID=-1,
    C=0,       //灰色
    B=1,       //绿色
    A=2,       //蓝色
    S=3,      //紫色
    SS=4,     //金色
    SSS=5,    //红色
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
    public const int _INVALID_ID = (-1);
    public const int _MAX_STRENGTHEN_ = (15);    //最大强化等级
}

