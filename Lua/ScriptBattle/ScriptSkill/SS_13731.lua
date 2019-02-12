local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--玉圭2技能 回血+驱散
local i_Core = i_mk{
    ImpactLogic = 1,               --治疗
    
    Param_1 = 2,
    Param_2 = "a1",

}


local i_qusan= i_mk{
    ImpactLogic = 5,                --驱散debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subCLass
    Param_3 = -1,                    --tag
    Param_4 =  1,                   --驱散的数量
    Param_5 = -1,                   --是否提示(0不提示,其他提示)
    Param_6 = 64257,                  
    Duration = 0,
}   

local i_DefenceEnhance = i_mk(sc.CommonBuffs.DefenceEnhance)   --引用通用防御增加
      i_DefenceEnhance.Duration = 1                            --修改持续回合       

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 17 ,
        I_1 = {                         --驱散
            Impact = i_qusan,
        },
 
        I_2 = {                        
            Impact =i_Core,            --治疗
        },
        I_3 = {                        
            Impact =i_DefenceEnhance,    --防御强化
        }
    
    },
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放
}

return sk_main