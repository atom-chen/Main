local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

-- ----------------------------------------------------1层---------------------------------------------------------------------------------------------------------------------------
-- --伤害加深
-- local i_enhance = i_mk{
--     ImpactLogic = 4,                        --逻辑说明：属性修正
--     Param_1 = 6,                            --参数1：战斗属性类型1(无则填 - 1 对应战斗属性的枚举类型 )
--     Param_2 = "a2",                         --参数2：类型1修正的值，固定值修正(无则填 0 正数增加 负数减小)
--     Param_3 = 0,                            --参数3：类型1修正的值，百分比修正(无则填 0 正数增加 负数减小)
--     Duration =1,                            -- 
--     AliveCheckType=2,                       --存活结算类型（1，回合开始；2，回合结束；3，下回合结束）
--     AutoFadeOutTag=16,                      --自动移除标签（0不自动移除，1受击，2发送者死亡，4自己的回合使用技能后移除，8任意回合使用技能后移除，16发送这个impact的技能结束，32发送者行动结束，64发送者回合结束），加起来表示任意条件满足就移除
-- }

-- --根据不同层数触发不同伤害加强
-- local i_check = i_mk{
--     ImpactLogic = 29,                      --逻辑说明：激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
--     Param_1 = 0,                           --类型（0，id；1，class；2，subClass）
--     Param_2 = 1274010,                     --参数2：--参数(根据类型区分具体意义)
--     Param_3 = 1,                           --参数3：--数量
--     Param_4 = i_enhance,                   --参数4：--额外的效果impact id
--     Duration =0,
-- }
--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}

local i_check = i_mk{
    ImpactLogic = 57,                      --逻辑说明：根据buff数量增加属性修正
    Param_1 = 1,                           --参数1：buff计数类型（1，impactId；2，class；3，subClass）
    Param_2 = 1274010,                     --参数2：参数(根据类型区分具体意义)
    Param_3 = 6,                           --参数3：属性类型
    Param_4 = 4000,                        --参数4：固定值修正
    Param_5 = 0,                           --参数5：百分比修正
    Duration =1,
    AliveCheckType=2,                      --存活结算类型（1，回合开始；2，回合结束；3，下回合结束）
    AutoFadeOutTag=16,                     --自动移除标签（0不自动移除，1受击，2发送者死亡，4自己的回合使用技能后移除，8任意回合使用技能后移除，16发送这个impact的技能结束，32发送者行动结束，64发送者回合结束），加起来表示任意条件满足就移除
}



--攻击单体敌人，伤害随着场上死亡人数增加而增加
local sk_main = sk_mk{

    H_1 = h_mk{                             --检测层数，伤害加深
        TargetType=2,
        IsAnimHit = 0,
        I_1 = {
            Impact = i_check,
        },
    },
    
    H_2 = h_mk{                              --普通伤害
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
    },
    H_3 = h_mk{                              --普通伤害
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
    },
    H_4 = h_mk{                              --普通伤害
        TargetType=1,
        I_1 = {
            Impact = i_damage,
        },
    },


}

return sk_main