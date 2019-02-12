local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_damage1  = i_mk{
    ImpactLogic = 0,              --穷奇普通伤害
    Param_1 ="a1",         -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    ImpactClass = 2,
}



local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                    ImpactLogic = 4,              --穷奇吸血
                    Param_1 =101,   
                    Param_2 =2000,
                    Param_3 =0,            
                    ImpactClass = 0,
                    Duration = 1,
                    AliveCheckType = 2,
                    AutoFadeOutTag = 8,
                },
        },
        I_2 = {
            Impact = i_mk{
                    ImpactLogic = 58,              --穷奇低血吸血
                    Param_1 =2,   
                    Param_2 =101,
                    Param_3 =5000, 
                    Param_4 =0,           
                    ImpactClass = 0,
                    Duration = 1,
                    AliveCheckType = 2,
                    AutoFadeOutTag = 8,
                },
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },

    },
}
return sk_main


