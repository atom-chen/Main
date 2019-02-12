--血量满足特定条件时，触发子效果，不满足时，根据配置是否移除子效果，该效果被移除时，自动移除子效果
--参数：
--血量操作符（0，小于，1大于，2小于（上buff立即触发），3大于（上buff立即触发））
--血量百分比（10000）
--不满足时是否移除（0，否；1，是）
--子效果
--子效果
--子效果


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

require("class")

local oper = {
    le = 0,
    ge = 1,
    le_imm = 2,
    ge_imm = 3,
}

local maxChildImpact = 5

local Impact_28 = class("Impact_28",Impact)

--只在激活时生效一次
function Impact_28:OnActive()
    Impact_28.__base.OnActive(self)
    self.childImpacts = {}
    local param = self.tab.Param
    self.oper = param[1]
    self.percent = param[2]
    self.isDispell = param[3]
    self.childImpactIds = {}

    for i=1,maxChildImpact do
        local childId = param[3+i]
        if childId ~= -1 then
            table.insert( self.childImpactIds, childId )            
        end
    end

    if self.oper == oper.le_imm or self.oper == oper.ge_imm then
        self:CheckAddChild(self.recver:GetHP(),self.recver:GetMaxHP())
    end
end

function Impact_28:msgHPChange(hp,maxHP)
    
    --血量为0了，死亡
    if hp <= 0 then
        return
    end
    self:CheckAddChild(hp,maxHP)
end

function Impact_28:CheckAddChild(hp,maxHP)
    local check = false
    if self.oper == oper.le or self.oper == oper.le_imm then
        if (hp / maxHP) < (self.percent / 10000) then
            check = true
        end
    elseif self.oper == oper.ge or self.oper == oper.ge_imm then
        if (hp / maxHP) >= (self.percent / 10000) then
            check = true
        end
    end
    
    local recver = self.recver

    if not check then
        --不满足
        if self.hasTrigger then
            if self.isDispell == 1 then
                self:ClearChilds()
            end
            self.hasTrigger = false
        end
    else
        if self.hasTrigger then
            --已经激活过了
            return
        end

        for _,childId in ipairs(self.childImpactIds) do
            --给自己上子效果
            local childImpact = Impact.SendImpactToTarget(childId,recver,recver)
            if childImpact ~= nil and childImpact:IsActiveValid() then
                table.insert(self.childImpacts,childImpact)                
            end
        end
        self.hasTrigger = true
    end
end

function Impact_28:OnImpactFadeOut()
    --移除所有子buff
    self:ClearChilds()
end

function Impact_28:ClearChilds()
    local recver = self.recver    
    --移除
    for _,childImpact in ipairs(self.childImpacts) do
        recver:RemoveBuff(childImpact,true)
    end
    recver:NotifyBuffChange()
    self.childImpacts = {}
end

return Impact_28