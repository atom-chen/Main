--触发暴击时，对暴击目标使用技能
--参数
--1,技能ID
--2,立即使用

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')


require("class")

local Impact_44 = class("Impact_44",Impact)

function Impact_44:msgDamageOther(ret)
    if not ret.isCrit then
        return
    end

    if not self:CanEffected() then
        return
    end

    if ret.targetId == nil or ret.targetId == -1 then
        return
    end

    local recver = self.recver

    local target = recver.battle:GetRoleById(ret.targetId)
    if target == nil then
        return
    end

    local succ = false
    if self.tab.Param[2] == 1 then
        succ = recver:CastSkill(self.tab.Param[1],ret.targetId)
    else
        succ = recver:ActUseSkillByID(self.tab.Param[1],ret.targetId)
    end

    if succ then
        self:ImpactEffected()
    end
    
end

return Impact_44