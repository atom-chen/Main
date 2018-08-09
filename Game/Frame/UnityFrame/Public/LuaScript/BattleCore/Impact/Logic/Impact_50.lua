--死亡前，使用一次技能，技能目标是击杀者，这个技能可以自救
--这个技能是立即使用的，哪怕配置了技能表现，因为死亡结算是立即的
--参数
--1,技能id

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_50 = class("Impact_50",Impact)

function Impact_50:msgBeforeDead(killerId)
    if not self:CanEffected() then
        return
    end
    
    --放一次技能
    local skillId = self.tab.Param[1]

    if skillId ~= -1 then
        if self.recver:CastSkill(skillId,killerId) then
            self:ImpactEffected()        
        end
    end
    
end

return Impact_50