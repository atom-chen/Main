local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--固定伤害塔-对敌方全体造成相当于其最大生命值8%的固定伤害。

--固定伤害
local i_damage  = i_mk{
    ImpactLogic = 34,              --固定伤害

    Param_1 ="a1",                 -- 百分比
    Param_2 = -1,                  -- 伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
    Param_3 = -1,                  -- 扣血百分比类型（-1生命上限 0当前血量）
}

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },  
    },
}
return sk_main

