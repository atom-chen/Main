require("class")
local common = require("common")
local BattleState = require("BattleCore/Common/BattleState")

local Scenario = class("Scenario")

local warn = common.mk_warn('Scenario')
local warnf = common.mk_warnf('Scenario')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local BattleSide = require("BattleCore/Common/BattleSide")
local Impact = require("BattleCore/Impact/Impact")

function Scenario:ctor()
end

local ft = {
    IsDone = function(self)
        return self.excutedCount >= (self.ExcuteCount or 1)
    end,

    CanExcute = function(self,S)
        return not self:IsDone() and self.Trigger(S)
    end,

    Excute = function(self,S)
        local events = self.Events
        if events ~= nil then
            for i,e in ipairs(events) do
                local ret = S:HandleEvent(e)            
                if not ret then
                    warnf("section[%s],event error.index:%d",self.Name,i)
                end
            end
        end
        self.excutedCount = self.excutedCount + 1
    end
}
local sectionMT = {
    __index = function(t,k)
        if ft[k] ~= nil then
            return ft[k]
        end
        return t.__script[k]
    end
}

function Scenario:Init(battle,name)
    self.battle = battle
    self.sections = nil
    local s = require(string.format( "Scenario/%s",name ))
    if s == nil then
        warn("load scenario failed:" .. name .. '.')
        return
    end
    self.sections = {}
    local sectionSet = {}
    for _,v in ipairs(s) do
        local runTime = {
            excutedCount = 0,
            __script = v,
        }
        setmetatable(runTime, sectionMT)
        table.insert( self.sections,runTime )
        if not sectionSet[v.Name] then
            sectionSet[v.Name] = true
        else
            warn("duplicate section:" .. v.Name)
        end
    end
end

function Scenario:BeforeRoundBegin(battle)
    self:CheckAndExcute()
end

function Scenario:AfterRoundEnd(battle)
    self:CheckAndExcute()
end

function Scenario:CheckAndExcute()
    if not self:CommonCheck() then
        return
    end
    local excuteSections = nil
    
    for _,section in ipairs(self.sections) do
        if section:CanExcute(self) then
            excuteSections = excuteSections or {}
            table.insert(excuteSections,section)
        end
    end
    if excuteSections ~= nil and #excuteSections > 0 then
        self.battle:NewEventGroup()
        for _,section in ipairs(excuteSections) do
            section:Excute(self)
        end
        local events = self.battle:PopEventGroup()        
        self.battle:AddEvent({
            type = BattleEventType.Scenario,
            scenarioEvent = {
                events = events,
            }
        })
    end
end

function Scenario:CommonCheck()
    if self.battle == nil then
        return false
    end
    if self.sections == nil then
        return false
    end
    return true
end

function Scenario:IsRoundBegin(waveIndex,roundIndex)
    if not self:CommonCheck() then
        return false
    end
    local battle = self.battle
    if battle.state ~= BattleState.RoundBegin then
        return false
    end
    if battle.roundCount + 1 == roundIndex and battle.waveIndex == waveIndex then
        return true
    end
    return false
end

function Scenario:IsRoundEnd(waveIndex,roundIndex)
    if not self:CommonCheck() then
        return false
    end
        local battle = self.battle
    if battle.state ~= BattleState.RoundEnd then
        return false
    end
    if battle.roundCount + 1 == roundIndex and battle.waveIndex == waveIndex then
        return true
    end
    return false
end

function Scenario:IsBattleFinish()
    if not self:CommonCheck() then
        return false
    end
    local battle = self.battle
    if battle.state ~= BattleState.RoundEnd then
        return false
    end
    local winSide = battle:CalcWinSide()
    return winSide ~= BattleSide.bs_Invalid
end

function Scenario:IsSectionDone(name)
    if not self:CommonCheck() then
        return false
    end
    
    for _,section in ipairs(self.sections) do
        if section.Name == name then
            return section:IsDone()
        end
    end

    warn("no such section,IsSectionDone failed",name)
    return false
end

function Scenario:IsRandLt(chacne)
    if not self:CommonCheck() then
        return false
    end
    return chacne >= self.battle:Random10000()
end

function Scenario:IsActorValid(actor)
    if actor == nil then
        warn('actor not found')
        return false
    end
    local role = self:FindActor(actor)
    return role ~= nil and role.isValid and role:IsAlive()
end

function Scenario:IsHPLt(actor,percent)
    local p = percent / 10000
     if actor == nil then
        warn('actor nil')
        return false
    end
    local role = self:FindActor(actor)
    if role == nil then
        warn('actor not found')
        return false
    end
    return (role:GetHP() / role:GetMaxHP()) < p
end

