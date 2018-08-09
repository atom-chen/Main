--根据impact class驱散buff
--参数：
--1.被驱散的impact class
--2.subCLass
--3.tag
--4.驱散的数量
--5.是否提示(0不提示，其他提示)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_5 = class("Impact_5",Impact)

--只在激活时生效一次
function Impact_5:OnActive()
    Impact_5.__base.OnActive(self)
    local recver = self.recver
    local tab = self.tab

    local impactClass = tab.Param[1]
    local subClass = tab.Param[2]
    local tag = tab.Param[3]

    local count = tab.Param[4]
    local isNotify = tab.Param[5] ~= 0

    if impactClass < 0 then
        warn("impact class should bigger then 0")
        return
    end

    if not self:CanEffected() then
        return
    end

    local buffs = recver:GetBuffsByImpactClass(impactClass)
    if buffs == nil then
        return
    end

    if subClass > 0 then
        common.removec(buffs,function(b)
            return not b:IsSubClass(subClass)
        end)
    end

    if tag > 0 then
        common.removec(buffs,function(b)
            return b.tag ~= tag
        end)
    end

    local n = 0
    for _,buff in ipairs(buffs) do
        if count < 0 or n < count then
            if recver:RemoveBuff(buff,true) then
                n = n + 1
            end
        end
    end
    
    if n > 0 then
        if isNotify then
            recver:NotifyDispell(impactClass,n)        
        end

        if self.sender ~= nil then
            self.sender:OnDispellBuffs(impactClass,n)
        end

        --一次性通知
        recver:NotifyBuffChange()
        self:ImpactEffected()    
    end
end

return Impact_5