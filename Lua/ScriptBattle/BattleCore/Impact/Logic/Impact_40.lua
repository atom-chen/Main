--召唤
--参数
--1，召唤物id
--2，召唤区域
--3，召唤站位
--4，召唤规则（0有人则失败；1有人则替换；2无论是否有人都会召唤）
--5, 附加Impact
--6, 召唤数量(小于0表示用光位置（如果召唤类型是3任意位置召唤，则小于0无效），大于0表示指定数量)
--7，随机召唤启示id(-1表示没有随机)
--8, 随机召唤接受id(-1表示没有随机)

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local tabMgr = require("TabManager")
local BattleAreaType = require('BattleCore/Common/BattleAreaType')
local BattleSide = require("BattleCore/Common/BattleSide")
local BattleCommonData = require("BattleCore/Common/BattleCommonData")
local SkillLogicType = require("BattleCore/Common/SkillLogicType")
local SummonType = require("BattleCore/Common/SummonType")

require("class")

local Impact_40 = class("Impact_40",Impact)

local findRoleByPos = function(r,area,pos) return r.battlePosArea == area and r.battlePos == pos end

function Impact_40:OnActive()
    Impact_40.__base.OnImpact(self)

    self.summonId = self.tab.Param[1]
    self.summonArea = self.tab.Param[2]
    self.summonPos = self.tab.Param[3]
    self.spawnRule = self.tab.Param[4]
    self.addImpactId = self.tab.Param[5]
    self.summonCount = self.tab.Param[6]
    self.randStart = self.tab.Param[7]
    self.randEnd = self.tab.Param[8]

    if not self:CanEffected() then
        return
    end

    if self.sender == nil or not self.sender:IsAlive() then
        return
    end

    local battle = self.sender.battle
    if battle == nil then
        return
    end

    --获取技能信息
    if self.skillInfo == nil then
        return
    end

    local process = self.skillInfo.skillProcess
    if process == nil then
        return
    end

    local skillEx = process:GetTabEx()
    if skillEx == nil then
        return
    end

    if skillEx.LogicID ~= SkillLogicType.Summon then
        return
    end 

    local summonType = skillEx.LogicParam[1]
    local maxCount = skillEx.LogicParam[2]


    local summon = function()
        
        local summonId = self:GetSummonId()

        if maxCount > 0 then
            --获取已经召唤的数量
            if self.sender:GetSummonRoleCount() >= maxCount then
                return nil
            end
        end
    
        local summonRole = nil
        if summonType == SummonType.SummonPos then
            --我方召唤物召唤，找到一个空位召唤，否则失败
            local side = self.sender.side
            local battlePosArea = BattleAreaType.RedSummon
    
            if side == BattleSide.bs_Red then
                battlePosArea = BattleAreaType.RedSummon
            elseif side == BattleSide.bs_Blue then
                battlePosArea = BattleAreaType.BlueSummon
            end
    
            local validPos = -1
            for i=1,BattleCommonData.SUNMMON_POS_NUM do
                local pos = i-1
                local orgRole = battle:GetRole(findRoleByPos,battlePosArea,pos)
                if orgRole == nil then
                    validPos = pos
                    break
                end
            end
            if validPos == -1 then
                return nil
            end
            summonRole = self.sender:Summon(summonId,true,validPos,battlePosArea,self.spawnRule)
    
        elseif summonType == SummonType.RolePos then
            --符灵位召唤，根据接受者的区域召唤，接受者是我方，则在我方召唤，接受者是敌方，则在敌方召唤
            local skillProcess = self.skillInfo.skillProcess
            if skillProcess == nil then
                return nil
            end
            local recver = battle:GetRoleById(skillProcess.targetSelectedId)
            if recver == nil then
                return nil
            end
            local battlePosArea = recver.battlePosArea
            local validPosList = recver.battle:GetValidRolePos(battlePosArea)
            if validPosList == nil then
                return nil
            end
            --找到空位
            local validPos = -1
            for pos,_ in pairs(validPosList) do
                local orgRole = battle:GetRole(findRoleByPos,battlePosArea,pos)
                if orgRole == nil then
                    validPos = pos
                    break
                end
            end
            if validPos == -1 then
                return nil
            end
            summonRole = self.sender:Summon(summonId,true,validPos,battlePosArea,self.spawnRule)
    
        elseif summonType == SummonType.AnyPos then
            --任意召唤，不受限制
            summonRole = self.sender:Summon(summonId,true,self.summonPos,self.summonArea,self.spawnRule)
        else
            return nil
        end
    
        if summonRole ~= nil then
            if self.addImpactId ~= -1 then
                --发送附加impact，发送者是召唤者
                Impact.SendImpactToTarget(self.addImpactId,summonRole,self.sender)
            end
        end

        return summonRole
    end

    local succ = false
    if self.summonCount < 0 then
        if self.summonType == SummonType.AnyPos then
            --召唤一个
            if summon() ~= nil then
                succ = true
            end
        else
            for i=1,10 do
                local summonRole = summon()
                if summonRole == nil then
                    break
                else
                    succ = true
                end
            end
        end
    else
        for i=1,self.summonCount do
            local summonRole = summon()
            if summonRole == nil then
                break
            else
                succ = true
            end
        end
    end

    if succ then
        self:ImpactEffected()
    end
end

function Impact_40:GetSummonId()
    local summonId = self.summonId

    if self.randStart > 0 and self.randEnd > 0 then
        --随机
        
    end

    return summonId
end

return Impact_40