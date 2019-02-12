local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Poison = i_mk(sc.CommonBuffs.Poison)           --持续伤害
      i_Poison.Duration = 1                            --修改持续回合 

local i_damage1  = i_mk{
    ImpactLogic = 0,              --玉兔普通伤害
    Param_1 ="a1",                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}

local i_tuitiao = i_mk{
    ImpactClass= 4,
    ImpactLogic = 6,                --行动条增减  
    Param_1 = -200,                 --参数：增减数（负数表示减少）
}

local sk_poison = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_Poison,
            IsChanceRefix = 1,
        },
    },
}

local i_critpoison = i_mk{
    ImpactLogic = 44,                       --逻辑说明：属性修正
    Param_1 = sk_poison,                           --//MaxHP = 0,//气血上限
    Duration =1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType= 2 ,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_critpoison,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },
    H_3 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },
    H_4 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },
    H_5 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },
}
return sk_main


