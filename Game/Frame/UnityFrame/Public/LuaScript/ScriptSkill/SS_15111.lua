local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")

require("BattleCore/SkillProcess/ScriptSkillParser")
local sc = require("ScriptSkill/ScriptSkillCommon")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local ImpactClass = require("BattleCore/Common/ImpactClass")


--祝融女 2技能用火焰融化一个目标，使其被动技能失效，同时获得灼伤效果


--封印被动（待补充）
-- local i_damage = i_mk{
--     ImpactClass=2,
--     ImpactLogic = 0,                --普通伤害

--     Param_1 = "a1",                 -- 攻击力百分比(无则填0   10000表示造成的100%攻击力)
--     Param_2 = 0,                    -- 额外固定值（0表示无附加掉血；如填60表示附加60点掉血）
--     Param_3 = 0,                    -- 同一技能相同impact多次命中衰减系数
-- }



local i_Burning = i_mk(sc.CommonBuffs.Burning)   --引用通用灼烧伤害
      i_Burning.Duration = 2                      --修改持续回合       






-- local i_Silent  = i_mk(sc.CommonBuffs.Silent )   --引用通用沉默伤害
--       i_Silent .Duration = 1                     --修改持续回合       



local sk_main = sk_mk{

    -- H_1 = h_mk{
    --     TargetType = 1 ,
    --     I_1 = {   
    --         IsChanceRefix = 1,                     
    --         Impact = i_Burning,                       --灼伤2回合
    --     },
    --     I_2 ={
    --         IsChanceRefix = 1,
    --         Impact =  i_Silent,                      --沉默1回合
    --     },
  
    --  EnvLimit = 0,
    -- OtherID = sk_mk{
        H_1 = h_mk{
            TargetType = 1,
            I_1 = {   
                IsChanceRefix = 1,                     
                Impact = i_Burning,                       --灼伤2回合
            },
        
        },
    -- EnvLimit = 1,   
    -- },

    
    
}

return sk_main