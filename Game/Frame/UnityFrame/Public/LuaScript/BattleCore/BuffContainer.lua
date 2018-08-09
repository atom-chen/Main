require("class")

local BuffContainer = class("BuffContainer")
local common = require('common')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local AutoFadeOutTag = require("BattleCore/Common/AutoFadeOutTag")
local ImpactDirtyType = require("BattleCore/Common/ImpactDirtyType")
local ImpactClass = require("BattleCore/Common/ImpactClass")
local CalcParams = require("BattleCore/CalcParams")

local warn = common.mk_warn('BuffContainer')

--Impact结算时间点
local AliveCheckType = {
    RoundBegin = 1,
    RoundEnd = 2,
    NextRoundEnd = 3,
}

function BuffContainer:Init(owner,idBase)
    self.buffs = {}
    self.owner = owner
    self.idBase = idBase or 0
end

local isBuffAlive = function(buff)
    return not buff.isAlive
end

function BuffContainer:RoundBegin()
    self:TickInterval()
    self:CheckBuffAlive(AliveCheckType.RoundBegin)
end

function BuffContainer:RoundEnd()
    self:CheckBuffAlive(AliveCheckType.RoundEnd)
    self:CheckBuffAlive(AliveCheckType.NextRoundEnd)
end

--间隔触发
function BuffContainer:TickInterval()
    local buffs = self.buffs

    for _,buff in ipairs(buffs) do
        buff:TickInterval()
    end

    --self:NotifyBuffChange()
end

--计算buff
function BuffContainer:CheckBuffAlive(aliveCheckType)
    local buffs = self.buffs
    local dirty = false

    for _,buff in ipairs(buffs) do
        
        if buff.isAlive then
            if not buff:CheckAlive(aliveCheckType) then
                buff:FadeOut(true)
            end
            dirty = true            
        end

    end
 
    if dirty then
        self:NotifyBuffChange()
    end
    --删除非活跃的
    common.removec(buffs,isBuffAlive)
end

function BuffContainer:NotifyBuffChange()
    if self.NeverNotify then
        return true
    end

    local infos
    for _,buff in ipairs(self.buffs) do
        if buff.dirtyType ~= nil and buff:IsSync() then
            local info = {}
            info.updateType = buff.dirtyType
            info.id = buff.buffId
            
            if buff.dirtyType == ImpactDirtyType.Add or buff.dirtyType == ImpactDirtyType.Update then
                local senderID = nil
                if buff.sender ~= nil then
                    senderID = buff.sender.id
                end
                --更新
                info.info = {
                    impactID = buff.impactId,
                    roundCount = buff.liveTime,
                    layerCount = buff:GetLayerCount(),
                    duration = buff.duration,
                    senderID = senderID,
                    id = buff.buffId,
                }
            end

            infos = infos or {}
            table.insert( infos, info )
            buff.dirtyType = nil
        end
    end

    if infos ~= nil and #infos > 0 then
        self.seq = self.seq or 1

        self.owner.battle:AddEvent({
            type  = BattleEventType.ImpactsChange,
            impactsChangeEvent = {
                ownerID = self.owner.id,
                seq = self.seq,
                infos = infos,
            }
        })
    
        self.seq = self.seq + 1
    end

end

function BuffContainer:GetBuffSync(isSyncAllAttrs)
    --warn(alwaysSync)
    local infos = {}
    for _,buff in ipairs(self.buffs) do
        if buff.isAlive and (buff:IsSync() or isSyncAllAttrs) then
            local senderID = nil
            if buff.sender ~= nil then
                senderID = buff.sender.id
            end
            table.insert(infos,{
                impactID = buff.impactId,
                roundCount = buff.liveTime,
                layerCount = buff:GetLayerCount(),
                senderID = senderID,
                id = buff.buffId,
            })
        end
    end
    return infos
end

function BuffContainer:SetNeverNotify()
    self.NeverNotify = true
end

function BuffContainer:AddBuff(buff,notNotify)
    if buff == nil then
        return
    end

    local buffOrDebuff = ImpactClass.Positive ~ ImpactClass.Negative
    if buff:IsClass(buffOrDebuff)then
        --buff、debuff超过上限
        if self:CountBuffsByImpactClass(buffOrDebuff) >= CalcParams.BuffMaxCount then
            return
        end
    end

    local isNewBuff = true

    if buff.tab.LayerID ~= -1 then
        local sameLayer = self:GetBuffByLayerID(buff.tab.LayerID)
        if sameLayer ~= nil then
            isNewBuff = false
            sameLayer:AddLayer(buff)
        end
    end

    if isNewBuff then
        
        --互斥
        for _,b in ipairs(self.buffs) do
            if b.isAlive then
                local mutex,ret = b:CheckMutex(buff.tab)
                if mutex then
                    if ret <= 0 then
                        --策划需求，互斥不算被移除
                        b:FadeOut(true)
                    end
                end
            end
        end

        table.insert( self.buffs,buff )
        buff.buffId = self.idBase
        self.idBase = self.idBase + 1
    end

    buff:FadeIn()

    if not notNotify then
        self:NotifyBuffChange()
    end
