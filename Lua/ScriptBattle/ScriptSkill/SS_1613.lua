local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Burning = i_mk(sc.CommonBuffs.Burning)          --引用通用灼伤
i_Burning.Duration = 2                            --修改持续回合

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",
}




--额外伤害+灼伤
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 4,                      
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_Burning,
            Chance = "a2",
        },  
    },
}

return sk_main