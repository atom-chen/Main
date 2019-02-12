local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--赤鱬1技能

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = 30000,                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}
  
local  i_UseSkilljisha = i_mk{       -- 击杀后释放技能
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=2100,
    MutexPriority=1,
   
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = 210000,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}




--普攻
local sk_main = sk_mk{
    H_1 = h_mk{                 --击杀后使用技能
    TargetType = 2,
    I_1 = {
        Impact = i_UseSkilljisha,
    },
},
    H_2 = h_mk{

        TargetType = 6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
    I_1 = {
            Impact = i_damage,
        },
    },
}
return sk_main

