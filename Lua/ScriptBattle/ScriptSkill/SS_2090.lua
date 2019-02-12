local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--免疫塔塔-为首领施加持续2回合的免疫塔效果。


local i_Immune = i_mk(sc.CommonBuffs.Immune)   --引用通用免疫塔buff
      i_Immune.Duration = 2                           --修改持续回合             

--
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_Immune,
        },  
    },
}
return sk_main

