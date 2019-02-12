
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--山鬼1技能-普攻+妖界击杀获得回合


local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}



-- --获得回合
local i_getround = i_mk{
    Duration=0,
    AliveCheckType=1,
    IsShow=1,

    ImpactLogic =15,                 --下回合立即行动       

    Param_1= 1,                      --是否无视主角（1无视，0不无视）
    Param_2= 7560,                   --冒字的字典号

}

local sk_assist = sk_mk{
    EnvLimit = 0,
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact = i_getround,               --获得回合
        },   
    },
    OtherID = sk_mk{
        EnvLimit = 1,
        H_1 = h_mk{

            TargetType = 2,
            I_1 ={
                
                Impact = i_getround,               --获得回合
            },   
        },
    }
   
}

local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
    Duration=1,
    IsShow=0,
    AliveCheckType=2,
    MutexID=120,
    MutexPriority=1,

    RoundMaxEffectedCount=1,
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,            -- 技能id
    Param_2 = 3,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

}

local sk_jisha = sk_mk{
    EnvLimit = 0,
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact =  i_UseSkillRoundOver,               --回合后使用技能
        },   
    } ,
    OtherID = sk_mk{
        EnvLimit = 1,
        H_1 = h_mk{

            TargetType = 2,
            I_1 ={
                
                Impact =  i_UseSkillRoundOver,           --回合后使用技能
            },   
        } ,
    }

}


local  i_UseSkilljisha = i_mk{       -- 击杀后释放技能
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=1830,
    MutexPriority=1,
   
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_jisha,            -- 技能id
    Param_2 = 8,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数


}



local sk_main = sk_mk{

    H_1 = h_mk{                 --击杀后使用技能
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkilljisha,
        },
    },
    H_2 = h_mk{                         --伤害
    TargetType = 1,
 
    I_1 = {
        Impact = i_damage,
    },
},
}

return sk_main