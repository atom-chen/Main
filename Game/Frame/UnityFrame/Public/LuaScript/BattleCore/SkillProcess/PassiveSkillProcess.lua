require("class")

local SkillProcess = require("BattleCore/SkillProcess/SkillProcess")

local PassiveSkillProcess = class("PassiveSkillProcess",SkillProcess)
local BuffContainer = require("BattleCore/BuffContainer")
local TabMgr = require('TabManager')
local Impact = require("BattleCore/Impact/Impact")
local common = require("common")


function PassiveSkillProcess:Cast()
    local tabEx = self.tabEx
    local caster = self.caster
    local castSkillId = self.skillId
    self.targetSelectedId = self.caster.id

    if self.tabEx == nil then
        return
    end

    common.debuglog("cast passive",caster.id,castSkillId)

    for _,hitID in ipairs(tabEx.Hit) do
        local isValidHit = false
        local htype = type(hitID)
        if htype == "number" and hitID > 0 then
            isValidHit = true
        elseif htype == "table" then
            isValidHit = true
        end
        if isValidHit then
            self:DoHit(hitID)
        end
    end
end

function PassiveSkillProcess:SendImpacts2Target(targetId,hitTab,isDirective)
    local targetRole
    if type(targetId) == "table" then
        targetRole = targetId
        targetId = targetRole.id
    else
        targetRole = self.battle:GetRoleById(targetId)
    end

    if targetRole == nil then 
        return nil,false
    end
    
    --hit阶段发生的事件，打包到hitresult里
    self.battle:NewEventGroup()

    --产生impact
    for i,impactId in ipairs(hitTab.Impact) do
        local itype = type(impactId)
        local isValidImpact = (itype == 'number' and impactId > 0) or (itype == 'table')

        if isValidImpact  then
            Impact.SendImpactToTarget(impactId,targetRole,self.caster,targetRole.passiveBuffContainer,{
                skillProcess = self,
                hitTab = hitTab,
                skillExId = self.skillId,
                IsPassive = true,
            })
        end
    end

    return {
        targetID = targetId,
        events = self.battle:PopEventGroup(),
    },true
end


return PassiveSkillProcess