local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--降低防御buff塔--对敌方全体施加持续2回合的防御降低效果。

local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)   --引用防御降低buff
      i_DefenceReduce.Duration = 2                  --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
   
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_DefenceReduce,
        }    
    },
}
return sk_main

