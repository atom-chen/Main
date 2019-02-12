local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_hpdmgup = i_mk{
    ImpactLogic = 58,                      --吸血
    Param_1 = 1,
    Param_2 = 6,
    Param_3 = 10000,
    Param_4 = 0,
    Duration =1,
    AliveCheckType = 2,
    AutoFadeOutTag = 8,
}



--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}



--普攻+血越多伤害 越高
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType= 2 ,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_hpdmgup,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        },  
    },
    H_3 = h_mk{
        TargetType = 1,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        },  
    },
    H_4 = h_mk{
        TargetType = 1,
        IsAnimHit = 1,
        I_1 = {
            Impact = i_damage,
        },  
    },

}
return sk_main

