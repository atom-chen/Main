
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--蜃珧1技能：普攻+概率沉睡1回合，【妖界】：如果攻击处于沉睡状态的敌人，增加40%攻击条

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--沉睡效果
local i_Sleep = i_mk(sc.CommonBuffs.Sleep)       --引用通用沉睡
      i_Sleep.Duration = 1                       --修改持续回合          





local i_nextskill = i_mk{
    Duration=1,
    AliveCheckType=2,
    MutexID=1091,
    MutexPriority=1,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a3",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a4",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = 2,            --条件类型（见17号逻辑）
    Param_9 = 1,            --条件参数1
    Param_10 = 906001,      --条件参数2


}


local sk_main = sk_mk{

  
    H_1 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
      
        I_2 = {                        --沉睡效果
            Chance = "a2",
            IsChanceRefix = 1,
            Impact = i_Sleep,
        },
    },
 
    H_2 = h_mk{                        
        TargetType = 2,
            I_1 = {
                Impact =  i_nextskill,
            },
        },


   
}

return sk_main