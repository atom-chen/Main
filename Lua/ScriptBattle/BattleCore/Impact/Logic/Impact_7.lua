--免疫、抵消效果
--参数
--ImpactClass，0时不考虑，否则必须完全满足
--ImpactSubClass,0时不考虑，否则和ImpactClass同时匹配时，满足任意一个SubClass即可
--Count,次数，-1时无限次数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_7 = class("Impact_7",Impact)

-- function Impact_7:OnActive()
--     Impact_7.__base.OnActive(self)
--     self.count = 0
-- end

function Impact_7:IsImmue(impactTab)
    local tab = self.tab

    local impactClass = tab.Param[1]
    local impactSubClass = tab.Param[2]
    local count = tab.Param[3]

    if impactClass > 0 then
        if impactTab.ImpactClass ~= impactClass then
            return false
        end
    end

    if impactSubClass > 0 then
        if impactTab.ImpactSubClass & impactSubClass == 0 then
            return false
        end
    end

    --都过了，检查次数
    if count < 0 then
        return true
    end

    if not self:CanEffected() then
        return false
    end

    self.count = self.count or 0
    if self.count >= count then
        return false
    else
        self.count = self.count + 1

        if self.count >= count then
            self.recver:RemoveBuff(self)
        end

        self:ImpactEffected()
        
        return true
    end
end

return Impact_7