--被动效果，使用技能时，额外释放一次技能
--参数

--1，skillType
    --1，普攻
    --2，任意主动
    --3，指定class
        --参数，skillClass
    --5，指定id
        --参数，skillExId
--2，技能参数

--3，概率

--4，额外使用的技能索引（该角色的第几个技能）
--5，额外使用的技能（skillEx表id，参数2必须填-1才生效）

--6，条件检查时机（1，使用技能前，2，使用技能后）

--7,不能切换目标（0，否（目标死亡后会随机选择目标），1是（目标死亡后追加不能触发））


--8，条件类型
    --1，使用技能前，目标血量想小于（大于）指定值
        --参数，（1，小于，2大于）
        --参数，比例（5000表示50%）
    --2，目标包含(不包含)指定buff
        --参数，(1,包含，2，不包含)
        --参数，impact id
    --3，敌方有目标包含（不包含）指定buff
        --参数，（1，包含，2，不包含）
        --参数，impact id

--9，条件参数
--10，条件参数

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')

require("class")

local SkillType = {
    Skill1 = 1,
    SkillAny = 2,
    BySkillClass = 3,
    BySKillExId = 4,
}

local CheckType = {
    BeforeCast = 1,
    AfterCast = 2,
}

local CondType = {
    CurHP = 1,
    HasBuff = 2,
    AnyOneHasBuff = 3,
}

local Impact_14 = class("Impact_14",Impact)

function Impact_14:OnActive()
    Impact_14.__base.OnActive(self)

    self.useSkillType = self.tab.Param[1]
    self.useSkillVal = self.tab.Param[2]

    self.chance = self.tab.Param[3]
    self.skillIndex = self.tab.Param[4]
    self.skillExId = self.tab.Param[5]
    self.checkType = self.tab.Param[6]
    self.notSwitchTarget = self.tab.Param[7] == 1

    self.condType = self.tab.Param[8]
    if self.condType > 0 then
        self.condParams = {self.tab.Param[9],self.tab.Param[10]}
    end

end

function Impact_14:CheckCond(skillProcess)
    local targetId = skillProcess.targetSelectedId
    local target = self.recver.battle:GetRoleById(targetId)

    if self.condType == CondType.CurHP then
        if target == nil then
            return false
        end

        local hpPercent = target:GetHPPercent10000()
        if self.condParams[1] == 1 then
            if hpPercent >= self.condParams[2] then
                return false
            end
        elseif self.condParams[1] == 2 then
            if hpPercent <= self.condParams[2] then
                return false
            end
        else
            return false
        end
    elseif self.condType == CondType.HasBuff then
        if target == nil then
            return false
        end

        if self.condParams[1] == 1 then
            if not target:HasImpact(self.condParams[2]) then
                return false
            end
        elseif self.condParams[1] == 1 then
            if target:HasImpact(self.condParams[2]) then
                return false
            end
        end
    elseif self.condType == CondType.AnyOneHasBuff then
        if target == nil then
            return false
        end

        local allRoles = target.battle:GetRoleAlies(target)
        local hasAnyOne = false
        for _,r in ipairs(allRoles) do
            if self.condParams[1] == 1 then
                if r:HasImpact(self.condParams[2]) then
                    hasAnyOne = true
                    break
                end
            elseif self.condParams[1] == 1 then
                if not r:HasImpact(self.condParams[2]) then
                    hasAnyOne = true
                    break
                end
            end
        end
        if not hasAnyOne then
            return false
        end
    end

    return true
end

function Impact_14:Cast(skillProcess)
    local targetId = skillProcess.targetSelectedId
    local skillIndex = self.skillIndex
    local skillExId = self.skillExId

    if skillIndex ~= -1 then
        skillExId = self.recver:GetSkillIdByIndex(skillIndex)
    end

    if skillExId ~= -1 then
        local role = self.recver.battle:GetRoleById(targetId)
        if role == nil or not role:IsAlive() or not role.isValid then
            
            if self.notSwitchTarget then
                return
            end

            --随机一个新目标
            role = self.recver:RandomSelectSkillTarget(skillExId)
        end

        if role == nil then
            return
        end

        self.recver:ActUseSkillByID(skillExId,role.id)
        self:ImpactEffected()        
    end
end

function Impact_14:TryCast(skillProcess)

    if not self:CanEffected() then
        return
    end

    if skillProcess == nil then
        return false
    end

    local chance = self.chance
    if not self.recver.battle:IsRandLt(chance) then
        return
    end

    if self.useSkillType == SkillType.Skill1 then
        if not skillProcess:IsNormalAttack() then
            return
        end
    elseif self.useSkillType == SkillType.SkillAny then
        if not skillProcess:IsCommonSkill() then
            return
        end
    elseif self.useSkillType == SkillType.BySkillClass then
        if skillProcess.tabBase.SkillClass ~= self.useSkillVal then
            return
        end
    elseif self.useSkillType == SkillType.BySKillExId then
        if skillProcess:GetSkillId() ~= self.useSkillVal then
            return
        end
    else
        return
    end

    if not self:CheckCond(skillProcess) then
        return
    end
    
    self:Cast(skillProcess)
end

function Impact_14:msgBeforeUseSkill(skillProcess)
    if self.checkType == CheckType.BeforeCast then
        self:TryCast(skillProcess)
    end
end


function Impact_14:msgAfterUseSkill( skillProcess )
    if self.checkType == CheckType.AfterCast then
        self:TryCast(skillProcess)
    end
end

return Impact_14