local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--禁疗塔--对敌方全体施加持续2回合的禁疗效果。

local i_CureProhibit = i_mk(sc.CommonBuffs.CureProhibit)   --引用禁疗塔buff
      i_CureProhibit.Duration = 2                  --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
   
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_CureProhibit,
        }    
    },
}
return sk_main

