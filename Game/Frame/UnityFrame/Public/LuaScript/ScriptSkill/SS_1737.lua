local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--复活
local sk_main = sk_mk{

        H_1 = h_mk{
        TargetType = 1,
                I_1 = {
                    Impact =  i_mk{
                        Id = 1027011,                      
                        ImpactClass= 1,                           
                        Duration =1,
                        AliveCheckType = 2,
                        IsShow = 1,
                        ImpactLogic = -1,
                    },
                },
            },
            -- LogicID = 3,
            -- LogicParam0 = 8000,
            -- LogicParam1 =-1,        
}
return sk_main


