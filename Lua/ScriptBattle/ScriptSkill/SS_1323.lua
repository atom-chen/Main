local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

-- 变冰冻普攻
local i_change1 = i_mk{
    Duration = -1,               
    ImpactLogic = 36,     --逻辑说明：技能替换，激活时，替换指定的技能为新技能
    Param_1 = "a1",       --参数1：技能1替换id，不替换配-1
    MutexID = 1323,
    MutexPriority=1,
}

-- 变减速普攻
local i_change2 = i_mk{  
    Duration = -1,             
    ImpactLogic = 36,      --逻辑说明：技能替换，激活时，替换指定的技能为新技能
    Param_1 = "a2",        --参数1：技能1替换id，不替换配-1 
    MutexID = 1323,
    MutexPriority=1,
}


--换技能
local sk_1 = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                        ImpactLogic = 10,           --逻辑说明：随机触发子效果
                        Param_1 = 1,                --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                        Param_2 = i_change1,        --参数2：impact1（可以填-1，代表有多少概率不触发）
                        Param_3 = 10,               --参数3：权重1
                        Param_4 = i_change2,        --参数4：impact2
                        Param_5 = 10,               --参数5：权重2
            }
        }
    },
}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 2,
        I_1 = {
            Impact = i_mk{
                ImpactLogic = 24,                   --逻辑说明：定期触发使用技能
                Param_1 = sk_1,                     --换技能
                Param_2 =1,                         --参数2：周期类型
                Duration = -1,
                IsShow = 0,
            }
        },
        --周期触发技能

    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main