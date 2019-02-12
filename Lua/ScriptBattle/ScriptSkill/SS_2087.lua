local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--弱化塔--对敌方全体施加持续2回合的虚弱效果。

local i_Weak = i_mk(sc.CommonBuffs.Weak)   --引用虚弱buff
      i_Weak.Duration = 2                  --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
   
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_Weak,
        }    
    },
}
return sk_main

