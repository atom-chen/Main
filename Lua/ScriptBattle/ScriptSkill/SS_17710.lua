local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御强化buff
      i_DefenceEnhance.Duration = 2                            --修改持续回合 

local i_Immune = i_mk(sc.CommonBuffs.Immune)           --引用通用免疫
      i_Immune.Duration = 2                            --修改持续回合 


local i_hudun  = i_mk{
    Id = 1771010,
    EffectID = 573,
    ImpactLogic = 9,              -- 狮爷护盾
    Param_1 =1500,                -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_3 =1, 
    ImpactClass = 1,
    Duration = 2,
    IsShow =1,
    MutexID = 1771,
    MutexPriority = 1,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_DefenceEnhance,
        },
        I_2 = {
            Impact = i_hudun,
        },
        I_3 = {
            Impact = i_Immune,
        },
    },
}
return sk_main


