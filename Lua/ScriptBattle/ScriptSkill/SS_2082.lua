local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--攻击buff塔-为首领施加持续2回合的攻击强化效果。


local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)   --引用通用攻击强化buff
      i_AttackEnhance.Duration = 2                           --修改持续回合             

--
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_AttackEnhance,
        },  
    },
}
return sk_main

