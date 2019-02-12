
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--应龙2技能：随机攻击敌方目标5次

local i_kezhi  = i_mk{
    Duration=-1,
    AutoFadeOutTag=64,
    Tag= 10004,

    ImpactLogic = -1, 
    
}

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


local i_colddowndown = i_mk{
    Duration=0,

    ImpactLogic =35,                 --增减CD，有CD的技能，才会受这个效果影响         

    Param_1= -1,                      --技能索引（-1表示全部技能(1~3)）
    Param_2= -1,                   --增减值(大于0增加CD，小于0减少CD)

}

local sk_assist1 = sk_mk{
    H_1 = h_mk{
        IsAnimHit=0,
        TargetType = 2,
        I_1 ={
            Impact = i_colddowndown,               --减少技能冷却
        },   
    }
   
}

local  i_UseSkilljisha = i_mk{       -- 击杀后释放技能
    Duration= 1,

    MutexID=1493,
    MutexPriority=1,
    AliveCheckType=2,
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist1,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数
    RoundMaxEffectedCount=1,

}

local sk_main = sk_mk{

    H_1 = h_mk{
        IsAnimHit=0,                         --环境克制
        TargetType = 2,
            I_1 = {
                Impact = i_kezhi,
            }
    },

    H_2 = h_mk{                 --击杀后使用技能
        IsAnimHit=0,
        TargetType = 2,
            I_1 = {
                Impact = i_UseSkilljisha,
            },
    },

    
    H_3 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 1,
        I_1 = {
            Impact = i_damage1,
        },
    },
    H_4 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage2,
        },
    },
    H_5 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage3,
        },
    },
    H_6 = h_mk{
        TargetType=6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage4,
        },
    },

   
}

return sk_main