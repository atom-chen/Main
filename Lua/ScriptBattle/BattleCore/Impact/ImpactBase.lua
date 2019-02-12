
require("class")

local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local Utils = require("BattleCore/Impact/ImpactUtils")
local ImpactDirtyType = require("BattleCore/Common/ImpactDirtyType")

local Impact = class("ImpactBase")
local common = require("common")
local warn = common.mk_warn('Impact')
local debuglog = common.debuglog

function Impact:Init(tab,target,sender)
    self.tab = tab
    self.impactId = self.tab.Id
    self.recver = target
    self.sender = sender
    self.skillInfo = nil
    self.layerChilds = nil          --叠加的子buff
    self.dirtyType = nil
    self.buffId = nil               --作为buff存在时的唯一标示
    self.tag = self.tab.Tag
end

function Impact:Active()

    self.enable = true
    self:OnActive()

    if self.tab.Duration == 0 then
        --立即生效的impact
        self:DoImpact()
        return true
    end
    --持续生效的impact
    self.liveTime = self.tab.Duration
    self.duration = self.tab.Duration
    self.intervalTimer = 0
    return false
end

function Impact:IsForever()
    return self.tab.Duration == -1
end

function Impact:IsSync()
    return self.tab.IsShow == 1
end

function Impact:DoImpact()
    if self:IsActiveValid() then
        self:OnImpact()
    end
end

--进行计算
function Impact:OnActive()
end

function Impact:OnImpact()
end

function Impact:OnImpactFadeIn()
end

function Impact:OnImpactFadeOut(autoFadeOut)
end

function Impact:TickInterval()

    if not self:IsActiveValid() then
        return false
    end

    --触发间隔的impact
    local intervalTimer = self.intervalTimer
    intervalTimer = intervalTimer - 1
    if intervalTimer < 0 then
        self:DoImpact()
        intervalTimer = 0 --目前interval都是0
    end
    self.intervalTimer = intervalTimer
    self.ticked = true

end

function Impact:CheckAlive(aliveCheckType)
    if not self.isAlive then
        return false
    end

    --检测存活
    if self:IsForever() then
        return true
    end

    if self.tab.AliveCheckType ~= aliveCheckType then
        return true
    end

    if aliveCheckType == 3 and not self.ticked then
        return true
    end

    local liveTime = self.liveTime
    
    liveTime = liveTime - 1;
    self.liveTime = liveTime
    self.dirtyType = ImpactDirtyType.Update
    if liveTime <= 0 then
        return false
    end
    return true
end

function Impact:IncLiveTime(val)

    if self:IsForever() then
        return true
    end
    
    local liveTime = self.liveTime
    
    liveTime = liveTime + val
    self.liveTime = liveTime
    self.dirtyType = ImpactDirtyType.Update
    if liveTime <= 0 then
        return false
    end
    return true
end

function Impact:FadeOut(autoFadeOut)
    if not self.isAlive then
        return
    end

    self.isAlive = false
    self.dirtyType = ImpactDirtyType.Remove
    self:OnImpactFadeOut(autoFadeOut)
    
    if self.layerChilds ~= nil then
        for _,child in ipairs(self.layerChilds) do
            if child:IsActiveValid() then
                child:FadeOut()
            end
        end
    end

    if self.recver ~= nil then
        self.recver:OnImpactFadeOut(self,autoFadeOut)
    end

    --子buff机制
    if self.child ~= nil then
        self.child:FadeOut(autoFadeOut)
    end
end

function Impact:FadeIn()
    self.isAlive = true
    self.dirtyType = ImpactDirtyType.Add
    self:OnImpactFadeIn()

    if self.recver ~= nil then
        self.recver:OnImpactFadeIn(self)
    end
end

function Impact:RefixImpactSend(other)
end

function Impact:RefixImpactRecv(other)
end

function Impact:SendMessage(methodName,...)
    if not self:IsActiveValid() then
        return
    end

    --拦截一个消息
    if methodName == "bcRoundBegin" and self:GetRoundMaxEffected() > 0 then
        self:ClearRoundEffected()
    end
    
    local func = self[methodName]
    if func ~= nil then
        return func(self,...)
    end
end

function Impact:IsSubClass(subClass)
    return Utils.IsSubClass(self.tab,subClass)
end

function Impact:IsClass(impactClass)
    return Utils.IsClass(self.tab,impactClass)
end

function Impact:HasAutoFadeOutTag(tag)
    return self.tab.AutoFadeOutTag & tag > 0
end

function Impact:IsImmue(tab)
    return false
end

function Impact:RefixDamage(ret)
end

function Impact:IsImmueHit(hitTab)
    return false
end

function Impact:IsDOT()
    if self.tab == nil then
        return false
    end
    return self.tab.Duration > 0
end

--检查冲突
--是否冲突,优先级差(当前impact和要上的impact的优先级差值)
function Impact:CheckMutex(tab)
    if self.tab.MutexID ~= -1 and self.tab.MutexID == tab.MutexID then
        return true,self.tab.MutexPriority - tab.MutexPriority
    end
    return false,0
end

function Impact:IsDirective()
    if self.skillInfo == nil then
        return false
    end

    return self.skillInfo.isDirective
end

--是否是普攻效果
function Impact:IsNormalAtk()
    if self.skillInfo == nil then
        return false
    end

    local skillProcess = self.skillInfo.skillProcess

    if skillProcess == nil then
        return false
    end

    return skillProcess:IsNormalAttack()
end

--增加叠加
function Impact:AddLayer(impact)

    --是否超过上限
    if not self:CanAddLayer() then
        return false
    end

    self.layerChilds = self.layerChilds or {}

    table.insert( self.layerChilds,impact )

    self.dirtyType = ImpactDirtyType.Update
    return true
