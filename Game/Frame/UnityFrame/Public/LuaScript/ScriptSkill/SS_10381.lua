local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祸斗攻击敌人，造成祸斗攻击640%的普通伤害，并且使其受到持续1回合的持续伤害效果。冷却时间2回合。

--普通伤害
local i_damage = i_mk{
    ImpactLogic = 0,                       --逻辑说明：普通伤害
    ImpactClass= 2,                        --效果分类（0，无类型，1增益，2减益（普通伤害），4减益）
    Param_1 = "a1",                        --攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Duration =0,
}



--持续伤害
local i_Poison = i_mk(sc.CommonBuffs.Poison)     --引用持续伤害效果
      i_Poison.Duration = 3                              --修改持续回合  



local i_nextattack =i_mk{
    Duration=1,
    AliveCheckType=2,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a2",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a3",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = -1,            --条件类型（见17号逻辑）
    Param_9 = -1,            --条件参数1
    Param_10 = -1,            --条件参数2


}

local sk_main = sk_mk{

    H_1 = h_mk{  
        IsAnimHit=0,               --使用第二击
        TargetType = 2,
        I_1 = {
            Impact = i_nextattack,
        },
    
    },

 
    H_2 = h_mk{                         --伤害
        TargetType = 1,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            IsChanceRefix = 1,
            Impact =i_Poison,
        }
    }

   
}

return sk_main
