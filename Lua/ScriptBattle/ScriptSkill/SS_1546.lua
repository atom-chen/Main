
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--白骨-fuzhu-huodehuihe
  
-- --获得回合
local i_getround = i_mk{
    Duration=0,
    AliveCheckType=1,
    IsShow=1,
    MutexID=1546,
    MutexPriority=1,

    ImpactLogic =15,                 --下回合立即行动       

    Param_1= 1,                      --是否无视主角（1无视，0不无视）
    Param_2= 7560,                   --冒字的字典号

}

local sk_main = sk_mk{


    H_1 = h_mk{   
        IsAnimHit=0,                    --huodehuihe
        TargetType = 2,
        I_1 = {
            Impact = i_getround,
        },
    },


   
}

return sk_main