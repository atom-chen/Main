local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--河伯用水流助阵我方任一符灵，使其立即获得2个行动回合，并为其施加持续2回合的攻击强化效果。【人界效果】为其回复30%的生命。冷却时间4回合。

local i_getround1 = i_mk{

    Duration=0,
    AliveCheckType=1,

    ImpactLogic =15,                 --下回合立即行动       

    Param_1= 1,                      --是否无视主角（1无视，0不无视）
    Param_2= 7560,                   --冒字的字典号

}


local i_recovery = i_mk{
    Id=1583011,

    ImpactLogic =1,                         --治疗

    Param_1 = 3,                            --治疗类型(1攻击力,2治疗者血上限,3被治疗者血上限)
    Param_2 = "a1",                         --技能系数(10000表示造成的100%的治疗)
    Duration = 0,

}


--攻击强化效果
local i_AttackEnhance = i_mk(sc.CommonBuffs.AttackEnhance)     --引用攻击强化效果
      i_AttackEnhance.Duration = 2                             --修改持续回合          


local i_getround2 = i_mk{

    Duration=0,
    AliveCheckType=1,

    ImpactLogic =15,                 --下回合立即行动       

    Param_1= 1,                      --是否无视主角（1无视，0不无视）
    Param_2= 7560,                   --冒字的字典号

}



local sk_assist = sk_mk{
    H_1 = h_mk{

        TargetType = 2,
        I_1 ={
            
            Impact = i_getround2,               --获得回合
        },   
    }
   
}

local  i_UseSkillRoundOver = i_mk{       -- 回合结束后释放技能
    Duration=1,
   
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
        TargetType = 1,
       

        I_1 = {
            Impact = i_getround1,
            
        },
        I_2 = {
            Impact =  i_AttackEnhance,
            
        },
        I_3 = {
            Impact =  i_recovery,
            
        },

    },
    H_2 = h_mk{
        IsAnimHit=0,
        TargetType = 1,
       
        I_1 = {
            Impact = i_UseSkillRoundOver,
            
        },
    }
  


}

return sk_main