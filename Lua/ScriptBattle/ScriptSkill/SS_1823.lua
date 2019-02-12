local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_damage1  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_biaoqian  = i_mk{
    Id = 1823010,
    Duration =1,
    AliveCheckType=2,                      --存活结算类型（1，回合开始；2，回合结束；3，下回合结束）
    AutoFadeOutTag=16,     
}

local i_check = i_mk{
    ImpactLogic = 57,                      --逻辑说明：根据buff数量增加属性修正
    Param_1 = 1,                           --参数1：buff计数类型（1，impactId；2，class；3，subClass）
    Param_2 = 1823010,                     --参数2：参数(根据类型区分具体意义)
    Param_3 = 7,                           --参数3：属性类型
    Param_4 = -1000,                        --参数4：固定值修正
    Param_5 = 0,                           --参数5：百分比修正
    Duration = 1,
    MutexID = 1823,
    MutexPriority = 1,
    AliveCheckType= 2,                      --存活结算类型（1，回合开始；2，回合结束；3，下回合结束）
    AutoFadeOutTag= 16,                     --自动移除标签（0不自动移除，1受击，2发送者死亡，4自己的回合使用技能后移除，8任意回合使用技能后移除，16发送这个impact的技能结束，32发送者行动结束，64发送者回合结束），加起来表示任意条件满足就移除
}

local i_tuitiao = i_mk{
    ImpactClass=4,
    ImpactLogic = 6,                --行动条增减  
    Param_1 = -200,                 --参数：增减数（负数表示减少）
}

local sk_main = sk_mk{
    H_1 = h_mk{
        TargetType = 4,
        I_1 = {
            Impact = i_check,
        },
    },
    H_2 = h_mk{
        TargetType = 1,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_biaoqian,
        },
    },
    H_3 = h_mk{
        TargetType = 6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_biaoqian,
        },
        I_3 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =2, 
                    Param_4 =i_tuitiao,           
                },
        },
    },
    H_4 = h_mk{
        TargetType = 6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_biaoqian,
        },
        I_3 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =2, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_4 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =3, 
                    Param_4 =i_tuitiao,           
                },
        },
    },
    H_5 = h_mk{
        TargetType = 6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_biaoqian,
        },
        I_3 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =2, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_4 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =3, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_5 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =4, 
                    Param_4 =i_tuitiao,           
                },
        },
    },
    H_6 = h_mk{
        TargetType = 6,
        TargetParam_1 = 1,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_damage1,
        },
        I_2 = {
            Impact = i_biaoqian,
        },
        I_3 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =2, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_4 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =3, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_5 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =4, 
                    Param_4 =i_tuitiao,           
                },
        },
        I_6 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有超过4层效果，造成晕眩1回合
                    Param_1 =0,   
                    Param_2 =1823010,
                    Param_3 =5, 
                    Param_4 =i_tuitiao,           
                },
        },
    },
}
return sk_main