function Scenario:FindActor(actor)
    local type = type(actor)
    if type == 'number' then
        if actor == -9999 then
            return self.battle:GetHero(BattleSide.bs_Blue)
        else
            return self.battle:GetRoleByMonsterId(actor)
        end
    elseif type == 'table' then
        local cardId = actor.CardId
        if cardId ~= nil then
            return self.battle:GetRoleByCardId(cardId)
        end

        local reliveTargetId = actor.ReliveMonsterId
        if reliveTargetId ~= nil then
            local deadRoleTb = self.battle.deadRoleTb
            if deadRoleTb == nil then
                return nil
            end
            for _,r in pairs(deadRoleTb) do
                if r.monsterId == actor.ReliveMonsterId then
                    return r
                end
            end
        end
        return nil
    end
    return nil
end

function Scenario:HandleEvent(e)
    local eventType = e.EventType
    local funcName = 'Handle_' .. eventType

    local func = self[funcName]
    if func == nil then
        warn("event type not support yet.",eventType)
        return false
    end

    return func(self,e)
end

function Scenario:Handle_Idle(e)
    local battle = self.battle

    if battle == nil then
        return false
    end

    battle:AddEvent({
        type = BattleEventType.Idle,
        idleEvent = {
            time = e.Time,
        },
    })

    return true
end

function Scenario:Handle_PlayCutscene(e)
    local battle = self.battle

    if battle == nil then
        return false
    end

    battle:AddEvent({
        type = BattleEventType.PlayCutscene,
        playCutsceneEvent = {
            id = e.BC_Id;
        },
    })

    return true
end

function Scenario:Handle_Spawn(e)
    local battle = self.battle

    if battle == nil then
        return false
    end

    local initData = battle:LoadRoleFromTab(e.MonsterId,e.Side,e.BattlePos)
    if e.Side == BattleSide.bs_Blue then
        initData.userObjId = 1 --强制id为1
    else
        initData.isAI = true
    end
    
    local role = battle:CreateRole(initData,e.IsPlaySpawnAnim)
    if role == nil then
        warn('spaw failed')
        return false
    end
    return true
end

function Scenario:Handle_ForceFinish(e)
    local battle = self.battle
    if battle == nil then
        return false
    end

    battle:ForceFinish(e.WinSide)
    return true
end

function Scenario:Handle_UseSkill(e)
    if e.Actor == nil then
        warn('actor nil')
        return false
    end
    local role = self:FindActor(e.Actor)
    if role == nil then
        warn('actor not found')
        return false
    end
    local target = self:FindActor(e.Target)
    if target == nil then
        warn('target not found')
        return false
    end
    role:CastSkill(e.SkillId,target.id)
    return true
end

function Scenario:Handle_DelRole(e)
    if e.Actor == nil then
        warn('actor nil')
        return false
    end
    local role = self:FindActor(e.Actor)
    if role == nil then
        warn('actor not found')
        return false
    end
    local battle = self.battle
    if battle == nil then
        return false
    end
    battle:DelRole(role)
    return true
end

function Scenario:Handle_SendImpact(e)
    if e.Target ~= nil then
        local target = self:FindActor(e.Target)
        if target == nil then
            warn('target not found')
            return false
        end
        Impact.SendImpactToTarget(e.ImpactId,target)
    elseif type(e.Targets) == 'table' then
        for _,t in ipairs(e.Targets) do
            local target = self:FindActor(t)
            if target == nil then
                warn('target not found')
                return false
            end
            Impact.SendImpactToTarget(e.ImpactId,target)
        end
    end
    return true
end

function Scenario:Handle_RoleAction(e)
    if e.Actor == nil then
        warn('actor nil')
        return false
    end
    local role = self:FindActor(e.Actor)
    if role == nil then
        warn('actor not found')
        return false
    end
    local battle = self.battle
    if battle == nil then
        return false
    end
    local action = {
        roleId = role.id,
    }
    if e.AnimId ~= nil and e.AnimId ~= -1 then
        action.animId = e.AnimId
    end
    if e.EffectId ~= nil and e.EffectId ~= -1 then
        action.effectId = e.EffectId
    end
    if e.PaoPao ~= nil and e.PaoPao ~= -1 then
        action.paopao = e.PaoPao
    end
    if e.PaoPaoTime ~= nil and e.PaoPaoTime > 0 then
        action.paopaoTime = e.PaoPaoTime
    end

    battle:AddEvent({
        type = BattleEventType.RoleActionEvent,
        roleActionEvent = action,
    })
    
    return true
end

function Scenario:Handle_PlayStoryContent(e)
    if e.SC_Id == nil then
        warn("stroy content id nil")
        return false
    end

    local battle = self.battle
    if battle == nil then
        return false
    end

    battle:AddEvent({
        type = BattleEventType.PlayStoryContentEvent,
        playStoryContentEvent = {
            id = e.SC_Id,
        }
    })
    return true
end

function Scenario:Handle_SkipRound(e)
    local battle = self.battle
    if battle == nil then
        return false
    end

    battle:SetRoundStart()
    return true
end

function Scenario:Handle_PauseEx(e)
    local battle = self.battle
    if battle == nil then
        return false
    end

    battle:AddEvent({
        type = BattleEventType.PauseEx,
        playCutsceneEvent = {
            id = e.PE_Id
        },
    })

    return true
end

return Scenario