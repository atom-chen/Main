local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Frozen = i_mk(sc.CommonBuffs.Frozen)     --引用通用冰冻buff
      i_Frozen.Duration = 1                              --修改持续回合 


--群体普通伤害
local i_damage = i_mk{
    ImpactLogic = 0, 
    ImpactClass= 2,
    Param_1 = "a1",               --参数1：攻击力百分比(无则填0   10000表示造成的100%攻击力)
}

--14号逻辑
local i_fourteen = i_mk{
                ImpactLogic = 14,         
                Param_1 = 4,             --参数1：skilltype
                Param_2 ="a2",           --参数2：技能参数
                Param_3 =10000,          --参数3：概率
                Param_4 =-1,             --参数4：额外使用的技能索引（该角色的第几个技能）     
                Param_5 ="a3",           --参数5：额外使用的技能（skillEx表id，参数4必须填-1才生效）
                Param_6 =2,              --参数6：条件检查时机（1，使用技能前，2，使用技能后）
                Param_7 =1,              --参数7：不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））   
                Param_8 =2,              --参数8：条件类型
                Param_9 =2,              --2，目标包含(不包含)指定buff
                Param_10 = i_Frozen,        --参数，impact id
                Duration = 1,
                AliveCheckType=2,

}





--群体普攻---
local sk_main = sk_mk{

    H_1 = h_mk{                 --群体伤害
    TargetType = 4,
        I_1 = {
            Impact = i_damage,
        },
        I_2 = {
            Impact =  i_mk{
                Id = 1322010,
                Duration = 1,
                ImpactLogic = -1,   
            }
        },

        I_3 = {                  --概率冰冻
            Chance = "a4", 
            Impact = i_Frozen,
            IsChanceRefix = 1,
        },

        I_4 = {
            Impact = i_mk{
                    ImpactLogic = 29,              --目标身上有冰冻，则用加标记
                    Param_1 =2,   
                    Param_2 =256,
                    Param_3 =1, 
                    Param_4 =1322011,           
                },
        },
},


    H_2 = h_mk{                 --上14号逻辑
        TargetType = 2,
        IsAnimHit = 0,
        I_1 = {
        Impact = i_fourteen,
        }
    },
}

return sk_main