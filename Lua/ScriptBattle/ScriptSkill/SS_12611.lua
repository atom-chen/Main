local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_clear = i_mk{
    ImpactLogic = 5,                --驱散buff/debuff

    Param_1 = 4,                    --被驱散的impact class
    Param_2 = -1,                   --subCLass
    Param_3 = -1,                   --tag
    Param_4 = 99,                   --驱散的数量
    Param_5 = -1,                   --是否提示(0不提示,其他提示)
    Duration = 0,
}

local i_getaround = i_mk{
    ImpactLogic = 15,                   --下回合立即行动 

    Param_1 = 1,                         --是否无视主角（1无视,0不无视）
    Param_2 = 7560,
    Duration = 0,

}






local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = 1,
        I_1 = {                                     --驱散
            Impact = i_clear,
            
        }
    },

    H_2 = h_mk{                                     --获得回合
        TargetType = 1,
        I_1 = {
            Impact = i_getaround,
        },

        
      
    },
}

return sk_main