
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--睚眦在回合外遭到攻击时，会对攻击过自己的所有角色进行记忆，
--在自己行动回合会对记录的所有目标进行强力的斩击，在自己回合结束后，记录目标会被清空重新计算；如果记录目标不存在，则睚眦必报处于未激活状态，无法使用。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



local i_qingchubiaoji  = i_mk{
    Duration=0,
    AliveCheckType=1,

    ImpactLogic = 25,              --根据id驱散buff

    Param_1 = 1162210,               -- 被驱散的impact id
}

local sk_main = sk_mk{

    H_1 = h_mk{                         --伤害
        TargetType = 11,
        TargetParam_1 = 1162210,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {                       
            Impact = i_qingchubiaoji,
    }
    },

   
}

return sk_main