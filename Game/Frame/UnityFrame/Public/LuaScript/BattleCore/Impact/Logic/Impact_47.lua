--特定buff持续时间增加/减少N回合
--参数
--1，impact class
--2, 回合变化值(正数表示增加，负数表示减少)
--3，影响的数量(小于等于0表示全部，大于0表示最大数量)
--4, impact tag

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_47 = class("Impact_47",Impact)

function Impact_47:OnActive()
    Impact_47.__base.OnImpact(self)
    local param = self.tab.Param
    local impactClass = param[1]
    local val = param[2]
    local maxCount = param[3]
    local tag = param[4]

    if not self:CanEffected() then
        return
    end

    if impactClass < 0 then
        return
    end

    local recver = self.recver
    local buffs = recver:GetBuffsByImpactClass(impactClass)
    if buffs == nil or #buffs == 0 then
        return
    end

    if tag ~= -1 then
        --把tag不等的移除掉
        common.removec(buffs,function(b) return b.tag ~= tag end)
    end

    local n = 0
    for _,buff in ipairs(buffs) do
        if ((maxCount > 0 and n <= maxCount) or (maxCount < 0)) and buff.isAlive then
            if not buff:IncLiveTime(val) then
                recver:RemoveBuff(buff,true)
            end
            n = n + 1
        end
    end

    if n > 0 then
        recver:NotifyBuffChange()
        self:ImpactEffected()        
    end
end

return Impact_47