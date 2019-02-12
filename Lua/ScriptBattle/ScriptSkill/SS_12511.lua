
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海东青2技能：海东青为自身施加持续2回合的攻击强化效果和狂暴效果 。【人界效果】使用【狂煞】后立即获得回合。冷却时间5回合。



--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)     --引用攻击强化效果
      i_AttackEnhance.Duration = 2                             --修改持续回合          


--狂暴效果
local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)     --引用狂暴效果
      i_CriticalEnhance.Duration = 2                               --修改持续回合          


local sk_main = sk_mk{

    H_1 = h_mk{                         
        TargetType = 2,
        I_1 = {
            Impact = i_AttackEnhance,
        },
        I_2 = {
            Impact = i_CriticalEnhance,
        },
    }
}

return sk_main