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



local i_SpeedReduce = i_mk(sc.CommonBuffs.SpeedReduce)     --引用通用缓速buff
      i_SpeedReduce.Duration = 1       

      
      

--普攻+缓速905001
local sk_main = sk_mk{
    H_1 = h_mk{

        I_1 = {
            Impact = i_damage,        --普攻
        },

        I_2 = { 
            Impact = i_SpeedReduce,            --减速
            IsChanceRefix = 1,
        }    
    },


}

return sk_main

