--全体随机转移impact(注意技能目标需要是单体了，否则会反复触发全体转移)
--参数
--0,ImpactClass
--1,偷取数量(全局)
--2,目标类型（1敌方，2我方，发送者）

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')
local ImpactClass = require("BattleCore/Common/ImpactClass")

require("class")

local Impact_30 = class("Impact_30",Impact)

--只在激活时生效一次
function Impact_30:OnActive()
    Impact_30.__base.OnActive(self)
    local recver = self.recver
    local sender = self.sender
    local tab = self.tab

    if not self:CanEffected() then
        return
    end

    local impactClass = tab.Param[1]
    local count = tab.Param[2]
    local stealRecverType = tab.Param[3]
    if impactClass < 0 then
        warn("impact class should bigger then 0")
        return
    end


    local buffs = {}
    local allRoles = recver.battle:GetRoleAlies(recver)

    for _,roles in ipairs(allRoles) do
        local tmpBuffs = recver:GetBuffsByImpactClass(impactClass)
        for _,b in ipairs(tmpBuffs) do
            table.insert( buffs,b )
        end
    end

    if buffs == nil or #buffs == 0 then
        return
    end

    --计算接受者
    local stealRecver = nil
    if stealRecverType == 1 then
        local targets = sender.battle:GetRoleEnemies(sender)
        if targets == nil then return end
        common.removec(targets,SkillProcess.TargetFilter)
        stealRecver = sender.battle:RandomSelectOne(targets)
    elseif stealRecverType == 2 then
        local targets = sender.battle:GetRoleAlies(sender)
        if targets == nil then return end
        common.removec(targets,SkillProcess.TargetFilter)
        stealRecver = sender.battle:RandomSelectOne(targets)
    else
        stealRecver = sender
    end

    if stealRecver == nil then
        return
    end

    --洗牌
    recver.battle:Shuffle(buffs)

    local n = 0
    for _,buff in ipairs(buffs) do
        if count < 0 or n < count then
            if recver:RemoveBuff(buff,true) then
                --接受者，上buff
                Impact.SendImpactToTarget(buff.tab,stealRecver,sender)
                n = n + 1
            end
        end
    end

    if impactClass == ImpactClass.Negative then
        --负面效果
        stealRecver:NotifyHitText(5843)
        recver:NotifyHitText(5845)
    else
        --正面效果
        stealRecver:NotifyHitText(5844)
        recver:NotifyHitText(5842)
    end


    --一次性通知
    recver:NotifyBuffChange()
    self:ImpactEffected()            
end

return Impact_30