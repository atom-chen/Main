local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御加强buff
      i_DefenceEnhance.Duration = 2                            --修改持续回合     

local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击加强buff
      i_AttackEnhance.Duration = 2                            --修改持续回合    




-- local i_dispel  = i_mk{
--     ImpactLogic = 5,              --素馨驱散我方减益
--     Param_1 =4,   
--     Param_4 =99,
-- }

local i_recover  = i_mk{
    ImpactLogic = 1,              --素馨回血
    Param_1 =3,   
    Param_2 ="a1",
}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_AttackEnhance,
        },
        I_2 = {
            Impact = i_DefenceEnhance,
        },
    },
    H_2 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_recover,
        },
    },
}
return sk_main