end

function BuffContainer:RemoveBuff(buff,notNotify)
    if buff == nil then
        return false
    end

    --防止错误，不立即删除，留给下次Update
    buff:FadeOut()
    if not notNotify then
        self:NotifyBuffChange()
    end
    return true
end

--清空
function BuffContainer:Clear()
    local dirty = false
    for _,buff in ipairs(self.buffs) do
        if buff.isAlive then
            buff:FadeOut()
            dirty = true
        end
    end
    
    if dirty then
        self:NotifyBuffChange()        
    end
end

function BuffContainer:RefixImpactSend(impact)
    for _,buff in ipairs(self.buffs) do
        if buff.isAlive then
            buff:RefixImpactSend(impact)
        end
    end
end

function BuffContainer:RefixImpactRecv(impact)
    for _,buff in ipairs(self.buffs) do
        if buff.isAlive then
            buff:RefixImpactRecv(impact)
        end
    end
end

function BuffContainer:SendMessage(methodName,...)
    --先看是否是自己关心
    local func = self[methodName]
    if func ~= nil then
        func(self,...)
    end

    --广播出去
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            buff:SendMessage(methodName,...)
        end
    end
end

function BuffContainer:IsImmue(impactTab)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsImmue(impactTab) then
                return true
            end
        end
    end
    return false
end

function BuffContainer:IsImmueHit(hitTab)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsImmueHit(hitTab) then
                return true
            end
        end
    end
    return false
end

function BuffContainer:IsStun()
    return self:AnyBuffIsSubClass(ImpactSubClass.Stun)
end

function BuffContainer:IsSilence()
    return self:AnyBuffIsSubClass(ImpactSubClass.Silence)
end

function BuffContainer:IsTaunt()
    return self:AnyBuffIsSubClass(ImpactSubClass.Taunt)
end

function BuffContainer:IsChaos()
    return self:AnyBuffIsSubClass(ImpactSubClass.Chaos)
end

function BuffContainer:AnyBuffIsSubClass(subClass)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsSubClass(subClass) then
                return true
            end
        end
    end
    return false
end

function BuffContainer:AnyBuffIsTag(tag)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff.tag == tag then
                return true
            end
        end
    end
    return false
end

function BuffContainer:GetBuffsByCond( func )
    local buffs = self.buffs
    local ret = {}
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if (func(buff)) then
                table.insert(ret,buff)
            end
        end
    end
    return ret
end

--按照subClass获取第一个buff
function BuffContainer:GetBuffByImpactSubClass(subClass)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsSubClass(subClass) then
                return buff
            end
        end
    end
    return nil
end

function BuffContainer:GetBuffsByImpactSubClass(subClass)
    local buffs = self.buffs
    local ret = {}    
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsSubClass(subClass) then
                table.insert(ret,buff)
            end
        end
    end
    return ret
end

--按照成class获取第一个buff
function BuffContainer:GetBuffByImpactClass(impactClass)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsClass(impactClass) then
                return buff
            end
        end
    end
    return nil
end

function BuffContainer:GetBuffsByImpactClass(impactClass)
    local buffs = self.buffs
    local ret = {}
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsClass(impactClass) then
                table.insert( ret,buff )
            end
        end
    end
    return ret
end

function BuffContainer:CountBuffsByImpactClass(impactClass)
    local buffs = self.buffs
    local ret = 0
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff:IsClass(impactClass) then
                ret = ret + 1
            end
        end
    end
    return ret
end

--按照ImpactId获取第一个Impact
function BuffContainer:GetBuffByImpactId(impactId)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff.impactId == impactId then
                return buff
            end
        end
    end
    return nil
end

function BuffContainer:GetBuffsByImpactId(impactId)
    local buffs = self.buffs
    local ret = {}
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff.impactId == impactId then
                table.insert( ret,buff )
            end
        end
    end
    return ret
end

function BuffContainer:msgRecvDamage(ret)
    --移除受击自动移除的buff
    local buffs = self.buffs
    local dirty = false

    local battle = self.owner.battle
    local attacker = battle:GetRoleById(ret.senderId)
    local silentAtk = false
    if attacker ~= nil then
        silentAtk = attacker:IsSilentAttack()
    end

    local isIceBreak = false

    --移除发送者死亡后，需要移除的buff
    for _,buff in ipairs(buffs) do
        local buffNeedFadeOut = false
        if buff.isAlive then
            if buff:IsSubClass(ImpactSubClass.Ice) then
                buffNeedFadeOut = true
                isIceBreak = true
            elseif buff:IsSubClass(ImpactSubClass.Sleep) and not silentAtk then
                buffNeedFadeOut = true
            elseif buff:HasAutoFadeOutTag(AutoFadeOutTag.RecvDamage) then
                buffNeedFadeOut = true
            end
        end

        if buffNeedFadeOut then
            buff:FadeOut()
            dirty = true
        end
    end

    if dirty then
        self:NotifyBuffChange()
    end

    if isIceBreak then
        self.owner:OnIceBreak(ret)
    end
