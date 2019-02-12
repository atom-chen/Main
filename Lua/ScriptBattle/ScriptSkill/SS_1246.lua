local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


local i_Stun = i_mk(sc.CommonBuffs.Stun)   --引用通用眩晕
      i_Stun.Duration = 1                      --修改持续回合          

--挂炸弹
local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType=6,
        TargetParam_1 = 2,
        TargetParam_2 = 0,
        I_1 = {
            Impact = i_mk{
                Id = 1243010,
                ImpactLogic =37,
                Param_1 = 1,
                Param_2= 334,
                Param_3= "a1",
                Param_4 = -1,
                Param_5 = 10000,
                Param_6 = 1243011,
                Param_7 = 10000, 
                Param_8 = i_Stun,
                Param_9 = 10000, 
            },
        },
    }, 
    IgnoreSelfState = 1, --处于眩晕等不可行动的状态时可释放    
}
return sk_main

