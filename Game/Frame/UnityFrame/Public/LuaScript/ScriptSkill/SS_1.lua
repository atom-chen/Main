--local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_damage = i_mk{
    ImpactLogic = 0,

    Param_1 = 10000,
    Param_2 = 10,
    Param_3 = 1000,
}

--眩晕
local ib_stun = ib_mk{
    Id = 1040011,
    
    ImpactLogic = -1,
    Duration = 5,
}

local ib_bleed = ib_mk{
    ImpactLogic = 0,

    Param_1 = 1000,
    Param_2 = 10,
    Duration = -1
}

local ib_heal = ib_mk{
    ImpactLogic = 17,

    Duration = -1,

    Param_1 = 10000,

    Param_2 = sk_mk{
        H_1 = h_mk{
            I_1 = {
                Impact = i_mk{
                    ImpactLogic = 1,
                    Param_1 = 1,
                    Param_2 = 50000,
                }
            }
        }
    },

    Param_3 = 1,
    Param_4 = 1,
    Param_5 = 2,
}

chain_mk{
    ib_stun,
    --ib_bleed,
    --ib_heal,
}

local sk_main = sk_mk{

    H_1 = h_mk{
        TargetType = ImpactTargetType.SkillTarget,
        I_1 = {
            Impact = i_damage,
        }
    },

    H_2 = h_mk{

        Tmp_1 = ib_mk{
            ImpactLogic = 4,
    
            Param_1 = 101,
            Param_2 = 1000,
            Param_3 = 0,
        },

        TargetType = ImpactTargetType.SkillTarget,
        I_1 = {
            Impact = i_damage,
        },
        -- --概率附加眩晕
        -- I_2 = {
        --     Chance = "a3",
        --     IsChanceRefix = 1,
        --     Impact = "a4",
        -- },
        -- I_3 = {
        --     Impact = i_mk{
        --         Id = "a5",

        --         LogicID = 19,
        --     }
        -- }
    },
}

return sk_main