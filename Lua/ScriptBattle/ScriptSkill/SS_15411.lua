
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白骨2技能-白骨妖女拔出骨剑攻击敌人，造成白骨妖女攻击520%的普通伤害。敌人的血量越低，伤害越高。击杀敌人后将重置【傲骨】的冷却时间。
--[A78C84]【妖界效果】回复所造成伤害量20%的生命。[-]冷却时间4回合。


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



--敌人血量越低伤害越高
local i_xueliangshanghai = i_mk{
    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag=8,
    MutexID=1541,
    MutexPriority=1,
    ImpactLogic =58,                    -- 根据当前血量（损失血量）计算属性修正 

    Param_1= 2,                         --类型（1，当前血量；2，损失血量）
    Param_2= 7,                         --属性类型
    Param_3= -10000,                     --固定值修正
    Param_4= 0,                         --百分比修正
}


local  i_UseSkilljisha1 = i_mk{       -- 击杀后释放技能
    Duration=1,
    IsShow=1,
    MutexID=1542,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能
    AutoFadeOutTag=4,
    
    Param_1 = 154801    ,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}
local  i_UseSkilljisha2 = i_mk{       -- 击杀后释放技能
    Duration=1,
    IsShow=1,
    MutexID=1542,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能
    AutoFadeOutTag=4,
    

    Param_1 = 154802    ,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}
local  i_UseSkilljisha3 = i_mk{       -- 击杀后释放技能
    Duration=1,
    IsShow=1,
    MutexID=1542,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能
    AutoFadeOutTag=4,

    Param_1 = 154803,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}
local  i_UseSkilljisha4 = i_mk{       -- 击杀后释放技能
    Duration=1,
    IsShow=1,
    MutexID=1542,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能
    AutoFadeOutTag=4,

    Param_1 = 154804,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}
local  i_UseSkilljisha5 = i_mk{       -- 击杀后释放技能
    Duration=1,
    IsShow=1,
    MutexID=1542,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能
    AutoFadeOutTag=4,

    Param_1 = 154805,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}
local i_SkillLevel1=i_mk{
    Duration=0,
    ImpactLogic = 29,

    Param_1 = 0,
    Param_2 = 1549010,
    Param_3 =1,
    Param_4 =i_UseSkilljisha1
}
local i_SkillLevel2=i_mk{
    Duration=0,
    ImpactLogic = 29,

    Param_1 = 0,
    Param_2 = 1549020,
    Param_3 =1,
    Param_4 =i_UseSkilljisha2
}
local i_SkillLevel3=i_mk{
    Duration=0,
    ImpactLogic = 29,

    Param_1 = 0,
    Param_2 = 1549030,
    Param_3 =1,
    Param_4 =i_UseSkilljisha3
}
local i_SkillLevel4=i_mk{
    Duration=0,
    ImpactLogic = 29,

    Param_1 = 0,
    Param_2 = 1549040,
    Param_3 =1,
    Param_4 =i_UseSkilljisha4
}
local i_SkillLevel5=i_mk{
    Duration=0,
    ImpactLogic = 29,

    Param_1 = 0,
    Param_2 = 1549050,
    Param_3 =1,
    Param_4 =i_UseSkilljisha5
}


local sk_main = sk_mk{
 
    H_1 = h_mk{                 --击杀后使用技能
        IsAnimHit=0,
        TargetType = 2,
        I_1 = {
            Impact = i_SkillLevel1,
        },
        I_2 = {
            Impact = i_SkillLevel2,
        },
        I_3 = {
            Impact = i_SkillLevel3,
        },
        I_4 = {
            Impact = i_SkillLevel4,
        },
        I_5 = {
            Impact = i_SkillLevel5,
        },
    },
    H_2 = h_mk{                         --伤害
    TargetType = 1,
    I_1 = {
        Impact = i_xueliangshanghai,
    },
    I_2 = {
        Impact = i_damage,
    },
},
}

return sk_main