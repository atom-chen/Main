local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--中毒塔--对敌方全体施加持续2回合的中毒效果。

local i_Poison = i_mk(sc.CommonBuffs.Poison)   --引用中毒buff
      i_Poison.Duration = 3                 --修改持续回合             

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
   
        I_1 = {
            IsChanceRefix = 1,
            Impact = i_Poison,
        }    
    },
}
return sk_main

