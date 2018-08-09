local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")




local i_nextattack =i_mk{
    MutexID=1180,
    MutexPriority=1,


    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 1,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = -1,          --技能参数
    Param_3 = 3000,          --概率
    Param_4 = 1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = -1,          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = -1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2
    RoundMaxEffectedCount=1,
    Duration = -1,
    AliveCheckType = 2,
}




local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType=2,
        I_1 = {
            Impact = i_nextattack,
        }
     
    }
}
return sk_main

