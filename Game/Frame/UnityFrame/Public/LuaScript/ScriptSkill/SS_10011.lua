local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_Immune = i_mk(sc.CommonBuffs.Immune)       --引用通用免疫buff
      i_Immune.Duration = 2                        --修改持续回合     

local i_Cure = i_mk(sc.CommonBuffs.Cure)       --引用通用恢复buff
      i_Cure.Duration = 2                      --修改持续回合     

    --   local i_texiao = i_mk{
    --     Id=1001010,
    -- }

    local i_hudun = i_mk{
        Id=1001010,
        Duration = 2,
        MutexID = 1001,
        MutexPriority =1,
        ImpactClass = 1,
        ImpactSubClass = 2048, 
        ImpactLogic = 9,                        --护盾（受到伤害时，优先扣护盾的数值，扣完后，buff移除）
        Param_1 = 1500,                            --被加护盾符灵的血量上限百分比
        Param_2 = 0,  
    }      


local sk_main = sk_mk{
    H_1 = h_mk{
        I_1 = {
            Impact = i_Cure,
        },
        I_2 = {
            Impact = i_Immune,
        },
        I_3 = {
            Impact = i_hudun,
        },
    },
}
return sk_main


