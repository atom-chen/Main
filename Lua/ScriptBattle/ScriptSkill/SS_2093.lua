local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--治疗塔-为首领回复相当于其最大生命值50%的生命。


--治疗
local i_cure  = i_mk{
    ImpactLogic = 1,              --治疗

    Param_1 =3,                 -- 治疗类型(1攻击力，2治疗者血上限，3被治疗者血上限)
    Param_2 ="a1",                 -- 技能系数(10000表示造成的100%的治疗)
}                                 

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_cure,
        },  
    },
}
return sk_main

