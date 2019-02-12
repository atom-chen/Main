
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祝融女3技能觉醒后在满足条件下触发的额外攻击-攻击目标，如果目标处于灼伤状态，触发额外伤害巨大重击

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}

--攻击力提升
local i_attackup = i_mk{
    MutexID=1512,
    MutexPriority=1,

    ImpactLogic = 4,               
    Param_1 = 7,                    
    Param_2 = -5000,                    
    Param_3 = 0,
    Duration = -1,
    AliveCheckType=1,
    AutoFadeOutTag=1,
}

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,                     
        
         I_1 ={
             Impact =  i_mk{
                    Duration = 0,
                    ImpactLogic = 29,                           --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
                
                    Param_1 = 2,                                -- 类型（0，id；1，class；2，subClass）
                    Param_2 = 128,                              -- 参数(根据类型区分具体意义)
                    Param_3 = 1,                                -- 数量
                    Param_4 = i_attackup,                        --额外的效果impact id

             }               
         },
         I_2 = {
            Impact = i_damage,
        },
      
    },
 

}

return sk_main