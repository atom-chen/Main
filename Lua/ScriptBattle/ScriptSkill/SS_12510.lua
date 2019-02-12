
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--海东青2技能：海东青为自身施加持续2回合的攻击强化效果和狂暴效果 。【人界效果】使用【狂煞】后立即获得回合。冷却时间5回合。


--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)     --引用攻击强化效果
      i_AttackEnhance.Duration = 2                             --修改持续回合          


--狂暴效果
local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)     --引用狂暴效果
      i_CriticalEnhance.Duration = 2                               --修改持续回合          


-- --获得回合
local i_getround = i_mk{
    Id=1253010,

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
            Impact = i_AttackEnhance,
        },
        I_3 = {
            Impact = i_CriticalEnhance,
        },
    }
}

return sk_main