--引爆炸弹
--参数
--1，是否只能引爆自己的
--2，标签1（-1就是全部，其他就是指定标签）
--3，标签2

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local Impact_38 = class("Impact_38",Impact)


function Impact_38:OnImpact()
    Impact_38.__base.OnImpact(self)

    local onlyMy = self.tab.Param[1] == 1
    local tag1 = self.tab.Param[2]
    local tag2 = self.tab.Param[3]

    if not self:CanEffected() then
        return
    end

    --找到所有的炸弹
    local recver = self.recver
    if recver == nil or not recver:IsAlive() then
        return
    end

    local bombs = recver:GetBuffsByCond(function(buff)
        
        if buff.logicID ~= 37 then
            return false
        end

        if onlyMy and self.sender ~= buff.sender then
            return false
        end

        if (tag1 ~= -1 and buff.tag ~= tag1) 
            and (tag2 ~= -1 and buff.tag ~= tag2) then
                return false
        end

        return true
        
    end)

    local dirty = false
    for _,bomb in ipairs(bombs) do
        bomb:Blast()
        recver:RemoveBuff(bomb,true)
        dirty = true
    end

    if dirty then
        recver:NotifyBuffChange()
        self:ImpactEffected()
    end

end

return Impact_38