local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神-雷 3技能辅助


local i_AddAtk = i_mk{                    --加10%攻击力
    Id = 2043000,
    ImpactLogic = 4,  
    Param_1 = 2,
    Param_2 = 0,
    Param_3 = 1000,


    Duration = -1,
    AliveCheckType = 1,
    LayerID  = 2043,
    LayerMax = 20,        
}
   

--增加攻击力
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_AddAtk,
        },

     
    },
}
return sk_main


