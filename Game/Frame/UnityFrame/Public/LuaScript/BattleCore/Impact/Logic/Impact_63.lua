--引爆DOT，立即触发DOT的效果
--参数：
--1.subClass
--2。增加buff回合（0无，-1减1回合，1加一回合，-2减2回合。。以此类推）

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")
local warn = common.mk_warn('Impact_63')

local Impact_63 = class("Impact_63",Impact)

function Impact_63:OnActive()
    self.subClass = self.tab.Param[1]
    self.roundOper = self.tab.Param[2]
end

function Impact_63:OnImpact()
    local buffs = self.recver:GetBuffsByImpactSubClass(self.subClass)
    if buffs == nil then
        return
    end

    local dirty = false
    for _,buff in ipairs(buffs) do

        --错误
        if buff == self then
            warn("buff is self!!!")
            return
        end

        --立即生效
        buff:OnImpact()
        --回合数增减
        if self.roundOper ~= 0 then
            if not buff:IncLiveTime(self.roundOper) then
                self.recver:RemoveBuff(buff,true)
            end
            dirty = true
        end
    end

    if dirty then
        self.recver:NotifyBuffChange()
    end
    self:ImpactEffected()        
end

return Impact_63