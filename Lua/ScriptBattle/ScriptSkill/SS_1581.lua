local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--河伯用河水祈福，为我方全体消除所有的减益效果。并为其施加持续2回合的急速效果。冷却时间4回合。


local i_clear = i_mk{
    ImpactLogic = 5,                --驱散buff/debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subCLass
    Param_3 = -1,                   --tag
    Param_4 =  -1,                   --驱散的数量
    Param_5 =  1,                   --是否提示(0不提示，其他提示)
    Duration = 0,
}



--急速效果
local i_SpeedEnhance = i_mk(sc.CommonBuffs.SpeedEnhance)     --引用缓速效果
      i_SpeedEnhance.Duration = 2                            --修改持续回合          




local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 3,
        I_1 = {
            Impact = i_clear,
            
        },
        I_2 = {
            Impact = i_SpeedEnhance,
            
        }
    }
  


}

return sk_main