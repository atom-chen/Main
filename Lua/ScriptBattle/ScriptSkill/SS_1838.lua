
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--山鬼3技能：山鬼驱使赤豹发出低吼，鼓舞1名随机队友，并为其施加持续1回合的狂暴效果和持续1回合的攻击强化效果，
--被鼓舞的队友将使用普通攻击和山鬼一同攻击敌人，山鬼对敌人造成自身攻击380%的普通伤害。冷却时间5回合。



local i_CriticalEnhance = i_mk(sc.CommonBuffs.CriticalEnhance)      --引用通用狂暴
      i_CriticalEnhance.Duration = 2                                --修改持续回合  


local i_xiezhan  = i_mk{
    Duration=1,
    AliveCheckType=2,
    AutoFadeOutTag=64,
    MutexID=1838,
    MutexPriority=1,

    ImpactLogic = 19,               --特定使用技能时，协同一起使用技能

    Param_1 = -2,                   -- cardId（特定符灵，-1时，任意队友攻击都可协同攻击,-2表示发送者使用技能时，协同攻击）
    Param_2 = 5,                    -- skillClass（技能类型）
    Param_3 = 10000,                -- 概率
    Param_4 = 1,                   --额外使用的技能索引（该角色的第几个技能）
    Param_5 = -1,                   --额外使用的技能额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 1,                   --一回合内激活的次数
}

local i_nextattack =i_mk{
    Duration=1,
    AliveCheckType=2,
    IsShow=0,
    MutexID=1839,
    MutexPriority=1,
    RoundMaxEffectedCount=1,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a1",          --技能参数
    Param_3 = 10000,         --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a2",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2

}


local sk_main = sk_mk{

    H_1 = h_mk{                         --加暴击
    IsAnimHit=0,
    TargetType = 3,
        I_1 = {
            Impact = i_CriticalEnhance,
        },
    },

    H_2 = h_mk{                         --协战
        TargetType = 5,
        TargetParam_1 = 2,
        TargetParam_2 = -1,
        TargetParam_3 = 1,
        TargetParam_4 = 1,
        I_1 = {
            Impact = i_xiezhan,
        },
    },

    H_3 = h_mk{                         --协战
        TargetType = 2,

    I_1 = {
        Impact = i_nextattack,
    }
},


}

return sk_main