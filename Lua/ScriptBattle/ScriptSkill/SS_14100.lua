local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

local ss = require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祝融男1技能-人界-附带灼伤

--普通伤害--高系数
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

     
      
local i_Burning = i_mk(sc.CommonBuffs.Burning)               --引用通用灼伤buff
      i_Burning.Duration = 2                                 --修改持续回合  




local sk_main = sk_mk{                      --人界技能
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Chance = "a2",                 --概率灼伤
            IsChanceRefix = 1,
            Impact = i_Burning,
        },
    },
}


return sk_main

