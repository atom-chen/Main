local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--酒童2技能：在接下来的3回合内受到的伤害降低30%，





local i_tihuan = i_mk{
    Duration=3,
    AliveCheckType=3,
    MutexID=1941,
    MutexPriority=1,
    ImpactLogic = 36,           --技能替换,激活时,替换指定的技能为新技能          
    Param_1 = "a1",            --技能3替换id,不替换配-1
}

local i_reducedam  = i_mk{
    ImpactLogic = 4,               
    Param_1 = 7,                    
    Param_2 = "a2",                    
    Param_3 = 0,

    Duration = 3,
    MutexID  = 1942,
    MutexPriority = 1,
    
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
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact = i_getround,               --获得回合
        },   
    }
   
}

local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
    Duration=1,
    IsShow=1,
    AliveCheckType=2,
    MutexID=120,
    MutexPriority=1,

    RoundMaxEffectedCount=1,
    ImpactLogic = 24,                --定期触发使用技能

    Param_1 = sk_assist,             -- 技能id
    Param_2 = 3,                    -- 周期类型（见24号逻辑）
    Param_3 = -1,                   -- 条件类型（见24号逻辑）
    Param_4 = -1,                   -- 条件参数
    Param_5 = -1,                   -- 条件参数
    Param_6 = -1,                   -- 是否立即生效（回合开始时用的技能，必须时立即释放，这个参数无用，回合结束使用技能，必须时顺序释放，这个参数无用）
    Param_7 = -1,                   -- 技能类型（-1，skillExId；1，skillIndex）
    Param_8 = -1,                   -- 周期参数
    Param_9 = -1,                   -- 周期参数

}

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_UseSkillRoundOver,
        },
        I_2 = {
            Impact = i_tihuan,
        },
        I_3 = {                        
            
            Impact = i_reducedam,
    }
    },
}
return sk_main