end

function BuffContainer:msgAfterUseSkill(skillProcess)
    
    --只有在自己的回合，使用技能后，才会移除【使用技能后移除】的buff
    local battle = self.owner.battle

    local buffs = self.buffs
    local dirty = false
    --移除使用技能后，需要移除的buff
    for _,buff in ipairs(buffs) do
        local needFadeOut = false
        if buff.isAlive then
            if buff:HasAutoFadeOutTag(AutoFadeOutTag.UseSkill)  and battle.curRoundRole == self.owner then
                needFadeOut = true
            elseif buff:HasAutoFadeOutTag(AutoFadeOutTag.UseSkillAny) then
                needFadeOut = true
            end

            if needFadeOut then
                buff:FadeOut()
                dirty = true
            end
        end
    end

    if dirty then
        self:NotifyBuffChange()
    end
end

function BuffContainer:bcRoleDead(roleId,killerId)
    local buffs = self.buffs
    local dirty = false
    --移除发送者死亡后，需要移除的buff
    for _,buff in ipairs(buffs) do
        if buff.isAlive 
            and buff:HasAutoFadeOutTag(AutoFadeOutTag.SenderDead) 
            and buff.sender.id == roleId then

            buff:FadeOut()
            dirty = true
        end
    end
    if dirty then
        self:NotifyBuffChange()
    end
end

function BuffContainer:bcActionDone(role,action)
    if role == nil then
        return
    end

    local buffs = self.buffs
    local dirty = false
    --移除发送者死亡后，需要移除的buff
    for _,buff in ipairs(buffs) do
        if buff.isAlive 
            and buff:HasAutoFadeOutTag(AutoFadeOutTag.SenderActEnd) 
            and buff.sender.id == role.id then

            buff:FadeOut()
            dirty = true
        end
    end
    if dirty then
        self:NotifyBuffChange()
    end
end

function BuffContainer:bcAfterUseSkill(role,skillProcess)
    if role == nil then
        return
    end

    local buffs = self.buffs
    local dirty = false
    --移除发送者死亡后，需要移除的buff
    for _,buff in ipairs(buffs) do
        if buff.isAlive 
            and (buff.skillInfo ~= nil and buff.skillInfo.skillProcess == skillProcess)
            and buff:HasAutoFadeOutTag(AutoFadeOutTag.SendSkillFin)
            and buff.sender.id == role.id then

            buff:FadeOut()
            dirty = true
        end
    end
    if dirty then
        self:NotifyBuffChange()
    end
end

function BuffContainer:bcRoundEnd(role)
    if role == nil then
        return
    end

    local buffs = self.buffs
    local dirty = false
    --发送者行动结束后，移除
    for _,buff in ipairs(buffs) do
        if buff.isAlive 
            and buff:HasAutoFadeOutTag(AutoFadeOutTag.SenderRoundEnd) 
            and buff.sender.id == role.id then

            buff:FadeOut()
            dirty = true
        end
    end
    if dirty then
        self:NotifyBuffChange()
    end
end

function BuffContainer:CallImpactFunc(methodName,...)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            local func = buff[methodName]
            if func ~= nil then
                func(buff,...)
            end
        end
    end
end

function BuffContainer:CheckMutex(tab)
    local buffs = self.buffs
    local dirty = false
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            local mutex,ret = buff:CheckMutex(tab)
            if mutex then
                if ret > 0 then
                    return true
                end
            end
        end
    end

    return false
end

function BuffContainer:CheckLayerFull(tab)
    if tab.LayerID == -1 then
        return false
    end

    local sameLayer = self:GetBuffByLayerID(tab.LayerID)
    if sameLayer == nil then
        return false
    end

    return not sameLayer:CanAddLayer()
end

function BuffContainer:HasImpact(impactId,sender)
    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff.impactId == impactId then
                if sender ~= nil then
                    if buff.sender == sender then
                        return true
                    end
                else
                    return true
                end
            end
        end
    end
    return false
end

function BuffContainer:GetBuffByLayerID(layerID)

    if layerID == -1 then
        return nil
    end

    local buffs = self.buffs
    for _,buff in ipairs(buffs) do
        if buff.isAlive then
            if buff.tab.LayerID == layerID then
                return buff
            end
        end
    end
    return nil
end

return BuffContainer