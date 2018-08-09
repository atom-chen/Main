local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")



local i_actionbar = i_mk{
    ImpactLogic =6,                 --行动条增加减少           

    Param_1= "a1",                  --增减数（负数表示减少）
    Duration = 0,

}

local i_nextskill =i_mk{
    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a2",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,   
    Param_5 = "a3" ,        --额外使用的技能索引（该角色的第几个技能）
    -- Param_5 = sk_mk{
    --             Id=126601,--"a3",                 --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    --             Cooldown=-1,

    -- },                        
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 0,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,           --条件参数2

    Duration = 1,
    AliveCheckType = 2,
}

local sk_main = sk_mk{
    
    H_1=h_mk{
        IsAnimHit=0,
        TargetType=2,
            I_1={                           
                Impact =i_nextskill,
               
            }
    },
    H_2=h_mk{
        TargetType=3,
         I_1={                       --拉条
             Impact =i_actionbar,
            
         }
 }
    
 
} 
                      

return sk_main