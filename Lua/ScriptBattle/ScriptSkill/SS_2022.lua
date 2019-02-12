local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海神-毒 3技能

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
local i_ActPoison = i_mk{
    ImpactLogic = 63,                      --引爆DOT，立即触发DOT的效果
    Param_1 = 64,                          --SubClass
    Param_2 =-1,                           --增加buff回合（0无，-1减1回合，1加一回合，-2减2回合。。以此类推） 
}   

local i_MoreZhanThreePoison =i_mk{            --超过X层毒触发
    ImpactLogic = 29,

    Param_1 = 2,
    Param_2 = 64,
    Param_3 = "a2",
    Param_4 = i_ActPoison,
}

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=4,
        I_1 = {
            Impact = i_damage,
        },

        I_2 = {
    
            Impact = i_MoreZhanThreePoison,
        } ,
        
    },
}
return sk_main

