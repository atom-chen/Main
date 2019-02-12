
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--山鬼2技能：山鬼驱使赤豹扑咬敌人两次，每次攻击造成山鬼攻击300%的普通伤害，并回复所造成伤害量20%的生命。冷却时间4回合。


local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

local i_damage2  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a2",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}    

local i_xixue = i_mk{
    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag=8,
    MutexID=1831,
    MutexPriority=1,
    ImpactLogic =0,                      

}



local sk_main = sk_mk{

    H_1 = h_mk{                         --空impact
        TargetType = 2,
        I_1 = {
            Impact = i_xixue,
        },
    },

    H_2 = h_mk{                         --1段伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },

    H_3 = h_mk{                         --2段伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage2,
        },
    },

}

return sk_main