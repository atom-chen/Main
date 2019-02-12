local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Immune = i_mk(sc.CommonBuffs.Immune)           --引用通用免疫
      i_Immune.Duration = 2                            --修改持续回合 


local i_hudun= i_mk{
    Id = 1661010,
    ImpactLogic = 8,                --逻辑说明：无视伤害

    Param_1 = 3,                    --次数
    Duration = 2,
    IsShow = 1,
    MutexID = 1661,
    MutexPriority =1,
}   


local i_quantiqusan= i_mk{

    ImpactLogic = 5,                --驱散debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subclasss
    Param_3 = -1,                   --tag
    Param_4 = 1,                    --驱散数量
    Param_5 = 1                     --是否提示
    
}   

local sk_assist = sk_mk{                      --我方全体驱散1个减益效果
    H_1 = h_mk{
        TargetType = 3,
        I_1 ={
            Impact =  i_quantiqusan,          --行动条增加
        },
      
    },  
}


local  i_UseSkillImpactFadeOut  = i_mk{  -- 护盾到回合消失时
    ImpactLogic = 24,                    -- 定期触发使用技能

    Param_1 = sk_assist,                 -- 技能id
    Param_2 = 11,                        -- 周期类型（见24号逻辑）--发送的impact被移除
    Param_3 = -1,                        -- 条件类型（见24号逻辑）
    Param_4 = -1,                        -- 条件参数
    Param_5 = -1,                        -- 条件参数
    Param_6 = 1,                        -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                        -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = 1661010,                         -- 周期参数--impactId
    Param_9 =  1,                        -- 周期参数--到回合消失

    Duration =2,
    MutexID = 16611,
    MutexPriority = 1,  
    AliveCheckType = 3,
}



--宫商用金钟罩保护我方单体目标，为其抵挡3次伤害。2回合后如果护盾没有被打破，将为我方全体驱散一个减益效果。冷却时间5回合。【人界效果】为目标施加持续2回合的免疫效果。
local sk_main = sk_mk{
    
    H_1 = h_mk{                          --护盾消失时，驱散群体DEBUFF1次

        TargetType = 1,
        IsAnimHit = 0,

        I_1 = {
        Impact = i_UseSkillImpactFadeOut,
        },
        I_2 = {
            Impact = i_hudun,
        },
        I_3 = {
            Impact = i_Immune,
        },
    },

}

return sk_main
