local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")

local i_Burning = i_mk(sc.CommonBuffs.Burning)          --引用通用灼伤
      i_Burning.Duration = 2                            --修改持续回合 


--单体普通伤害
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
                Param_9 =1,              --参数，(1,包含，2，不包含)
                Param_10 = 911001,        --参数，impact id
                Duration = 1,
                AliveCheckType=2,
}





--单体普攻---
local sk_main = sk_mk{

    H_1 = h_mk{                 --单体伤害
    TargetType = 1,
    I_1 = {
        Impact = i_damage,
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