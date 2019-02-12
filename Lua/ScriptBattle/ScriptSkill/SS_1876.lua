
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--新手黄泉大招

local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

local i_damage2  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a2",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

local i_damage3  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a3",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

local i_damage4  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a4",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_DefenceEnhance = i_mk{                  --通用防御强化
Id = 915001,
ImpactLogic = 4,
Param_1 = 3,
Param_2 = 0,
Param_3 = 5000,

Duration = 3,
IsShow=1,
AliveCheckType = 1,
AutoFadeOutTag = 0,
ImpactClass = 1,
ImpactSubClass = 0, 
MutexID = 1872,
MutexPriority =1,
}




local sk_main = sk_mk{


    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        }
    },
    H_2 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage2,
        }
    },
    H_3 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage3,
        }
    },
    H_4 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage4,
        }
    },
    H_5 = h_mk{ 
        IsAnimHit=0,                        --伤害
        TargetType = 2,
        I_1 = {
            
            Impact = i_DefenceEnhance,
        }
    },


}

return sk_main