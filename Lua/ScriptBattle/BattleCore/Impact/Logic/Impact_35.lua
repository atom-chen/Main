--增减CD，有CD的技能，才会受这个效果影响
--参数
--0，技能索引（-1表示全部技能(1~3)）
--1，增减值(大于0增加CD，小于0减少CD)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_35 = class("Impact_35",Impact)

function Impact_35:OnActive()
    Impact_35.__base.OnActive(self)
    local index = self.tab.Param[1]
    
    local val = self.tab.Param[2]
    local recver = self.recver
    
    if not self:CanEffected() then
        return
    end


    if index == -1 then
        for i=1,3 do
            self:DoIncCooldown(i,val)
        end
    else
        self:DoIncCooldown(index,val)
    end

    self:ImpactEffected()            
end

function Impact_35:DoIncCooldown(index,val)
    local recver = self.recver
    
    if val < 0 then
        recver:DescCooldownByIndex(index,-val)
    else
        recver:IncCooldownByIndex(index,val)
    end
end

return Impact_35