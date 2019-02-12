local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--降低攻击buff塔--对敌方全体施加持续2回合的攻击降低效果。

local i_AttackReduce = i_mk(sc.CommonBuffs.AttackReduce)   --引用通用攻击降低buff
      i_AttackReduce.Duration = 2                  --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
   
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_AttackReduce,
        }    
    },
}
return sk_main

