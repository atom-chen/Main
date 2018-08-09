local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage = i_mk{
    ImpactLogic = 0,
    ImpactClass= 2,
    Param_1 = "a1",
}

-- --冰冻额外追加玄冥扇-减速
-- local i_moreattack1 = i_mk{
--     ImpactLogic = 29,                     --逻辑说明：激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
--     ImpactClass= 2,                 
--     Param_1 = 2,                          --参数1：--类型（0，id；1，class；2，subClass）
--     Param_2 = 256,                        --参数2：--参数(根据类型区分具体意义)
--     Param_3 = 1,                          --参数3：--数量
--     Param_4 = i_damage,                       --参数4：--额外的效果impact id
-- }

-- --冰冻额外追加玄冥扇-冰冻
-- local i_moreattack2 = i_mk{
--     ImpactLogic = 29,                     --逻辑说明：激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
--     ImpactClass= 2,                 
--     Param_1 = 2,                          --参数1：--类型（0，id；1，class；2，subClass）
--     Param_2 = 256,                        --参数2：--参数(根据类型区分具体意义)
--     Param_3 = 1,                          --参数3：--数量
--     Param_4 = i_damage,                       --参数4：--额外的效果impact id
-- }




local i_iffroze1 = i_mk{
    ImpactLogic = 14,                 --额外释放冰
    Param_1 = 4,                      --4，指定id 参数，skillExId
    Param_2 = "a2",                 --参数2：技能参数
    Param_3 = 10000,                  --参数3：概率
    Param_4 = -1,                     --参数4：额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a2",                 --参数5：额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,                      --参数6：条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,                      --参数7：不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））
    Param_8 = 2,                      --参数8：条件类型       --2，目标包含(不包含)指定buff
    Param_9 = 1,                      --        --参数，(1,包含，2，不包含)
    Param_10 = 901001,                --        --参数，impact id
    Duration= 1,
    AliveCheckType=2,
    AutoFadeOutTag = 16,
    MutexID = 1339,
    MutexPriority=1,
    RoundMaxEffectedCount=5,
}

local i_iffroze2 = i_mk{
    ImpactLogic = 14,                 --额外释放减速
    Param_1 = 4,                      --4，指定id 参数，skillExId
    Param_2 = "a2",                 --参数2：技能参数
    Param_3 = 10000,                  --参数3：概率
    Param_4 = -1,                     --参数4：额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a3",                 --参数5：额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,                      --参数6：条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,                      --参数7：不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））
    Param_8 = 2,                      --参数8：条件类型       --2，目标包含(不包含)指定buff
    Param_9 = 1,                      --        --参数，(1,包含，2，不包含)
    Param_10 = 901001,                --        --参数，impact id
    Duration= 1,
    AliveCheckType=2,
    AutoFadeOutTag = 16,
    MutexID = 1339,
    MutexPriority=1,
    RoundMaxEffectedCount = 5,
}

local i_Frozen = i_mk(sc.CommonBuffs.Frozen)     --引用通用冰冻buff
      i_Frozen.Duration = 1                      --修改持续回合 


--普攻+冰冻901001
local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 2,
        IsAnimHit= 0 ,
        I_1 = {
            Impact = i_mk{
                        ImpactLogic = 10,           --逻辑说明：随机触发子效果
                        Param_1 = 1,                --参数1：随机类型，（0第一次激活随机，之后每次固定生效,1每次效果生效时随机）
                        Param_2 = i_iffroze1,        --参数2：impact1（可以填-1，代表有多少概率不触发）
                        Param_3 = 10,               --参数3：权重1
                        Param_4 = i_iffroze2,        --参数4：impact2
                        Param_5 = 10,               --参数5：权重2
                        RoundMaxEffectedCount = 5,
            }
        } 
    },
    H_2 = h_mk{
        I_1 = {
            Impact = i_damage,        --普攻
            RoundMaxEffectedCount = 5,
        },

        I_2 = { 
            RoundMaxEffectedCount = 5,
            Impact = i_Frozen,            --冰冻
            IsChanceRefix = 1,
        },

    },
}

return sk_main

