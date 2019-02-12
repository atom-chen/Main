--定期触发使用技能
--参数
--1.技能id
--2.周期类型
    --1，自己回合开始；2，波次开始;
    --3，自己回合结束；
    --4，任意队友回合开始，5，任意敌人回合开始
    --6，任意队友死亡时，7，任意敌人死亡时
    --8，自己击杀后
    --9，队友使用特定技能时，10，敌人使用特定技能时
        --参数，skillClass
    --11，发送的impact被移除
        --参数，impact id
        --参数，移除类型（-1，只要被移除;1，到回合自动移除；2，被移除（比如驱散、打破、转移等）

    --12，人妖切换时使用技能

--3,条件类型
    --1,敌方包含特定class的buff
        --参数，impact class

--4，条件参数
--5，条件参数

--6，是否立即释放
    --回合开始时用的技能，必须时立即释放，这个参数无用
    --回合结束使用技能，必须时顺序释放，这个参数无用

--7,技能类型（-1，skillExId；1，skillIndex）

--8，周期参数
--9，周期参数
--10，技能目标类型（-1，自己；1，当前回合行动者；）


local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')

local UseType = {
    RoundBegine = 1,
    WaveStart = 2,
    RoundEnd = 3,
    AnyOurRoundBegin = 4,
    AnyEnemyRoundBegin = 5,
    AnyOurDead = 6,
    AnyEnemyDead = 7,
    Killed = 8,
    OurUseSkill = 9,
    EnemyUseSkill = 10,
    SendImpactFadeOut = 11,
    EnvChange = 12,
}

local CondType = {
    EnemyHasBuffByClass = 1, -- 参数1：impactClass；
}

local CastType = {
    BySkillExId = -1,
    BySkillIndex = 1,
}

require("class")

local Impact_24 = class("Impact_24",Impact)

function Impact_24:OnActive()
    Impact_24.__base.OnActive(self)
    local skillParam = self.tab.Param[1]
    
    self.useType = self.tab.Param[2]
    self.condType = self.tab.Param[3]
    if self.condType ~= -1 then
        self.condParams = {self.tab.Param[4],self.tab.Param[5]}
    end

    self.isCastImm = self.tab.Param[6] == 1
    self.castBy = self.tab.Param[7]

    self.useParams = {self.tab.Param[8],self.tab.Param[9]}
    self.skillTargetType = self.tab.Param[10]

    if self.castBy == CastType.BySkillExId then
        self.skillId = skillParam
    elseif self.castBy == CastType.BySkillIndex then
        self.skillId = self.recver:GetSkillIdByIndex(skillParam)
    end
end

function Impact_24:msgRoundBegin()
    if self.useType ~= UseType.RoundBegine then
        return
    end

    self:Cast()
end

function Impact_24:bcRoundBegin(curRole)
    --永远不处理自己
    if self.recver == curRole then
        return
    end
	
    if self.useType == UseType.AnyOurRoundBegin 
            and curRole.side == self.recver.side then
        
        self:Cast()
    elseif self.useType == UseType.AnyEnemyRoundBegin 
            and curRole:GetOppositeSide() == self.recver.side then
        
        self:Cast()
    end
end

function Impact_24:msgBeforeRoundEnd()
    if self.useType ~= UseType.RoundEnd then
        return
    end

    self:CastSeq()
end

function Impact_24:bcWaveStart(waveIndex)
    if self.useType ~= UseType.WaveStart then
        return
    end
    
    self:Cast()
end

function Impact_24:Cast()
    
    if not self:CheckCond() then
        return
    end

    --平行使用，强制使用
    if self.recver:CastSkill(self.skillId,self:GetSkillCastTarget()) then
		self:ImpactEffected()  
	end
end

function Impact_24:CastSeq()
    
    if not self:CheckCond() then
        return
    end

    --不能使用
    if not self.recver:CheckCanUseSkill(self.skillId,_G.debuglogEnable) then
        return
    end
    --回合结束前，顺序使用
    self.recver:ActUseSkillByID(self.skillId,self:GetSkillCastTarget())
    self:ImpactEffected()  
end

function Impact_24:GetSkillCastTarget()
    if self.skillTargetType == -1 then
        return self.recver.id
    elseif self.skillTargetType == 1 then
        local battle = self.recver.battle
        local curRoundRole = battle.curRoundRole
        if curRoundRole ~= nil then
            return curRoundRole.id
        end
    end

    return self.recver.id
end

function Impact_24:CheckCond()
    
    if not self:CanEffected() then
        return false
    end

    if self.condType == -1 then
        return true
    end

    if self.condType == CondType.EnemyHasBuffByClass then
        local targets = self.recver.battle:GetRoleEnemies(self.recver)
        if targets == nil then
            return 0
        end
        for _,target in ipairs(targets) do
            local buffs = target:GetBuffsByImpactClass(self.condParams[1])
            if buffs ~= nil and #buffs > 0 then
                return true
            end
        end
    end

    return false
end

function Impact_24:bcRoleDead(roleId,killerId)

    local useType = self.useType
    
    if useType ~= UseType.AnyOurDead
        and useType ~= UseType.AnyEnemyDead
        and useType ~= UseType.Killed
     then
        return
    end

    local role = self.recver.battle:GetRoleById(roleId)

    if role == nil then
        return
    end

    --不处理自己的死亡
    if role.id == self.recver.id then
        return
    end

    local recver = self.recver

    
    if useType == UseType.AnyOurDead then
        if role.side ~= recver.side then
            return
        end
    elseif useType == UseType.AnyEnemyDead then
        if role.side == recver.side then
            return
        end
    elseif useType == UseType.Killed then
        if killerId ~= recver.id then
            return
        end
    end

    --触发点满足
    if self.isCastImm then
        self:Cast()
    else
        self:CastSeq()
    end
end

function Impact_24:bcAfterUseSkill(caster,skillProcess)
    --不处理自己
    if self.recver == caster then
        return
    end

    if self.useType == UseType.OurUseSkill then
        if self.recver.side ~= caster.side then
            return
        end
    elseif self.useType == UseType.EnemyUseSkill then
        if self.recver.side == caster.side then
            return
        end
    else
        return
    end

    if skillProcess.tabBase.SkillClass ~= self.useParams[1] then
        return
    end

    --触发点满足
    if self.isCastImm then
        self:Cast()
    else
        self:CastSeq()
    end
end

function Impact_24:bcRoleImpactFadeOut(impact,autoFadeOut)
    
    if self.useType ~= UseType.SendImpactFadeOut then
        return
    end

    if impact.sender ~= self.recver then
        return
    end

    if impact.impactId ~= self.useParams[1] then
        return
    end

    if self.useParams[2] == 1 then
        if not autoFadeOut then
            return
        end
    elseif self.useParams[2] == 2 then
        if autoFadeOut then
            return
        end
    else
        --始终通过
    end

    --触发点满足
    if self.isCastImm then
        self:Cast()
    else
        self:CastSeq()
    end
    
end

function Impact_24:bcChangeEnv(envType)
    if self.useType ~= UseType.EnvChange then
        return
    end

    self:Cast()
end

return Impact_24