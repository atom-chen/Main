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

public enum BATTLE_TYPE
{
    CONTROL = 1, //控制系
    STORM = 2,   //强攻系
    QUICK = 3,   //敏攻系
    ASSIST = 4,  //辅助系
}
//装备部位
public enum EQUIP_TYPE
{
    INVALID = -1,
    WEAPON = 0,         //武器
    SHUKO = 1,          //护手
    CLOTH = 2,          //甲
    HELM = 3,           //头
    TROUSERS = 4,       //裤子
    SHOES = 5,          //鞋
    RING = 6,           //戒指
    WING = 7,           //翅膀
    MAX
}

//属性类型
public enum ATTR_TYPE
{
    INVALID = -1,
    POWER = 1,       //力量
    AGILITY = 2,     //敏捷
    SPIRIT = 3,     //精神力
    LIFE = 4,         //生命
    WAKAN = 5,      //灵力
}

//装备稀有度
public enum EQUIP_RARE
{
    INVALID=-1,
    C=0,       //白色 十年
    B=1,       //黄色 百年
    A = 2,     //紫色 千年
    S=3,      //黑色 万年
    SS=4,     //红色 十万年
    SSS=5,    //金色 神品
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

