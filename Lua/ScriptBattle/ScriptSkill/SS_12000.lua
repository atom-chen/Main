local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage  = i_mk{
    ImpactLogic = 0,                --英招普通伤害
    Param_1 ="a1",                  -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_critup  = i_mk{
    ImpactLogic = 4,                -- 英招暴击率增加
    Param_1 = 4 ,                   -- //CritChance = 4,//暴击率
    Param_2 = 3000,
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
}

local i_ifcrit  = i_mk{             --若暴击，则触发技能
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
    RoundMaxEffectedCount = 1,
    ImpactLogic = 44,
    Param_1 = "a2",
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit = 0 ,
        I_1 = {
            Impact = i_ifcrit,   
        },
        I_2 = {
            Impact = i_critup,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
    },
}
return sk_main


