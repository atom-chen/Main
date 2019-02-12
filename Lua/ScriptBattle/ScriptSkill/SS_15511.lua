
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--地动2技能：地动为我方全体施加持续2回合的急速效果 。【人界效果】为我方全体施加持续2回合的攻击强化效果。冷却时间5回合。


--急速效果
local i_SpeedEnhance = i_mk(sc.CommonBuffs.SpeedEnhance)     --引用急速效果
      i_SpeedEnhance.Duration = 2                            --修改持续回合          


--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)     --引用攻击强化效果
      i_AttackEnhance.Duration = 2                             --修改持续回合          


local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 3,
        I_1 = {
            Impact = i_SpeedEnhance,
        },
        I_2 = {
            Impact = i_AttackEnhance,
        },
    }
}

return sk_main