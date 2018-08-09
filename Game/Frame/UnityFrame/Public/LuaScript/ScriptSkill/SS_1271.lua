local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--免疫DOT
local i_immune1 = i_mk{                 
    ImpactLogic = 7,                       --逻辑说明：免疫、抵消效果
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = 0,                           --参数1：ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 64,                          --参数2：ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,                          --参数3：次数（-1时无限次数）
    Duration =-1,
}

local i_immune2 = i_mk{
    ImpactLogic = 7,                       --逻辑说明：免疫、抵消效果
    ImpactClass= 0,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = 0,                           --参数1：ImpactClass（0时不考虑，否则必须完全满足）
    Param_2 = 4096,                        --参数2：ImpactSubClass（0时不考虑，否则和ImpactClass同时匹配时，抵消，满足任意一个SubClass即可）
    Param_3 = -1,                          --参数3：次数（-1时无限次数）
    Duration =-1,

}






--免疫DOT+免疫削减生命上限
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=1,

        I_1 = {
            Impact = i_immune1,
        },

        I_2 = {
            Impact = i_immune2,
        }    
    },

}

return sk_main