local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--云梦章1技能

local i_damage1  = i_mk{
    ImpactLogic = 0,              --云梦章普通伤害

    Param_1 ="a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
    ImpactClass = 2,
}

local i_damage2  = i_mk{
    ImpactLogic = 34,              --云梦章固定伤害

    Param_1 ="a2",                 -- 百分比
    Param_2 = -1,                  -- 伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
    Param_3 = -1,                  -- 扣血百分比类型（-1生命上限 0当前血量）
    ImpactSubClass = 32768 ,
}


local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        }  
    },
    H_2 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        }  
    },
    H_3 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        }  
    },
    H_4 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        }  
    },
    H_5 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_damage2,
        }  
    },
}
return sk_main


