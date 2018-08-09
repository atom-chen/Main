local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白狼3技能

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_damage1 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a2",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}          
local i_damage2 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a3",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_damage3 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a4",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_damage4 = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a5",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a6",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
--普攻
local sk_main = sk_mk{
    H_1 = h_mk{                            --一段
        TargetType=10,
        I_1 = {
            Impact = i_damage,
        }, 
    },
    H_2 = h_mk{                            --二段
        TargetType=10,
        I_1 = {
            Impact = i_damage1,
        }, 
    },
    H_3 = h_mk{                            --三段
        TargetType=10,
        I_1 = {
            Impact = i_damage2,
        }, 
    },
    H_4 = h_mk{                           --四段
        TargetType=10,
        I_1 = {
            Impact = i_damage3,
        }, 
    },
    H_5 = h_mk{                          --五段
        TargetType=10,
        I_1 = {
            Impact = i_damage4,
        }, 
    },
    H_6 = h_mk{                          --六段
        TargetType=10, 
        I_1 = {
            Impact = i_damage5,
        }, 
    },
}
return sk_main