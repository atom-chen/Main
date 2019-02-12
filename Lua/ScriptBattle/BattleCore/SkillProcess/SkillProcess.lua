--技能处理过程，每次cast一个技能，会创建一个skillprocess
--不同的技能流程，不同的计算方式

require("class")

local SkillProcess = class("SkillProcess")

local ImpactTargetType = require("BattleCore/Common/ImpactTargetType")
local Impact = require("BattleCore/Impact/Impact")
local SkillTargetType = require("BattleCore/Common/SkillTargetType")
local BattleEventType = require('BattleCore/Common/BattleEventType')
local TabMgr = require("TabManager")
local AttrType = require("BattleCore/Common/AttrType")
local common = require("common")
local SkillClass = require("BattleCore/Common/SkillClass")
local ImpactClass = require("BattleCore/Common/ImpactClass")
local ImpactUtils = require("BattleCore/Impact/ImpactUtils")
local CalcParams = require("BattleCore/CalcParams")
local SkillLogicType = require("BattleCore/Common/SkillLogicType")
local SS = require("BattleCore/SkillProcess/ScriptSkillParser")

function SkillProcess:Init(skillId,caster,battle)
    self.caster = caster
    self.targetSelectedId = -1
    self.tabBase  = nil
    self.tabEx = nil
    self.battle = battle

    self.tabEx = TabMgr.GetByID("SkillEx",skillId)
    if self.tabEx ~= nil and (self.tabEx.__script_type == nil and self.tabEx.ScriptID ~= -1) then
        self.tabEx = SS.Parse(self.tabEx,self)
    end

    if self.tabEx ~= nil then
        self.tabBase = TabMgr.GetByID("SkillBase",self.tabEx.BaseID)
    end
end

function SkillProcess:GetSkillId()
    return self.tabEx.Id
end

--标准的技能处理流程，不同的技能流程可以重写
function SkillProcess:Process()
    local caster = self.caster
    self:OnSkillStart()
    self:Cast()
    self:OnSkillEnd()
end

function SkillProcess:OnSkillStart()
    --消耗
    if self.caster == nil then
        return
    end

    if self.tabEx ~= nil and self.tabEx.SPCost > 0 then
        self.caster:CostSP(self.tabEx.SPCost)
    end
end

function SkillProcess:OnSkillEnd()
    --结算
end

function SkillProcess:Cast()
    local castSkillId = self.tabEx.Id

    if self.tabEx == nil then
        return
    end

    local tabEx = self.tabEx
    local caster = self.caster
    if tabEx.CooldownId >= 0 and tabEx.Cooldown > 0 then
        caster:BeginCooldown(tabEx.id,tabEx.CooldownId,tabEx.Cooldown)
    end
    
    local tmpBuffs = nil
    if tabEx.TmpImpacts ~= nil then
        tmpBuffs = {}
        --上临时buff
        for _,t in ipairs(tabEx.TmpImpacts) do
            local impact = Impact.SendImpactToTarget(t,self.caster,self.caster,nil,{
                skillProcess = self,
                skillExId = castSkillId,
            },1,true)
            if impact ~= nil then
                table.insert(tmpBuffs,impact)
            end
        end
    end
    
    local hits = {}

    if self:IsReliveSkill() then
        --复活技能，只处理第一个hit
        local hit = self:ReliveTarget(self.targetSelectedId,tabEx.Hit[1])
        table.insert(hits,hit)
    else

        for _,hitID in ipairs(tabEx.Hit) do
            local isValidHit = false
            local htype = type(hitID)
            if htype == "number" and hitID > 0 then
                isValidHit = true
            elseif htype == "table" then
                isValidHit = true
            end
            if isValidHit then
                local hit = self:DoHit(hitID)
                if hit ~= nil then
                    table.insert( hits,hit )
                end
                --不能行动了(打断)
                if not self.caster:CanAct(self.tabEx) then
                    break
                end
            end
        end
    end

    if tmpBuffs ~= nil then
        for _,buff in ipairs(tmpBuffs) do
            buff:FadeOut(true)
        end
    end

    self.battle:AddEvent({
        type = BattleEventType.UseSkill,
        skillEvent = {
            targetID = self.targetSelectedId,
            usedSkillID = castSkillId,
            casterID = self.caster.id,
            hits = hits,
        },
    })
