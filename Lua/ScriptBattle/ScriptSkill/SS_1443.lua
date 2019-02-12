
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--朽扉觉醒3技能

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--防御降低效果
local i_DefenceReduce = i_mk(sc.CommonBuffs.DefenceReduce)       --引用通用防御降低
      i_DefenceReduce.Duration = 2                               --修改持续回合          



local i_tuitiao = i_mk{
    ImpactClass=4,
    ImpactLogic =6,                 --行动条增加减少           

    Param_1= "a2",                  --增减数（负数表示减少）
    Duration = 0,

}


--如果目标有buff，则退条
local i_jiancebuff1  = i_mk{
    Duration= 0,

    ImpactLogic = 29,              -- 激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果

    Param_1 = 1,                   -- 类型（0，id；1，class；2，subClass）
    Param_2 = 1,                   -- 参数(根据类型区分具体意义)
    Param_3 = 1,                   -- 数量
    Param_4 = i_tuitiao,           -- 额外的效果impact id

}

-- --标记
-- local i_biaoji  = i_mk{
--     Id= 1443011,
--     Duration= -1,
--     AutoFadeOutTag=16,
--     MutexID=1443,
--     MutexPriority=2,

--     ImpactLogic = -1,              


-- }

--如果目标有buff，则上标记
local i_jiancebuff2  = i_mk{
    Duration= 0,
    
    ImpactLogic = 29,              -- 激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果

    Param_1 = 1,                   -- 类型（0，id；1，class；2，subClass）
    Param_2 = 1,                   -- 参数(根据类型区分具体意义)
    Param_3 = 1,                   -- 数量
    Param_4 = i_mk{
        Id= 1443011,
        Duration= -1,
        AutoFadeOutTag= 64,
        MutexID=1443,
        MutexPriority=2,
    
        ImpactLogic = -1,              
    },            -- 额外的效果impact id
}

local i_nextattack =i_mk{
    MutexID=1443,
    MutexPriority=1,


    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a3",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a4",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = -1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = 2,            --条件类型（见17号逻辑）
    Param_9 = 2,            --条件参数1
    Param_10 = 1443011,          --条件参数2
    RoundMaxEffectedCount=1,
    Duration = -1,
    AliveCheckType = 2,
}



local sk_main = sk_mk{

    H_1 = h_mk{
        IsAnimHit=-0,
        TargetType=2,
       
        I_1 = {
            Impact = i_nextattack,
        }
     
    },
 
    H_2 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact = i_jiancebuff1,
        },
        I_3 = {
            Impact = i_jiancebuff2,
        },
        I_4 = {                        --防御降低效果
            Chance = "a5",
            IsChanceRefix = 1,
            Impact = i_DefenceReduce,
        }
    },
 
    
  
   
}

return sk_main





