
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--毕方1技能：造成毕方攻击320%的普通伤害。
--【人界效果】在攻击处于灼伤效果的敌人时，额外造成敌人当前生命15%的普通伤害。【妖界效果】在攻击处于灼伤效果的敌人时，额外造成敌人最大生命10%的固定伤害。

local i_damage  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_dangqianxueliang  = i_mk{
    Duration=0,
    MutexID=1170,
    MutexPriority=1,
    ImpactClass=2,
    ImpactLogic =34,              --根据受击者生命上限百分比直接造成伤害;无视防御，不会被修正

    Param_1 = "a2",               -- 百分比
    Param_2 = -1,                  -- 伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
    Param_3 = 0,                 -- -1生命上限，0当前血量
}

-- local i_shangxianxueliang  = i_mk{
--     Duration=0,
--     MutexID=1170,
--     MutexPriority=1,
--     ImpactClass=2,
--     ImpactLogic =34,              --根据受击者生命上限百分比直接造成伤害;无视防御，不会被修正

--     Param_1 = 1000,--"a1",               -- 百分比
--     Param_2 = -1,                  -- 伤害类型（-1普通伤害，11血祭伤害（扣血不播受击）
--     Param_3 = -1,                 -- -1生命上限，0当前血量
-- }

  

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,                     
        I_1 = {
            Impact = i_damage,
        },
         I_2 ={
             Impact =  i_mk{
                    IsShow=1,
                    ImpactLogic = 29,                           --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
                
                    Param_1 = 2,                                -- 类型（0，id；1，class；2，subClass）
                    Param_2 = 128,                              -- 参数(根据类型区分具体意义)
                    Param_3 = 1,                                -- 数量
                    Param_4 = i_dangqianxueliang,               --额外的效果impact id

             }               
         },
      
    },
 

}

return sk_main