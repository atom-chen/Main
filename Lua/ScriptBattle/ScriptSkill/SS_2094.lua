local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--无视防御塔-对敌方单体造成相当于辅助部位攻击150%的无视防御伤害。

--降低防御
local i_defencereduce  = i_mk{
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,

    ImpactLogic = 4,              --属性修正
    IsShow = 0,
    Param_1 =3,                 -- 战斗属性类型1(无则填 - 1 对应战斗属性的枚举类型 )
    Param_2 =0,                  -- 类型1修正的值，固定值修正(无则填 0 正数增加 负数减小)
    Param_3 = -10000,                  -- 类型1修正的值，百分比修正(无则填 0 正数增加 负数减小)
}

--伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
}

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_defencereduce,
        },  
        I_2 = {
            Impact = i_damage,
        },
    },
}
return sk_main

