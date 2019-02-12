local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")





local i_boomdot  = i_mk{
    ImpactLogic = 63,              --爆毒

    Param_1 =64,                   -- 参数1：SubClass
    Param_2 = -1,                  -- 参数2：增加buff回合（0无，-1减1回合，1加一回合，-2减2回合。。以此类推）
    ImpactClass = 4,
}



local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=4,
        I_1 = {
            Impact = i_boomdot,
        }  
    },
  
}
return sk_main