end

function SkillProcess:DoHit(hitID)
    --读表
    local hitTab = TabMgr.GetByID("SkillHit",hitID)
    if hitTab == nil then
        return nil
    end

    local targetType = hitTab.TargetType
    local tabBase = self.tabBase

    if tabBase == nil then
        return nil
    end

    local tmpBuffs = nil
    if hitTab.TmpImpacts ~= nil then
        tmpBuffs = {}
        --上临时buff
        for _,t in ipairs(hitTab.TmpImpacts) do
            local impact = Impact.SendImpactToTarget(t,self.caster,self.caster,nil,{
                skillProcess = self,
                skillExId = self.tabEx.Id,
            },1,true)
            if impact ~= nil then
                table.insert(tmpBuffs,impact)
            end
        end
    end

    local hit = {}
    local hitResults = {}
    hit.hitResults = hitResults

    if hitTab.IsAnimHit == 1 then
        hit.isAnimHit = true
    end

    if targetType == ImpactTargetType.SkillTarget then
        --技能选择的目标
        if tabBase.Range == 1 then
            local ret = self:SendImpacts2Target(self.targetSelectedId,hitTab,true)
            table.insert( hitResults,ret )
        else
            local targets
            if tabBase.TargetType == SkillTargetType.Our then
                targets = self.battle:GetRoleAlies(self.caster)
            elseif tabBase.TargetType == SkillTargetType.Enemy then
                targets = self.battle:GetRoleEnemies(self.caster)
            end
            self:SendTo(targets,hitTab,hitResults)
        end
    elseif targetType == ImpactTargetType.SkillCaster then
        --施法者
        local ret = self:SendImpacts2Target(self.caster,hitTab)
        table.insert( hitResults,ret )
    elseif targetType == ImpactTargetType.OurAll then
        --我方全体
        local targets = self.battle:GetRoleAlies(self.caster)
        self:SendTo(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.EnemyAll then
        --对方全体
        local targets = self.battle:GetRoleEnemies(self.caster)
        self:SendTo(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.OurRandom then
        --我方随机
        local targets = self.battle:GetRoleAlies(self.caster)
        self:SendToRandTargets(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.EnemyRandom then
        --对方随机
        local targets = self.battle:GetRoleEnemies(self.caster)
        self:SendToRandTargets(targets,hitTab,hitResults)
    -- elseif targetType == ImpactTargetType.OurRandomWithTarget then
    --     --我方随机
    --     local targets = self.battle:GetRoleAlies(self.caster)
    --     self:SendToRandWithSkillTargets(targets,hitTab,hitResults)
    -- elseif targetType == ImpactTargetType.EnemyRandomWithTarget then
    --     --对方随机
    --     local targets = self.battle:GetRoleEnemies(self.caster)
    --     self:SendToRandWithSkillTargets(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.OurHpMin then
        --我方血量最小
        local targets = self.battle:GetRoleAlies(self.caster)
        self:SendToHpMin(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.EnemyHpMin then
        --对方血量最小
        local targets = self.battle:GetRoleEnemies(self.caster)
        self:SendToHpMin(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.EnemyHpMax then
        --对方血量最小
        local targets = self.battle:GetRoleEnemies(self.caster)
        self:SendToHpMax(targets,hitTab,hitResults)
    elseif targetType == ImpactTargetType.MarkedEnemy then
        --特定buff标记过的敌人
        self:SendToMarked(hitTab,hitResults)
    elseif targetType == ImpactTargetType.Splash then
        --溅射伤害，从目标开始，概率溅射到旁边的人
        self:SplashTo(hitTab,hitResults)
    elseif targetType == ImpactTargetType.OurRandomHasBuff then
         --我方随机
        local targets = self.battle:GetRoleAlies(self.caster)
        local count = hitTab.TargetParam[1]
        local impactClass = hitTab.TargetParam[2]
        local isRevert = hitTab.TargetParam[3] == 1
        self:SendToRandCond(targets,count,hitTab,hitResults,function(r)
            local hasBuff = not SkillProcess.TargetFilterByHasBuff(r,impactClass)
            if isRevert then hasBuff = not hasBuff end
            return hasBuff
        end)
    elseif targetType == ImpactTargetType.EnemyRandomHasBuff then
        local targets = self.battle:GetRoleEnemies(self.caster)
        local count = hitTab.TargetParam[1]
        local impactClass = hitTab.TargetParam[2]
        local isRevert = hitTab.TargetParam[3] == 1
        self:SendToRandCond(targets,count,hitTab,hitResults,function(r)
            local hasBuff = not SkillProcess.TargetFilterByHasBuff(r,impactClass)
            if isRevert then hasBuff = not hasBuff end
            return hasBuff
        end)
    elseif targetType == ImpactTargetType.OurRandomByCardId then
          --我方随机
          local targets = self.battle:GetRoleAlies(self.caster)
          local count = hitTab.TargetParam[1]
          local cardId = hitTab.TargetParam[2]
          self:SendToRandCond(targets,count,hitTab,hitResults,function(r)
            if r.cardId == nil then return false end
            return r.cardId == cardId
          end)
    elseif targetType == ImpactTargetType.OurHpPercentMin then
        --我方血量百分比最小
        local targets = self.battle:GetRoleAlies(self.caster)
        self:SendToHpPercentMin(targets,hitTab,hitResults)
	elseif targetType == ImpactTargetType.OurApMin then
        --我方AP最小
        local targets = self.battle:GetRoleAlies(self.caster)
        self:SendToApMin(targets,hitTab,hitResults)
    end

    if tmpBuffs ~= nil then
        for _,buff in ipairs(tmpBuffs) do
            buff:FadeOut(true)
        end
    end

    return hit
end

function SkillProcess:SendTo(targets,hitTab,hitResults)
    if targets == nil then
        return
    end
    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)
    for _,r in ipairs(targets) do
        local ret = self:SendImpacts2Target(r,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SendToRandTargets(targets,hitTab,hitResults)
    if targets == nil then
        return
    end

    local count = hitTab.TargetParam[1]
    --0，纯随机；1，必然包含技能目标；2，必然不包含技能目标
    local containsSkillTarget = hitTab.TargetParam[2]
    --是否包含施法者
    local containsCaster = hitTab.TargetParam[3] ~= 1
	local noSummon = hitTab.TargetParam[4] --是否排除召唤物

    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)

	if noSummon == 1 then
		--排除召唤物
		common.removec(targets,function(r) return r:IsSummon() end)
	end
	
    if not containsCaster then
        common.removec(targets,function(r)
            return r == self.caster
        end)
    end
	
    if containsSkillTarget == 1 then
        --如果已经包含技能目标了，则不需要处理，如果没包含，则替换其中一个目标为技能目标
        targets = self.battle:RandomSelect(targets,count)    
        if targets == nil then
            return
        end
        local alreadyHas = false
        for _,target in ipairs(targets) do
            if target.id == self.targetSelectedId then
                alreadyHas = true
                break
            end
        end
        if not alreadyHas then
            if #targets > 0 then
                targets[1] = self.targetSelectedId
            end        
        end
    elseif containsSkillTarget == 2 then
        --忽略掉技能目标
        common.removec(targets,function(r) return r.id == self.targetSelectedId end)
        targets = self.battle:RandomSelect(targets,count)    
    else
        targets = self.battle:RandomSelect(targets,count)    
    end
    
    if targets == nil then
        return
    end

    for _,r in ipairs(targets) do
        local ret = self:SendImpacts2Target(r,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SendToRandCond(targets,count,hitTab,hitResults,func)
    if targets == nil then
        return
    end
	
    --忽略掉不合法的角色
	local targetfilterEx = function(r,caster,tabEx)
		if not func(r) then
			return true
		end
		return SkillProcess.TargetFilterEx(r,caster,tabEx)
	end
	
	common.removecEx(targets,targetfilterEx,self.caster,self.tabEx)
    
    targets = self.battle:RandomSelect(targets,count)    
    
    if targets == nil then
        return
    end

    for _,r in ipairs(targets) do
        local ret = self:SendImpacts2Target(r,hitTab)
        table.insert( hitResults,ret )
    end
end

-- function SkillProcess:SendToRandWithSkillTargets(targets,hitTab,hitResults)
--     if targets == nil then
--         return
--     end
--     --忽略掉不合法的角色
--     common.removec(targets,SkillProcess.TargetFilter)
--     targets = self.battle:RandomSelect(targets,hitTab.TargetParam[1] - 1)
--     table.insert(targets,self.targetSelectedId)
--     for _,r in ipairs(targets) do
--         local ret = self:SendImpacts2Target(r,hitTab)
--         table.insert( hitResults,ret )
--     end
-- end

function SkillProcess:SendToHpMin(targets,hitTab,hitResults)
    if targets == nil then
        return
    end
    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)
    --找到血量最小的
    local minHpRole = nil
    for _,r in ipairs(targets) do
        if minHpRole == nil then 
            minHpRole = r
        elseif r:GetHP() < minHpRole:GetHP() then
            minHpRole = r
        end
    end
    if minHpRole ~= nil then
        local ret = self:SendImpacts2Target(minHpRole,hitTab,true)
        table.insert( hitResults,ret )
    end
end

--选AP最小的
function SkillProcess:SendToApMin(targets,hitTab,hitResults)
    if targets == nil then
        return
    end
    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)
    --找到血量最小的
    local minApRole = nil
    for _,r in ipairs(targets) do
        if minApRole == nil then 
            minApRole = r
        elseif r:GetAP() < minApRole:GetAP() then
            minApRole = r
        end
    end
    if minApRole ~= nil then
        local ret = self:SendImpacts2Target(minApRole,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SendToHpPercentMin(targets,hitTab,hitResults)
    if targets == nil then
        return
    end
    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)
    --找到血量百分比最小的
    local minHpRole = nil
    for _,r in ipairs(targets) do
        if minHpRole == nil then 
            minHpRole = r
        elseif r:GetHP() / r:GetMaxHP()  < minHpRole:GetHP() / minHpRole:GetMaxHP() then
            minHpRole = r
        end
    end
    if minHpRole ~= nil then
        local ret = self:SendImpacts2Target(minHpRole,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SendToHpMax(targets,hitTab,hitResults)
    if targets == nil then
        return
    end
    --忽略掉不合法的角色
    common.removecEx(targets,SkillProcess.TargetFilterEx,self.caster,self.tabEx)
    --找到血量最小的
    local maxHpRole = nil
    for _,r in ipairs(targets) do
        if maxHpRole == nil then 
            maxHpRole = r
        elseif r:GetHP() > maxHpRole:GetHP() then
            maxHpRole = r
        end
    end
    if maxHpRole ~= nil then
        local ret = self:SendImpacts2Target(maxHpRole,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SendToMarked(hitTab,hitResults)
    local buffId = hitTab.TargetParam[1]
    local targets = SkillProcess.GetMarkedTargets(self.caster,buffId, self.tabEx)
    if targets == nil then
        return
    end
    for _,target in ipairs(targets) do
        local ret = self:SendImpacts2Target(target,hitTab)
        table.insert( hitResults,ret )
    end
end

function SkillProcess:SplashTo(hitTab,hitResults)
    local minCount = hitTab.TargetParam[1]
    local maxCount = hitTab.TargetParam[2]
    local chance = hitTab.TargetParam[3]
    local chanceDamping = hitTab.TargetParam[4]
    local chanceMin = hitTab.TargetParam[5]

    local lastTarget = self.battle:GetRoleById(self.targetSelectedId)

    local doHit = function()
        local ret,succ = self:SendImpacts2Target(lastTarget,hitTab)
        table.insert( hitResults,ret )
        if ret == nil or not succ then
            --hit失败
            return false
        end
        return true
    end

    local filter_last = function(r,caster,tab) return r == lastTarget or SkillProcess.TargetFilterEx(r,caster,tab) end

    --首先命中技能目标
    if not doHit() then
        return
    end

    local targets = self.battle:GetRoleEnemies(self.caster)
    if targets == nil then
        return
    end

    local count = 0

    while count < maxCount do
        --保证最少次数
        if count >= minCount then
            --概率退出
            if not self.battle:IsRandLt(chance) then
                break
            end
            --概率通过，衰减直至最小概率
            chance = chance - chanceDamping
            chance = common.clamp(chance,chanceMin,10000)
        end

        --删掉不合法的
        common.removecEx(targets,filter_last,self.caster,self.tabEx)
        if #targets == 0 then
            break
        end
        --随机选取不是之前目标的角色
        local tmp = lastTarget
        lastTarget = self.battle:RandomSelectOne(targets)

        if not doHit() then
            break
        end

        --把上上次目标重新插入目标列表
        table.insert(targets,tmp)
        count = count + 1
    end
end

function SkillProcess.TargetFilterEx(target,caster,tabEx)
    
    --非复活技能 且 目标必须存活的 检查目标是否存活
	if not SkillProcess.Utils.IsRelive(tabEx) and tabEx.IgnoreTargetDead ~=1 and not target:IsAlive() then
		return true
    end
    
    if tabEx.IgnoreTargetDead ~=1 then
        if target.isHeroCard and not caster.isHeroCard then
            return true
        end
    end

    --目标必须是 1敌方，2我方，3自己，4我方死亡目标
    if tabEx.TargetMustBeType == 1 then
        if target.side ~= caster:GetOppositeSide() then
			return true
        end
    elseif tabEx.TargetMustBeType == 2 then
        if target.side ~= caster.side then
			return true
        end
    elseif tabEx.TargetMustBeType == 3 then
        if target.id ~= caster.id then
			return true
        end
    elseif tabEx.TargetMustBeType == 4 then
        if target.side ~= caster.side or target:IsAlive() then
			return true
        end
    end
	
	return false
end

function SkillProcess.TargetFilter(r)
    if not r:IsAlive() then
        return true
    end

    if not r.isValid then
        return true
    end

    if r:IsNothingness() then
        return true
    end

    return false
end

function SkillProcess.TargetFilterByHasBuff(r,impactClass)
    local buffs = r:GetBuffsByImpactClass(impactClass)
    if buffs == nil or #buffs == 0 then
        return true
    end
    return false
end

function SkillProcess.GetMarkedTargets(role,impactId, tabEx)
    local targets = role.battle:GetRoleEnemies(role)
    if targets == nil then
        return nil
    end
	
    common.removecEx(targets,function(r,caster,tab) 
        return SkillProcess.TargetFilterEx(r,caster,tab) or not r:HasImpact(impactId,caster)
    end,
	role,
	tabEx)
    return targets
end

function SkillProcess:SendImpacts2Target(targetId,hitTab,isDirective)
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

    if targetRole:IsNothingness() then
        return nil,false
    end


    --hit阶段发生的事件，打包到hitresult里
    self.battle:NewEventGroup()

    --计算是否触发抵抗
    if targetRole:IsImmueHit(hitTab) then
        targetRole:NotifyImmue()
        return {
            targetID = targetId,
            events = self.battle:PopEventGroup(),
        },false
    end

    -- --计算是否产生了环境抵抗
    -- local hasEnvResist = nil
    -- if ImpactUtils.CalcEnvEffect(self.caster,targetRole) < 0 then
    --     --概率产生环境抵抗
    --     hasEnvResist = true
    -- end

    --产生impact
    for i,impactId in ipairs(hitTab.Impact) do
        
        local chance = hitTab.Chance[i]
        local isRefix = hitTab.IsChanceRefix[i] > 0
        local canSend = false
        if isRefix then
            --计算效果命中影响
            local ret = SkillProcess.CheckImpactChance(self.battle
                        ,chance
                        ,self.caster:GetAttrValue(AttrType.ImpactChance)
                        ,targetRole:GetAttrValue(AttrType.ImpactResist)
                    )

            if ret == 0 then
                canSend = true
            elseif ret == -1 then
                canSend = false
            elseif ret == -2 then
                canSend = false
                --提示抵抗
                targetRole:NotifyResist()
            end
        else
            canSend = self.battle:IsRandLt(chance)
        end

        local itype = type(impactId)
        local isValidImpact = (itype == 'number' and impactId > 0) or (itype == 'table')

        if isValidImpact and canSend and self:CanSendImpact(impactId,hitTab) then
            Impact.SendImpactToTarget(impactId,targetRole,self.caster,nil,{
                skillProcess = self,
                hitTab = hitTab,
                isDirective = isDirective,
                skillExId = self.tabEx.Id,
                --hasEnvResist = hasEnvResist,
            })
        end
    end

    return {
        targetID = targetId,
        events = self.battle:PopEventGroup(),
    },true
end

--复活效果特写
function SkillProcess:ReliveTarget(targetId,hitId)
    --hit阶段发生的事件，打包到hitresult里
    self.battle:NewEventGroup()

    local reliveRole = self.battle:Relive(targetId,self.tabEx.LogicParam[1])

    local events = self.battle:PopEventGroup()

    if reliveRole == nil then
        return nil
    end

    local hit = {
        hitResults = {
            {
                targetID = targetId,
                events = events,
            }
        }
    }

    if hitId ~= nil and hitId ~= -1 then
        local hitTab = hitId
        if type(hitTab) == 'number' then
            hitTab =  TabMgr.GetByID("SkillHit",hitId)
        end

        if hitTab.IsAnimHit == 1 then
            hit.isAnimHit = true
        end

        self.battle:NewEventGroup()

        --产生impact
        for i,impactId in ipairs(hitTab.Impact) do
            
            local chance = hitTab.Chance[i]
            local canSend = self.battle:IsRandLt(chance)
            local itype = type(impactId)
            local isValidImpact = (itype == 'number' and impactId > 0) or (itype == 'table')
            if isValidImpact and canSend and self:CanSendImpact(impactId,hitTab) then
                Impact.SendImpactToTarget(impactId,reliveRole,self.caster,nil,{
                    skillProcess = self,
                    hitTab = hitTab,
                    isDirective = isDirective,
                    skillExId = self.tabEx.Id,
                    hasEnvResist = hasEnvResist,
                })
            end
        end

        local ret = {
            targetID = reliveRole.id,
            events = self.battle:PopEventGroup()
        }

        table.insert(hit.hitResults,ret)
    end

    return hit
end

function SkillProcess:GetTabEx()
    return self.tabEx
end

function SkillProcess.CheckImpactChance(battle,chance,enhance,resist)
    --先随机一次，通过了后，才进入抵抗、命中
    if battle.gm_ImpactChanceFull then
        return 0
    end

    if not battle:IsRandLt(chance) then
        return -1
    end

    --效果命中、抵抗再次随机
    local chanceRefix = math.ceil(10000 + enhance - resist)
    chanceRefix = math.max(chanceRefix,0)
    --min 1000,max 9000
    chanceRefix = common.clamp(chanceRefix,1000,9000)

    if battle:IsRandLt(chanceRefix) then
        return 0
    else
        return -2
    end
end

function SkillProcess:CanSendImpact(impactId,hitTab)
    local impactTab = TabMgr.GetByID("Impact",impactId)
    if impactTab == nil then
        return false
    end
    
    --混乱时，只能操作普通伤害的效果
    if self.caster:IsChaos() then
        if impactTab.ImpactClass ~= ImpactClass.NormalDamage then
            return false
        end
    end

    if self.caster:IsPassiveDisable() then
        if impactTab.IsPassiveImpact == 1 then
            return false
        end
    end

    return true
end

local Utils = {}

SkillProcess.Utils = Utils

Utils.IsSkillClass = function(tabBase,skillClass)
    local tabClass = tabBase
    if type(tabBase) ~= "number" then
        tabClass = tabBase.SkillClass
    end
    return tabClass & skillClass ~= 0
end

Utils.IsPassiveSkill = function(tabBase)
    return Utils.IsSkillClass(tabBase,SkillClass.Passive)
end

Utils.CanSilence = function(tabBase)
    return Utils.IsSkillClass(tabBase,SkillClass.CanSilence)
end

Utils.IsCommonSkill = function(tabBase)
    return not Utils.IsSkillClass(tabBase,SkillClass.SpecailSkill)
end

Utils.IsNormalAttack = function(tabBase)
    return Utils.IsSkillClass(tabBase,SkillClass.Attack)
end

Utils.IsRelive = function(tabEx)
    return tabEx.LogicID == SkillLogicType.Relive
end

Utils.GetSkillByBase = function(baseId,lv)

    local tabLv = TabMgr.GetByID("SkillLevels",baseId)
    if tabLv == nil then
        return -1
    end

    return tabLv.Level[lv]
end

function SkillProcess:CanSilence()
    return Utils.CanSilence(self.tabBase)
end

--是否是普攻
function SkillProcess:IsNormalAttack()
    return Utils.IsNormalAttack(self.tabBase)
end

function SkillProcess:IsCommonSkill()
    return Utils.IsCommonSkill(self.tabBase)
end

function SkillProcess:IsReliveSkill()
    return Utils.IsRelive(self.tabEx)
end

return SkillProcess