end

--是否是某个特定符灵
--类似的效果有很多，这里封装一个接口
function Impact:CheckIsSpecCard(role)

    if self.specRole ~= nil then
        return self.specRole == role
    end

    if self.specCardId == role.cardId then
        self.specRole = role
        --role也要标记一下，相同logic，触发了别人时，不能生效
        return true
    end

    return false
end

function Impact:GetLayerCount()
    if self.layerChilds == nil then
        return 1
    end

    return 1 + #self.layerChilds
end

function Impact:CanAddLayer()
    if self.tab.LayerID <= -1 then
        return false
    end
    
    return self:GetLayerCount() < self.tab.LayerMax
end

--增加同一技能同一效果计数
function Impact:IncImpactedCount(targetId)
    if self.skillInfo == nil then
        return
    end

    if self.skillInfo.skillProcess == nil then
        return
    end

    local skillProcess = self.skillInfo.skillProcess
    skillProcess.impactedCountTb = skillProcess.impactedCountTb or {}
    local tb = skillProcess.impactedCountTb[targetId] or {}
    skillProcess.impactedCountTb[targetId] = tb
    local impactedCount = tb[self.impactId] or 0
    tb[self.impactId] = impactedCount + 1
end

function Impact:HasEnvResist()
    return Utils.CalcEnvEffect(self.sender,self.recver) < 0
end

function Impact:HasEnvEnhance()
    return Utils.CalcEnvEffect(self.sender,self.recver) > 0
end

--获取同一技能，同一效果计数
function Impact:GetImpactedCount(targetId)
    if self.skillInfo == nil then
        return 0
    end

    if self.skillInfo.skillProcess == nil then
        return
    end

    local skillProcess = self.skillInfo.skillProcess


    if skillProcess.impactedCountTb == nil then
        return 0
    end

    local tb = skillProcess.impactedCountTb[targetId]
    if tb == nil then
        return 0
    end

    return tb[self.impactId]
end

--效果生效
function Impact:ImpactEffected()
    common.debuglog("impact effected",self.impactId,self.__name)
    if self.skillInfo ~= nil then
        local skillInfo = self.skillInfo
        --被动技能触发的Impact的，显示特效
        if skillInfo.IsPassive and skillInfo.skillExId ~= nil then
            if self.recver ~= nil then
                self.recver:NotifyPassiveEffected(skillInfo.skillExId)
            end
        end
    end

    --通知impact的发送者，impact生效了
    if self.sender ~= nil then
        self.sender:OnSendImpactEffected(self)
    end

    self:IncRoundEffectedCount()
    self:SetCD()
end

function Impact:SkillWrapBegin(skilId,casterId,targetId)
    local battle = self.recver.battle
    return {
        type = BattleEventType.UseSkill,
        skillEvent = {
            targetID = targetId,
            usedSkillID = skilId,
            casterID = casterId,
        },
    }
end

function Impact:SkillWrapAddHit(e)
    if e.skillEvent == nil then
        return
    end

    e.skillEvent.hits = e.skillEvent.hits or {}
    local hit = {hitResults={}}
    table.insert(e.skillEvent.hits,hit)
    return hit
end

function Impact:SkillWrapHitRetBegin()
    local battle = self.recver.battle
    battle:NewEventGroup()
end

function Impact:SkillWrapHitRetEnd(hit,targetId)
    local battle = self.recver.battle
    local hitRet = {
        targetID = targetId,
        events = battle:PopEventGroup()
    }
    table.insert(hit.hitResults,hitRet)
end

function Impact:SkillWrapEnd(e)
    local battle = self.recver.battle
    battle:AddEvent(e)
end

--效果是否可以生效
function Impact:CanEffected()
    if self:IsCDing() then
        debuglog("impact can not effected.","cding")
        return false
    end

    if not self:IsActiveValid() then
        return false
    end

    if self.roundEffectedCount and self.roundEffectedCount >= self:GetRoundMaxEffected() then
        debuglog("impact can not effected.","Effected Count limit",self:GetRoundMaxEffected(),self.roundEffectedCount)
        return false
    end

    return true
end

--一回合内触发上限
function Impact:GetRoundMaxEffected()
    return self.tab.RoundMaxEffectedCount
end

function Impact:ClearRoundEffected()
    self.roundEffectedCount = 0
end

function Impact:IncRoundEffectedCount()
    if self:GetRoundMaxEffected() > 0 then
        self.roundEffectedCount = self.roundEffectedCount or 0
        self.roundEffectedCount = self.roundEffectedCount + 1
    end
end

function Impact:SetCD()
    if self.tab.CooldownId ~= -1 and self.recver ~= nil and self.recver.isValid then
        self.recver:BeginCooldown(0,self.tab.CooldownId,self.tab.Cooldown)
    end
end

function Impact:IsCDing()
    if self.tab.CooldownId == -1 then
        return false
    end

    if self.recver == nil then
        return false
    end

    return self.recver:IsCooldowning(self.tab.CooldownId)
end

--是否被动效果
function Impact:IsPassiveImpact()
    return self.tab.IsPassiveImpact ~= nil and self.tab.IsPassiveImpact == 1
end

function Impact:IsActiveValid()
    if self.tab.Duration ~= 0 and not self.isAlive then
        return false
    end
    
    if not self.enable then
        return false
    end

    if self:IsPassiveImpact() then
        if self.sender == nil then
            return false
        end

        if self.sender:IsPassiveDisable() then
            return false
        end
    end

    return true
end

function Impact:SetEnable(val)
    self.enable = val
end

function Impact:GetSkillTab()
    if self.skillInfo ~= nil and self.skillInfo.skillProcess ~= nil then
        return self.skillInfo.skillProcess.tabEx
    end
	return nil
end

return Impact