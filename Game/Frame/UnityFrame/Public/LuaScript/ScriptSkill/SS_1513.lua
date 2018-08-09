
require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

--祸斗觉醒3技能：【被动】祸斗使用两次技能后激活为可使用技能，对敌人造成祸斗攻击640%的普通伤害。根据激活之前的技能组合情况，将分别获得【黑炎】【赤炎】【幽炎】效果。冷却时间2回合。
--【人界效果】对生命低于50%的敌人额外使用1次伤害量为50%的当前技能。[A78C84]【妖界效果】对生命低于50%的敌人额外使用2次伤害量为50%的当前技能。[-]
--3-1击退

local i_damage = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a1",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_damage2  = i_mk{
    ImpactClass=2,
    ImpactLogic = 0,              --普通伤害

    Param_1 = "a2",               -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
    Param_2 = 0,                  -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
    Param_3 = -1,                 -- 同一技能相同impact多次命中衰减系数
}


local i_nextattack =i_mk{
    Duration=1,
    AliveCheckType=2,

    ImpactLogic = 14,        --使用技能时，条件额外使用技能

    Param_1 = 4,             --skilltype，1，普攻，2，任意主动，3指定class，4指定id
    Param_2 = "a3",          --技能参数
    Param_3 = 10000,          --概率
    Param_4 = -1,            --额外使用的技能索引（该角色的第几个技能）
    Param_5 = "a4",          --额外使用的技能（skillEx表id，参数4必须填-1才生效）
    Param_6 = 2,             --条件检查时机（1，使用技能前，2，使用技能后）
    Param_7 = 1,             --不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））     
    Param_8 = 1,            --条件类型（见17号逻辑）
    Param_9 = 2,            --条件参数1
    Param_10 = 5000,         --条件参数2


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
        I_2 ={
            Impact =  i_mk{
                   IsShow=1,
                   ImpactLogic = 29,                           --激活时检测，如果接受者指定buff存在超过指定数量时，触发一个额外的效果
               
                   Param_1 = 2,                                -- 类型（0，id；1，class；2，subClass）
                   Param_2 = 128,                              -- 参数(根据类型区分具体意义)
                   Param_3 = 1,                                -- 数量
                   Param_4 = i_damage2,                        --额外的效果impact id

            }               
        },
    }

   
}

return sk_main