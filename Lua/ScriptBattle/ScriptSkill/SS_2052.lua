local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--黑狼3技能



local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

local i_jianshe = i_mk{
    ImpactLogic = 59,                     --伤害溅射，产生伤害后，按照比例溅射给敌方2其他人
    ImpactClass= 2, 

    Param_1 = 6000,
    Param_2 = -1,
    Param_3 = -1,

    MutexID = 2052,
    MutexPriority = 1,
    Duration = 1,
    AliveCheckType = 2,
    AutoFadeOutTag = 16,
}

--普攻
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_jianshe,
        },  
    },
    H_2 = h_mk{
        TargetType=10,
        I_1 = {
            Impact = i_damage,
        },  
    },
}
return sk_main

