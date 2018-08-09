--免疫hit
--参数
--次数,-1表示无限
--impactClass，0时不考虑，非0时，hit只要包含了这类impact就免疫

local Impact = require("BattleCore/Impact/Impact")

require("class")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local Utils = require("BattleCore/Impact/ImpactUtils")

local Impact_16 = class("Impact_16",Impact)

function Impact_16:IsImmueHit(hitTab)
    local count = self.tab.Param[1]
    local class = self.tab.Param[2]

    if not self:CanEffected() then
        return false
    end

    local shouldImm = false
    
    if class <= 0 then
        shouldImm = true
    else
        for _,impactId in ipairs(hitTab.Impact) do
            if Utils.IsClass(impactId,class) then
                shouldImm = true
                break
            end
        end
    end

    if not shouldImm then
        return false
    end


    if count < 0 then
        self:ImpactEffected()                
        return true
    end

    self.count = self.count or 0
    self.count = self.count + 1
    
    if self.count >= count then
        self.recver:RemoveBuff(self)
    end
    self:ImpactEffected()            
    return true
end

return Impact_16