--死亡前,对击杀者使用一次技能
--参数
--1,技能ID

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')


require("class")

local Impact_22 = class("Impact_22",Impact)

-- function Impact_22:OnActive()
--     Impact_22.__base.OnActive(self)
-- end

function Impact_22:msgDead(killerId)
    local recver = self.recver

    if not self:CanEffected() then
        return
    end

    if killerId == -1 then
        return
    end

    --平行使用
    --强制使用
    --目标需要存活
    local target = recver.battle:GetRoleById(killerId)
    if target == nil then
        return
    end
    recver:CastSkill(self.tab.Param[1],killerId)
    self:ImpactEffected()
    
end

return Impact_22