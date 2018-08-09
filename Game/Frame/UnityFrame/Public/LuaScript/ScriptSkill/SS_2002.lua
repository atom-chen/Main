local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神-水 3技能

--免疫盾
local i_hundun = i_mk{
    Id = 2002000,
    ImpactLogic = 16,                       --免疫hit
   
    Param_1 = "a1",                         --次数（-1无限）
    Param_2 = 6,                          --impactClass，0时不考虑，非0时，hit只要包含了这类impact就免疫

    Duration =2,
    AliveCheckType = 1,
}
          

--给自己加盾
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_hundun,
        },  
    },
}
return sk_main

