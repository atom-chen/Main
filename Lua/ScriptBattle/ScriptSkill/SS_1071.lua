local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage1  = i_mk{
    ImpactLogic = 0,              --玉兔普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_qusan= i_mk{
    ImpactLogic = 5,                --驱散buff
    
    Param_1 = 1,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 99,                    --驱散数量
    Param_5 = 1                     --是否提示
}   

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 1,
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_qusan,
            Chance = "a2",
        },
        I_2 = {
            Impact = i_damage1,
        },

    },
}
return sk_main


