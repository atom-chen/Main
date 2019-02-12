
require('class')
local common = require("common")

local Role = class("Role")

local Attr = require("BattleCore/RoleAttrs")
local AttrType = require("BattleCore/Common/AttrType")
local BattleCommonData = require("BattleCore/Common/BattleCommonData")
local BattleSide = require("BattleCore/Common/BattleSide")
local BuffContainer = require("BattleCore/BuffContainer")
local CDCounter = require("BattleCore/CDCounter")
local tabMgr = require('TabManager')
local SkillTargetType = require('BattleCore/Common/SkillTargetType')
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')
local PassiveSkillProcess = require("BattleCore/SkillProcess/PassiveSkillProcess")
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local BattleActionType = require("BattleCore/ActionType")
local ImpactSubClass = require("BattleCore/Common/ImpactSubClass")
local CalcParams = require("BattleCore/CalcParams")
local AI = require("BattleCore/AIExtend/AI")
local SPGainer = require("BattleCore/SPGainer")
local ImpactUtils = require('BattleCore/Impact/ImpactUtils')
local BattleAreaType = require('BattleCore/Common/BattleAreaType')
local RoleType = require('BattleCore/Common/RoleType')
local ImpactTags = require("BattleCore/Common/ImpactTags")
local BubbleType = require("BattleCore/Common/BubbleType")
local BattleState = require("BattleCore/Common/BattleState")
local SS = require("BattleCore/SkillProcess/ScriptSkillParser")

local warn = common.mk_warn('Role')
local debuglog = common.debuglog

local RoleStatusFlag = {
    None = 0,
    ClearBuff = 1,
    ClearCD = 2,
    GainHP = 4,
    DescCooldown = 8,
}

--无论接受者是否死亡，都会投递的消息
local MsgIgnoreDead = common.arrayToSet({
    "msgBeforeDead",
    "bcBeforeDead",
    "msgDead",
    'bcRoleDead',
})

function Role:Init(initData)
    --缓存一下初始化数据，复活用
    self.cachedInitData = initData

    self.attrs = Attr.RoleAttrTb.Create()
    local attrs = initData.roleInfo.attrs
    self.attrs:Init(initData.roleInfo.attrs)

    self.roleBaseID = initData.roleInfo.roleBaseID
    self.isHeroCard = initData.roleInfo.isHeroCard
    
    if rawget(initData,'status') ~= nil then
        self.ap = initData.status.ap
        self.hp = initData.status.hp
        self.hasStatus = true
    else
        self.ap = 0
        self.hp = self.attrs:GetValue(AttrType.MaxHP)
    end

    self.orgMaxHP = self:GetMaxHP()
    self.shiledValue = 0

    if rawget(initData.roleInfo,'visualInfo') ~= nil then
        self.visualInfo = initData.roleInfo.visualInfo
    end

    local monsterId = rawget(initData.roleInfo,'monsterId')
    if monsterId ~= nil then
        self.monsterId = monsterId
    end

    self.userObjId = rawget(initData,'userObjId') or nil
    if self.userObjId == -1 then
        self.userObjId = nil
    end

    local heroId = rawget(initData.roleInfo,"heroId")
    if heroId ~= nil then
        self.heroId = heroId
    end

    --print('new role',self.roleBaseID,self.userObjId)
    
    if self.userObjId == nil then    --没有玩家操作他
        self.isAI = true
    end

    self.isAuto = false

    local cardId = rawget(initData.roleInfo,'cardId')
    if cardId ~= nil then
        self.cardId = cardId
    end

    self.skills = {}
    self.skillLvs = {}
    if initData.roleInfo.skillInfos ~= nil then
        for _,skillInfo in ipairs(initData.roleInfo.skillInfos) do
            if skillInfo.skillID >= 0 then
                table.insert( self.skills, skillInfo.skillID)                
                table.insert(self.skillLvs,skillInfo.level)
            end
        end
    end

    self.skillCooldowns = {}
    self.side = initData.side
    self.battlePos = initData.battlePos
    self.battlePosArea = initData.battlePosArea

    self.buffContainer = new(BuffContainer)
    self.buffContainer:Init(self)

    self.passiveBuffContainer = new(BuffContainer)
    self.passiveBuffContainer:Init(self,10000)
    self.passiveBuffContainer:SetNeverNotify()

    if self.isHeroCard then
        self.spGainer = new(SPGainer)
        self.spGainer:Init(self)
    end

    self.cdCounter = new(CDCounter)

    self.isValid = true
    self.deading = false
    self.dead = false

    self.castCount = 0

    self.roleBaseTab = tabMgr.GetByID("RoleBaseAttr",self.roleBaseID)
    if self.roleBaseTab == nil then
        warn("roleBase is nil",self.roleBaseID)
    end
    self.envType = self.roleBaseTab.EnvType

    local aiID = rawget(initData,'ai')
    if aiID ~= nil and aiID ~= -1 then
        local aiTab = tabMgr.GetByID("AI",aiID)
        if aiTab ~= nil then
            self.aiTab = aiTab
            if aiTab.ExtendLogic ~= -1 then
                self.aiExtend = AI.CreateAI(aiTab.ExtendLogic,self)
            end
        end
    end
	--Boss分部
	self.AvatarBindingTab = nil
	if self.roleBaseTab.AvatarBinding and self.roleBaseTab.AvatarBinding ~= -1 then
		self.AvatarBindingTab = tabMgr.GetByID("AvatarBinding",self.roleBaseTab.AvatarBinding)
	end
end

function Role:OnDel()
    --如果是召唤物，需要告知召唤者
    if self.summonOnwerId and self.summonOnwerId ~= -1 and self.battle ~= nil then
        local onwer = self.battle:GetRoleById(self.summonOnwerId)
        if onwer ~= nil and onwer.isValid then
            if onwer.summonRoles ~= nil then
                onwer.summonRoles[self.id] = nil
            end
        end
    end
end

--是否是召唤物
function Role:IsSummon()
	if self.summonOnwerId and self.summonOnwerId ~= -1 then
		return true
	end	
	return false
end

--是否是Boss分部主体
function Role:IsAvatarPartsMain()
	if self.AvatarBindingTab == nil then
		return false
    end
    if self.roleBaseTab.Id == self.AvatarBindingTab.MainId then
        return true
    end
	return false
end


function Role:IsApFull()
    return self.ap >= BattleCommonData.AP_FULL
end

function Role:SetHP(hp)
    local maxHP = self.attrs:GetValue(AttrType.MaxHP)
    self.hp = common.clamp(hp,0,maxHP)
    self:SendMessage("msgHPChange",self.hp,maxHP)
end

function Role:GetMaxHP()
    return self.attrs:GetValue(AttrType.MaxHP)
end

function Role:GetHPPercent10000()
    return math.ceil(10000 * ( self:GetHP() / self:GetMaxHP() ) )
end

function Role:CanBeHeal()
    return self:GetAttrValue(Attr.HidenAttrType.NeverHeal) <= 0
end

function Role:IsSilentAttack()
    return self:GetAttrValue(Attr.HidenAttrType.SilentAtk) >= 1
end

function Role:SetAP(ap)
    self.ap = common.clamp(ap,0,BattleCommonData.AP_FULL*2,ap)
end

function Role:IncAP()
    self:SetAP(self.ap + self.randomRoundSpeed)
end

function Role:GetHP()
    return self.hp
end

function Role:GetAP()
    return self.ap
end

function Role:CostSP(cost)
    if self.spGainer == nil then
        return
    end
    self.spGainer:CostSP(cost)
end

function Role:GainSP(sp)
    if self.spGainer == nil then
        return
    end
    self.spGainer:GainSP(sp)
end

function Role:GetOppositeSide()
    if self.side == BattleSide.bs_Blue then
        return BattleSide.bs_Red
    elseif self.side == BattleSide.bs_Red then
        return BattleSide.bs_Blue
    end
    return BattleSide.bs_None
end

function Role:IsAlive()
    return not self.deading and not self.dead
end

function Role:GetAttrValue(attrType)
    return self.attrs:GetValue(attrType)
end

function Role:AddAttrRefix(refix)

    --如果是降低血上限，则需要修正一下
    --boss类的，效果减半
    if refix.attrType == AttrType.MaxHP then
        if self.roleBaseTab ~= nil and self.roleBaseTab.Type == RoleType.Boss then
            if refix.addition < 0 then
                refix.addition = math.ceil(refix.addition * 0.5)
            end
            if refix.percent < 0 then
                refix.percent = math.ceil(refix.percent * 0.5)
            end
        end
    end

    --当前血上限
    local maxHP = self:GetMaxHP()

    self.attrs:AddRefix(refix)

    if refix.attrType == AttrType.MaxHP then
        --血上限发生变化，通知一下
        self:MaxHPChange(maxHP)
    end
end

function Role:SetAttrsDirty(attrIndex)
    self.attrs:SetDirty(attrIndex)
end

function Role:RemoveAttrRefix(refix)
    local maxHP = self:GetMaxHP()

    self.attrs:RemoveRefix(refix)

    if refix.attrType == AttrType.MaxHP then
        self:MaxHPChange(maxHP)
    end
end

function Role:MaxHPChange(lastMaxHP)
    local maxHP = self:GetMaxHP()

    if maxHP == lastMaxHP then
        return
    end
    
    local dmg = 0
    
    if self.hp > maxHP then
        dmg = self.hp - maxHP
        --血量变化
        self:SetHP(maxHP)
    end

    --通知客户端
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            --senderID = ret.senderId,
            targetID = self.id,
            val = maxHP,
            hitType = HitType.MaxHPChange,
            param0 = dmg,
            param1 = self:GetMaxHpReduce(),
        }
    })
end

function Role:CanAct(tab)
    --根据配置 可跳过检查
    if tab == nil or tab.IgnoreSelfDead ~= 1 then
        if not self:IsAlive() then
            return false
        end
    end

    if tab == nil or tab.IgnoreSelfState ~= 1 then
        if self.buffContainer:IsStun() then
            return false
        elseif self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Ice) then
            return false
        elseif self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Sleep) then
            return false
        end
    end
    return true
end

function Role:IsSilence()
    return self.buffContainer:IsSilence()
end

function Role:IsTaunt()
    return self.buffContainer:IsTaunt()
end

function Role:IsChaos()
    return self.buffContainer:IsChaos()
end

function Role:RefixImpactSend(impact)
    self.buffContainer:RefixImpactSend(impact)
    self.passiveBuffContainer:RefixImpactSend(impact)
end

function Role:RefixImpactRecv(impact)
    self.buffContainer:RefixImpactRecv(impact)
    self.passiveBuffContainer:RefixImpactRecv(impact)
end

function Role:AddBuff(impact,notNotify)
    self.buffContainer:AddBuff(impact,notNotify)
end

function Role:RemoveBuff(impact,notNotify)
    return self.buffContainer:RemoveBuff(impact,notNotify)
end

function Role:NotifyBuffChange()
    self.buffContainer:NotifyBuffChange()
end

function Role:ReCalcSpeed()
    --随机速度(乱敏)
    local p = self.battle:Random(10000 - CalcParams.P6,10000 + CalcParams.P6) / 10000
    self.randomRoundSpeed = math.ceil(self:GetAttrValue(AttrType.Speed) * p)
end

function Role:NewWaveStart(waveTab,waveIndex)
    --根据不同的模式

    if waveTab.RoleStatusFlag ~= RoleStatusFlag.None and waveIndex > 1 then
        --决定buff是否保留
        if waveTab.RoleStatusFlag & RoleStatusFlag.ClearBuff ~= 0 then
            if self.buffContainer ~= nil then
                self.buffContainer:Clear()
            end
        end
    
        if waveTab.RoleStatusFlag & RoleStatusFlag.ClearCD ~= 0 then
            self:ClearAllCooldown()        
        end

        if waveTab.RoleStatusFlag & RoleStatusFlag.DescCooldown ~= 0 then
            --减cd
            self:DescAllCooldown(CalcParams.WaveStartDescCoolDown)
        end

        if waveTab.RoleStatusFlag & RoleStatusFlag.GainHP ~= 0 and not self.isHeroCard then
            --回血
            local val = math.ceil( self:GetMaxHP() * (CalcParams.WaveStartGainHPPercent / 10000) )
            self:RecvHeal({
                value = val,
                senderId = self.id
            })
        end
    end

    self.ap = 0
end

--护盾
function Role:SetShield(val)
    self.shiledValue = val

    self:NotifyHit(HitType.ShiledChange,val)
end

function Role:IncShiled(val)
    if math.abs(val) > 0 then
        self:SetShield(self.shiledValue + val)
    end
end

function Role:GetShiled()
    return self.shiledValue
end

function Role:RoundBegin()
    self:SendMessage("msgRoundJustBegin")  --回合刚刚开始，什么都没更新时
    if self.cdCounter:Cooldown() then
        self:NotifyCD()
    end

    --高频，直调的形式加速
    self.buffContainer:RoundBegin()
    if not self:IsPassiveDisable() then
        self.passiveBuffContainer:RoundBegin()
    end

    self:SendMessage("msgRoundBegin")
    self.ap = 0

    if not self:CheckCanRoundBegin() then
        return false
    end
    
    if not self.battle.shouldStartRound and ( self.isAI or self.isAuto ) then
        self:ProcessAI()
        return false
    else
        return true
    end

end

function Role:CheckCanRoundBegin(notNotify)
    --死了
    if not self:IsAlive() then
        return false
    end

    if not self:CanAct() then
        --没法操作，不需要输入
        self.battle:AddEvent({
            type = BattleEventType.Idle,
            idleEvent = {
                time = 0.5,
            }
        })
        return false
    end

    --混乱、嘲讽，不能输入
    if self:IsChaos() then
        self:ProcessChaos()
        return false
    end

    if self:IsTaunt() then
        self:ProcessTaunt()
        return false
    end

    --有技能指令
    if self.skillCmd ~= nil then
        local cmd = self.skillCmd
        self.skillCmd = nil
        self:ActUseSkillByIndex(cmd[1],cmd[2])
        return false
    end

    --主角再次修改
    -- if self.isHeroCard then
    --     return false
    -- end

    --检查一下是否有可用技能
    if not self:CanUseAnySkill(notNotify) then
        return false
    end

    return true
end

function Role:CanUseAnySkill(notNotify)
    --检查技能是否可以放
    local validSkills = {}
    for _,skillId in ipairs(self.skills) do
        local validSkillId = self:GetSkillIdFitEnv(skillId)
        if self:CheckCanUseSkill(validSkillId) then
            table.insert( validSkills, validSkillId )
        end
    end

    if #validSkills == 0 then
        if not self.isAI and not notNotify then
            --提示无可用技能
            self.battle:Notice("#{46}")
        end
        return false
    end

    --检查可用的技能，是否有可选目标
    local isAnySkill = false
    local battle = self.battle
    for _,skillId in ipairs(validSkills) do
        local targets = self:GetValidSkillTargets(skillId)
        if targets ~= nil and #targets ~= 0 then
            isAnySkill = true
            break
        end
    end

    if not isAnySkill then
        if not self.isAI and not notNotify then
            --提示无合适的目标
            self.battle:Notice("#{47}")
            return false
        end
    end

    return true
end

function Role:BeforeRoundEnd()
    self:SendMessage("msgBeforeRoundEnd")
end

function Role:RoundEnd()
    --高频，直调的形式加速
    self.buffContainer:RoundEnd()
    if self:IsPassiveDisable() then
        self.passiveBuffContainer:RoundEnd()
    end

    self:SendMessage("msgRoundEnd")
end

function Role:BeginCooldown(skillExId,id,time)

    if self.battle ~= nil and self.battle.gm_noCD then
        return
    end

    -- local ret = {cdId = id,time = time,skillExId = skillExId}
    -- self.buffContainer:CallImpactFunc("RefixCD",ret)
    -- self.passiveBuffContainer:CallImpactFunc("RefixCD",ret)
    
    -- if ret.time > 0 then
    --     self.cdCounter:Begin(ret.cdId,ret.time)
    --     self:NotifyCD()
    -- end

    if time > 0 then
        if self.cdCounter:Begin(id,time) then
            self:NotifyCD()
        end
    end

end

function Role:ClearCooldown(id)
    if self.cdCounter:ClearCooldown(id) then
        self:NotifyCD()
    end
end

function Role:IncCooldown(id,val)
    if val == 0 then
        return
    end
    self.cdCounter:IncCooldown(id,val)
    self:SendMessage("msgIncCooldown",val)

    self:NotifyCD()
end

function Role:DescCooldown(id,val)
    if val == 0 then
        return
    end
    self.cdCounter:IncCooldown(id,-val)
    self:SendMessage("msgDescCooldown",-val)

    self:NotifyCD()
end

function Role:GetCooldown(id)
    return self.cdCounter:GetCooldown(id)
end

function Role:IsCooldowning(id)
    return self:GetCooldown(id) > 0
end

function Role:ClearAllCooldown()
    if self.cdCounter:Clear() then
        self:NotifyCD()
    end
end

function Role:IncAllCooldown(val)
    if val == 0 then
        return
    end
    self.cdCounter:IncAllCooldown(val)
    self:SendMessage("msgIncCooldown",val)

    self:NotifyCD()
end

function Role:DescAllCooldown(val)
    if val == 0 then
        return
    end
    self.cdCounter:IncAllCooldown(-val)
    self:SendMessage("msgDescCooldown",-val)

    self:NotifyCD()
end

function Role:GetMaxHpReduce()
    return math.max(0,self.orgMaxHP - self:GetMaxHP())
end

function Role:GetSync(isSyncAllAttrs)
    local info = {
        roleBaseID = self.roleBaseID,
        roleID = self.id,
        hp = self.hp,
        ap = self.ap,
        maxHP = self:GetAttrValue(AttrType.MaxHP),
        actIndex = self:GetActIndex(),
        battlePos = self.battlePos,
        battlePosArea = self.battlePosArea,
        side = self.side,
        isHeroCard = self.isHeroCard,
        cardId = self.cardId,
        monsterId = self.monsterId,
        userObjId = self.userObjId,
        isWaitingAct = self:IsWaitingAct(),
        visualInfo = self.visualInfo,
        summonOnwerId = self.summonOnwerId,
        heroId = self.heroId,
        maxHpReduce = self:GetMaxHpReduce(),
        shiled = self:GetShiled(),
        speed = self:GetAttrValue(AttrType.Speed),
    }

    if self.spGainer ~= nil then
        info.sp = self.spGainer:SP()
    end

    info.skills = self.skills
    info.skillCooldowns = self.cdCounter:Pack()
    info.impacts = self.buffContainer:GetBuffSync(isSyncAllAttrs)
    --把被动也同步了
    --if isSyncAllAttrs then
        local passives = self.passiveBuffContainer:GetBuffSync(isSyncAllAttrs)
        for _,v in ipairs(passives) do
            table.insert(info.impacts,v)
        end
    --end

    if isSyncAllAttrs then
        local attrs = {}
        for _,attrType in pairs(AttrType) do
            table.insert( attrs,{
                type = attrType,
                value = self:GetAttrValue(attrType),
            })
        end
        info.fullAttrs = {attrs = attrs}
    end

    return info
end

function Role:IsWaitingAct()
    if self.ap >= BattleCommonData.AP_FULL then
        return true
    end

    if self.battle == nil or self.battle.actQ == nil then
        return false
    end

    if self.battle.actQ:IsInPriorityQueue(self) then
        return true
    end

    return false
end

function Role:GetActIndex()
    if self.battle == nil or self.battle.actQ == nil then
        return -1
    end
    return self.battle.actQ:GetActIndex(self)
end

function Role:ProcessAI()
    if self.aiExtend ~= nil then
        self.aiExtend:ProcessAI()
    elseif self.isHeroCard then
        self:ProcessSequenceCastSkillAI()
    else
        self:ProcessNormalAI()
    end
end

function Role:ProcessNormalAI()
    local validSkills = {}
    local battle = self.battle
    for i,skillId in ipairs(self.skills) do
        --跟前当前环境选取
        local validSkillId = self:GetSkillIdFitEnv(skillId)
        local lv = self:GetSkillLvByIndex(i)
        if lv >= 0 and self:CheckCanUseSkill(validSkillId) then
            table.insert(validSkills,validSkillId)
        end
    end

    local count = #validSkills
    if count == 0 then
        return
    end
    -- local randIndex = battle:Random(1,count)
    -- local randSkillId = validSkills[randIndex]

    -- --先尝试随机放
    -- if self:AI_TryCast(randSkillId) then
    --     return
    -- end

    -- --失败，删掉不能放，顺序尝试
    -- table.remove(validSkills,randIndex)

    -- for _,skillId in ipairs(validSkills) do
    --     if self:AI_TryCast(skillId) then
    --         return
    --     end
    -- end

    --新AI规则，有大放大
    local notPerfectSkillId = -1
    for i=count,1,-1 do
        local skillId = validSkills[i]
        --可以放，但是不太合适
        if not self:AI_ShouldUse(skillId) then
            if notPerfectSkillId == -1 then
                notPerfectSkillId = skillId                
            end
        else
            if self:AI_TryCast(skillId) then
                return
            end
        end
    end

    --没放技能，看看是否有不太合适才不放的技能
    if notPerfectSkillId ~= -1 then
        --放这个不合适但是能放的技能
        self:AI_TryCast(notPerfectSkillId)
    end
end

function Role:AI_ShouldUse(skillId)
    local tab = tabMgr.GetByID("SkillEx",skillId)
    if tab == nil then
        return false
    end
    if tab.LogicID == -1 then
        return false
    end
    if tab.SkillAI == -1 then
        return true
    end
    return self:AI_ShouldUseIter(tab.SkillAI,0)
end

function Role:AI_ShouldUseIter(skillAiId,counter)
    --print("shoulduse",skillAiId)
    if counter > 10 then
        warn("Skill AI recursion too much.",skillAiId)
        return true
    end
	
	local skillAI = require("BattleCore/SkillAI/SA_"..skillAiId)
	if skillAI == nil then
		warn('skillAI is nil!!!! ',skillAiId)
	else
		if skillAI.ShouldUse ~= nil then
			return skillAI.ShouldUse(self)
		else
			warn('skillAI.ShouldUse is nil!!!! ',skillAiId)
		end
	end
	
    return false
end

--Buff数量  ByImpactClass AI_OurAllBuffCount
function Role:AI_IsBuffCountByImpactClass(impactClass, count, oper)

	local t = self.buffContainer:GetBuffsByImpactClass(impactClass)
	if t ~= nil then
		if oper == 0 then --大于
			return #t > count
		else --小于
			return #t < count
		end
	end
	
	return false
end

--Buff数量  ByImpactSubClass
function Role:AI_IsBuffCountByImpactSubClass(subClass, count, oper)

	local t = self.buffContainer:GetBuffsByImpactSubClass(subClass)
	if t ~= nil then
		if oper == 0 then --大于
			return #t > count
		else --小于
			return #t < count
		end
	end
   
	return false
end

--Buff数量  ByImpactId
function Role:AI_IsBuffCountByImpactImpactId(impactId, count, oper)

	local t = self.buffContainer:GetBuffsByImpactId(impactId)
	if t ~= nil then
		if oper == 0 then --大于
			return #t > count
		else --小于
			return #t < count
		end
	end
   
	return false
end

--自己血量小于等于
function Role:AI_ShouldUse_HPLe(HPLe)
    if HPLe ~= -1 then
        if self:GetHPPercent10000() > HPLe then
            return false
        end
    end
    return true
end

--自己血量大于等于
function Role:AI_ShouldUse_HPMore(HPLe)
    if HPLe ~= -1 then
        if self:GetHPPercent10000() < HPLe then
            return false
        end
    end
    return true
end

--全员平均血量，小于等于
function Role:AI_ShouldUse_OurAverageHPLe(OurAverageHPLe)
    if OurAverageHPLe ~= -1 then
        local roles = self.battle:GetRoleAlies(self)        
        local max = 0
        local hp = 0
        for _,role in ipairs(roles) do
            if role:IsAlive() then
                max = max + role:GetMaxHP()
                hp = hp + role:GetHP()
            end
        end
        
        local percent = math.ceil( 10000 * (hp / max) )
        if percent > OurAverageHPLe then
            return false
        end
    end
    return true
end

--任意一员，血量小于等于
function Role:AI_ShouldUse_AnyHPLe(AnyHPLe)
    if AnyHPLe ~= -1 then
        local roles = self.battle:GetRoleAlies(self)            
        local pass = false
        for _,role in ipairs(roles) do
            if role:IsAlive() then
                if role:GetHPPercent10000() <= AnyHPLe then
                    pass = true
                    break
                end
            end
        end
        if not pass then
            return false
        end
    end
    return true    
end

--是否处于某个环境下
function Role:AI_ShouldUse_IsEnv(IsEnv)
    if IsEnv == -1 then
        return true
    end

    return self.battle.curEnvType == IsEnv
end

--是否处于我方优势环境下，根据我方优势符灵数量，对比对方优势符灵数量
function Role:AI_ShouldUse_OurAllBetterEnv(OurAllBetterEnv)
    if OurAllBetterEnv == - 1 then
        return true
    end

    local envCounter0 = self.battle:CountEnvRole(self.battle.curEnvType,self.side)
    local envCounter1 = self.battle:CountEnvRole(self.battle.curEnvType,self:GetOppositeSide())

    if OurAllBetterEnv == 0 then
        return envCounter0 <= envCounter1
    else
        return envCounter1 > envCounter0
    end

    return false
end

--我方特定buff数量和是否大于指定数量
function Role:AI_ShouldUse_OurAllBuffCount(param)
    --local param = tab.OurAllBuffCount
    if param[1] == -1 then
        return true
    end
    
    local impactClass = param[1]
    local oper = param[2]
    local sum = param[3]

    local ours = self.battle:GetRoleAlies(self)
    local counter = 0
    for _,r in ipairs(ours) do
        if r:IsAlive() then
            local t = r.buffContainer:GetBuffsByImpactClass(impactClass)
            if t ~= nil then
                counter = counter + #t
            end
        end
    end

    if oper == 0 then
        return counter > sum
    else
        return counter < sum
    end
end

--正定方全员指定buff类型数量小于or大于
function Role:AI_IsOurAllBuffCountByImpactClass(impactClass, sum, oper, enemy)
	local ours
	if enemy == true then
		ours = self.battle:GetRoleEnemies(self)
	else
		ours = self.battle:GetRoleAlies(self)
	end
    local counter = 0
    for _,r in ipairs(ours) do
        if r:IsAlive() then
            local t = r.buffContainer:GetBuffsByImpactClass(impactClass)
            if t ~= nil then
                counter = counter + #t
            end
        end
    end

    if oper == 0 then
        return counter > sum
    else
        return counter < sum
    end
end

function Role:AI_IsOurAllBuffCountByImpactSubClass(subClass, sum, oper, enemy)
	local ours
	if enemy == true then
		ours = self.battle:GetRoleEnemies(self)
	else
		ours = self.battle:GetRoleAlies(self)
	end
    local counter = 0
    for _,r in ipairs(ours) do
        if r:IsAlive() then
            local t = r.buffContainer:GetBuffsByImpactSubClass(subClass)
            if t ~= nil then
                counter = counter + #t
            end
        end
    end

    if oper == 0 then
        return counter > sum
    else
        return counter < sum
    end
end

function Role:AI_IsOurAllBuffCountByImpactImpactId(impactId, sum, oper, enemy)
	local ours
	if enemy == true then
		ours = self.battle:GetRoleEnemies(self)
	else
		ours = self.battle:GetRoleAlies(self)
	end
    local counter = 0
    for _,r in ipairs(ours) do
        if r:IsAlive() then
            local t = r.buffContainer:GetBuffsByImpactId(impactId)
            if t ~= nil then
                counter = counter + #t
            end
        end
    end

    if oper == 0 then
        return counter > sum
    else
        return counter < sum
    end
end

--我方角色死亡数量大于等于
function Role:AI_ShouldUse_OurRoleDead(OurRoleDeadCount)

    if OurRoleDeadCount <= 0 then
        return true
    end

    if self.battle:CountRoleDead(self.side) < OurRoleDeadCount then
        return false
    end

    return true
end

--顺序释放技能的AI
function Role:ProcessSequenceCastSkillAI()
    local validSkills = {}
    local battle = self.battle
    for i,skillId in ipairs(self.skills) do
        --跟前当前环境选取
        local validSkillId = self:GetSkillIdFitEnv(skillId)
        local lv = self:GetSkillLvByIndex(i)
        if lv >= 0 and self:CheckCanUseSkill(validSkillId) then
             table.insert(validSkills,validSkillId)
        end
        --table.insert(validSkills,validSkillId)
    end

    local count = #validSkills
    if count == 0 then
        return
    end

    self.seqCastIndex = self.seqCastIndex or 0
    
    --按照顺序尝试使用
    for i=1,count do
        self.seqCastIndex = self.seqCastIndex + 1
        if self.seqCastIndex > count then
            self.seqCastIndex = 1
        end

        local skillId = validSkills[self.seqCastIndex]

        if self:AI_ShouldUse(skillId) then
            if self:AI_TryCast(skillId) then
                return
            end
        end
    end

end

function Role:AI_TryCast(skillId)

    if skillId == nil then return end
    if skillId == -1 then return end

    if not self:CheckCanUseSkill(skillId) then
        return false
    end

    local targetRole = self:AI_SelectSkillTarget(skillId)
    if targetRole == nil then
        return false
    end

    self:ActUseSkillByID(skillId,targetRole.id)
    return true
end

function Role:AI_SetPriorityTarget(target)
    if target == nil then
        return
    end
    if target.isValid and target:IsAlive() then
        if target.side == self.side then
            self.aiOurPriorityTarget = target
        else
            self.aiPriorityTarget = target
        end
    end
end

--技能目标排序
local sortTargetFunc = function(r1,r2)

    if r1:GetHPPercent10000() ~= r2:GetHPPercent10000() then
        return r1:GetHPPercent10000() > r2:GetHPPercent10000()
    end

    if r1:GetHP() ~= r2:GetHP() then
        return r1:GetHP() > r2:GetHP()
    end

    return r1.battlePos > r1.battlePos

end

function Role:AI_SelectSkillTarget(skillId)

    local roles = self:GetValidSkillTargets(skillId)
    if roles == nil or #roles == 0 then
        return nil
    end

    --1,是否有优先目标，并且是合法的技能目标
    local currPriorityTarget = self.aiPriorityTarget
    if currPriorityTarget ~= nil then
        if currPriorityTarget.isValid and currPriorityTarget:IsAlive() then
            for _,r in ipairs(roles) do
                if r == currPriorityTarget then
                    return r
                end
            end
        else
            self.aiPriorityTarget = nil
        end
    end

    --1,是否有优先友方目标，并且是合法的技能目标
    local currOurPriorityTarget = self.aiOurPriorityTarget
    if currOurPriorityTarget ~= nil then
        if currOurPriorityTarget.isValid and currOurPriorityTarget:IsAlive() then
            for _,r in ipairs(roles) do
                if r == currOurPriorityTarget then
                    --print("select our side target")
                    return r
                end
            end
        else
            self.aiOurPriorityTarget = nil
        end
    end

    --1.1，是否有根据技能Id，执行AI选择目标
    if self.aiExtend ~= nil then
        local r = self.aiExtend:SelectSkillTarget(skillId)
        if r ~= nil then
            return r
        end
    end

	--1.2，技能自己的AI是否有优先目标
	local skillAISel = false
	while false == skillAISel do
		skillAISel = true
		local tab = tabMgr.GetByID("SkillEx",skillId)
		if tab == nil then
			break
		end
		if tab.SkillAI == -1 then
			break
		end
		local skillAI = require("BattleCore/SkillAI/SA_"..tab.SkillAI)
		if skillAI == nil then
			warn('AI_SelectSkillTarget SkillAI is nil '..tab.SkillAI)
			break
		end
		if skillAI.IsPriorityTarget == nil then
			break
		end
		for _,r in ipairs(roles) do
			 if r.isValid and r:IsAlive() and skillAI.IsPriorityTarget(r) then
				return r
			end
		end
	end
	
    local smartChance = 4000
    if self.aiTab ~= nil then
        smartChance = self.aiTab.SmartChance
    end

    --概率无脑随机打
    if not self.battle:IsRandLt(smartChance) then
        return self.battle:RandomSelectOne(roles)        
    end
    
    --2.是否有血量低于50%的角色
    local priorityTargets = nil
    for _,r in ipairs(roles) do
        if r:GetHPPercent10000() <= 5000 then
            priorityTargets = priorityTargets or {}
            table.insert(priorityTargets,r)
        end
    end

    if priorityTargets == nil or #priorityTargets == 0 then
        --随机选择一个
        return self.battle:RandomSelectOne(roles)        
    end

    --3.排序（血量百分比、血量绝对值、站位）
    table.sort( priorityTargets, sortTargetFunc )

    return priorityTargets[1]
end

function Role:AI_TryCastByIndex(index)
    local id = self:GetSkillIdByIndex(index)
    if id ~= -1 then
        return self:AI_TryCast(id)
    end
end

function Role:GetSkillIdByIndex(index)

    if index <= 0 or index > #self.skills then
        common.warn('invalid skill index:',tostring(index))
        return -1
    end

    local skillId = self.skills[index]

    local validSkillId = self:GetSkillIdFitEnv(skillId)
    return validSkillId
end

function Role:GetSkillLvByIndex(index)
    if index <= 0 or index > #self.skillLvs then
        common.warn('invalid skill index:',tostring(index))
        return -1
    end

    local lv = self.skillLvs[index]
    return lv
end

function Role:GetSkillIndex(skillId)
    if self.skills == nil then
        return -1
    end
    for i,id in ipairs(self.skills) do
        if id == skillId then
            return i
        end
        local validSkillId = self:GetSkillIdFitEnv(id)
        if validSkillId == skillId then
            return i
        end
    end
    return -1
end

function Role:ActUseSkillByID(skillExId,targetId,data)
    self.battle:AddAction({
        actionType = BattleActionType.UseSkill,
        roleId = self.id,
        skillId = skillExId,
        targetId = targetId,
        data = data,
    })
    return true
end

function Role:ActUseSkillByIndex(index,targetId,data)
    self.battle:AddAction({
        actionType = BattleActionType.UseSkillByIndex,
        roleId = self.id,
        index = index,
        targetId = targetId,
        data = data,
    })
    return true
end

function Role:CastSkillByIndex(index,targetId,data)
    local fitId = self:GetSkillIdByIndex(index)
    if fitId == -1 then
        return false
    end
    local lv = self:GetSkillLvByIndex(index)
    if lv < 0 then
        return false
    end
    local ret = self:CastSkill(fitId,targetId,data)
    return ret
end

function Role:CastSkill(skillExId,targetId,data)

    if skillExId == nil then
        return false
    end

    local stype = type(skillExId)

    if stype == "number" and skillExId == -1 then
        return false
    end

    --健壮性保证，一回合内不能触发太多次技能，防止死循环
    self.castCount = self.castCount + 1
    if self.castCount > 10 then
        common.warn("cast skill failed.to many cast one round.",self.castCount,skillExId,self.id)
        return false
    end

    --修改，使用技能始终适配人妖界
    skillExId = self:GetSkillIdFitEnv(skillExId)

    if not self:CheckCanUseSkill(skillExId,_G.debuglogEnable) then
        common.debuglog("cast skill faied.can not use:",skillExId)
        return false
    end

    --复活类技能，目标死亡也可使用
    local tabEx = tabMgr.GetByID('SkillEx',skillExId)
	--读取脚本化技能配置 
	if tabEx.__script_type == nil and tabEx.ScriptID ~= -1 then
        tabEx = SS.Parse(tabEx,nil)
    end
    if not SkillProcess.Utils.IsRelive(tabEx) then 
        --非复活技能，目标需要存活
        if not self:IsValidSkillTarget(targetId, tabEx) then
            common.debuglog('cast skill failed,target dead',skillExId)
            return false
        end
    end

    --战斗喊话
    if self.castCount <= 1 
        and self.battle.curRoundRole == self 
        and self.battle.state == BattleState.Round
    then
        --自己的回合，第一次使用技能时，概率触发
        self:TryBubbleDirect(BubbleType.UseSkill)
    end

   local process = new(SkillProcess)
   process:Init(skillExId,self,self.battle)
   process.targetSelectedId = targetId
   process.data = data
   process.skillIndex = self:GetSkillIndex(skillExId)
   --warn(self.id .. " cast skill " .. skillExId)\
   --print(self.battle.roundCount,self.battle.state,":",self.id,"cast skill",skillExId,targetId)
   --debuglog(self.battle.roundCount,self.battle.state,":",self.id,"cast skill",skillExId,targetId)
   debuglog(self.id,"cast skill",skillExId,targetId)
   self:SendMessage("msgBeforeUseSkill",process)   
   self.battle:BroadCastMsg('bcBeforeUseSkill',self,process)
   process:Process()
   self:SendMessage("msgAfterUseSkill",process)
   self.battle:BroadCastMsg("bcAfterUseSkill",self,process)

   return true
end

function Role:IsValidSkillTarget(targetId, tabEx)
    local target = self.battle:GetRoleById(targetId)
    if target == nil then
        --common.warn('target not valid',targetId)
        return false
    end

    if not target.isValid then
        return false
    end

	if SkillProcess.TargetFilterEx(target,self,tabEx) == true then
        return false
	end
    
    return true
end

function Role:CheckCanUseSkill(skillid,islog)

    if skillid == nil then return false end

	
	local tabEx = tabMgr.GetByID('SkillEx',skillid)
    if tabEx == nil then
        if islog then
            debuglog("check can use,skillId not found.",skillid)
        end
        return false
    end
    
    if self:IsCooldowning(tabEx.CooldownId) then
        if islog then
            debuglog("check can use,cooldonwing.",skillid)
        end
        return false
    end

    --消耗是否够
    if tabEx.SPCost > 0 then
        if self.spGainer == nil then
            if islog then
                debuglog("check can use,sp not enough.",skillid)
            end
            return false
        end
        if self.spGainer:SP() < tabEx.SPCost then
            if islog then
                debuglog("check can use,sp not enough.",skillid)
            end
            return false
        end
    end

    local tabBase = tabMgr.GetByID('SkillBase',tabEx.BaseID)
    if tabBase == nil then
        if islog then
            debuglog("check can use,skillBase not found.",tabEx.BaseID)
        end
        return false
    end
	--是否是被动
    if SkillProcess.Utils.IsPassiveSkill(tabBase) then
        if islog then
            debuglog("check can use,is passiveSkill.",skillid)
        end
        return false
    end

    --环境限制
    if not self:IsAlwaysDay() and not self:IsAlwyasNight() then
        if tabEx.EnvLimit ~= -1 and self.battle.curEnvType ~= tabEx.EnvLimit then
            if islog then
                debuglog("check can use,env limit.",skillid)
            end
            return false
        end
    end

    --是否被沉默
    if SkillProcess.Utils.CanSilence(tabBase) then
        if self:IsSilence() then
            if islog then
                debuglog("check can use,silence.",skillid)
            end
            return false
        end
    end
	
	--读取脚本化技能配置 
	if tabEx.__script_type == nil and tabEx.ScriptID ~= -1 then
        tabEx = SS.Parse(tabEx,nil)
    end
    --特殊技能

    --睚眦大招，必须得有中了标记标记的敌人
    if tabEx.BaseID == 1162 then
        local targets = SkillProcess.GetMarkedTargets(self,1162210,tabEx)
        if targets == nil or #targets == 0 then
            if islog then
                debuglog("check can use,1162 skill need target has buff 1162210.",skillid)
            end
            return false
        end
    end

	
	
    --确实死了，不能使用技能，deading时可以放
    if tabEx.IgnoreSelfDead ~= 1 and self.dead then
        if islog then
            debuglog("check can use,self is dead.",skillid)
        end
        return false
    end
	--状态检查
	if tabEx.IgnoreSelfState ~= 1 then
		if self.buffContainer:IsStun() then
			if islog then
				debuglog("check can use,self is stun.")
			end
			return false
		elseif self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Ice) then
			if islog then
				debuglog("check can use,self is ice.")
			end
			return false
		elseif self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Sleep) then
			if islog then
				debuglog("check can use,self is sleep.")
			end
			return false
		end
	end
    return true
end

function Role:ClearCooldownByIndex(index)
    if index <= 0 or index > #self.skills then
        --common.warn('invalid skill index:',tostring(index))
        return
    end
    local skillExId = self.skills[index]
    local tabEx = tabMgr.GetByID('SkillEx',skillExId)
    if tabEx == nil then
        return
    end
    if tabEx.CooldownId < 0 then
        return
    end
    self:ClearCooldown(tabEx.CooldownId)
end

function Role:IncCooldownByIndex(index,val)
    if index <= 0 or index > #self.skills then
        --common.warn('invalid skill index:',tostring(index))
        return
    end
    local skillExId = self.skills[index]
    local tabEx = tabMgr.GetByID('SkillEx',skillExId)
    if tabEx == nil then
        return
    end
    if tabEx.CooldownId < 0 then
        return
    end
    self:IncCooldown(tabEx.CooldownId,val)
end

function Role:DescCooldownByIndex(index,val)
    if index <= 0 or index > #self.skills then
        --common.warn('invalid skill index:',tostring(index))
        return
    end
    local skillExId = self.skills[index]
    local tabEx = tabMgr.GetByID('SkillEx',skillExId)
    if tabEx == nil then
        return
    end
    if tabEx.CooldownId < 0 then
        return
    end
    self:DescCooldown(tabEx.CooldownId,val)    
end

function Role:IsPassiveSkill(skillId)
    local tabEx = tabMgr.GetByID('SkillEx',skillId)
    if tabEx == nil then
        return false
    end
    local tabBase = tabMgr.GetByID('SkillBase',tabEx.BaseID)
    if tabBase == nil then
        return false
    end

    return tabBase.SkillClass & 2 ~= 0
end

function Role:TryCastPassive()
    --释放被动
    --目前状态不重置，被动只激活一次
    if self.hasCastPassive then
        return
    end

    self.hasCastPassive = true
    for _,skillId in ipairs(self.skills) do
        if self:IsPassiveSkill(skillId) then
            self:CastPassiveSkill(skillId)
        else
            --主动技能可以附带额外的被动技能
            local tabEx = tabMgr.GetByID('SkillEx',skillId)
            if tabEx ~= nil and tabEx.ChildPassive ~= -1 then
                self:CastPassiveSkill(tabEx.ChildPassive)
            end
        end
    end
end

function Role:CastPassiveSkill(skillId)
    local process = new(PassiveSkillProcess)
    process:Init(skillId,self,self.battle)
    --print(self.id .. " cast passive skill " .. skillId)
    process:Process()

    --被动技能递归，用的情况应该比较少
    local tabEx = tabMgr.GetByID('SkillEx',skillId)
    if tabEx ~= nil and tabEx.ChildPassive ~= -1 then
        self:CastPassiveSkill(tabEx.ChildPassive)
    end
end

function Role:RecvDamage(ret)
    if not self:IsAlive() then
        return
    end

    --主角不能收到伤害
    --TT7575
    if self.isHeroCard then
        return
    end

    --
    if ret.impact ~= nil then
        --dot不能被反击，不能被反伤
        if ret.impact:IsDOT() then
            ret.notReflect = true
            ret.notReverge = true
        end
    end

    ret.targetId = self.id

    --修正伤害
    if not ret.notRefix then
        if ret.senderId ~= nil then
            local attacker = self.battle:GetRoleById(ret.senderId)
            if attacker ~= nil and attacker.isValid and attacker:IsAlive() then
                attacker:RefixSendDamage(ret)
            end            
        end
        self:RefixDamage(ret)        
    end

    --伤害已经没了
    if ret.isImmue then
        return
    end

    --环境效果
    local envEffect = 10000

    if ret.refixByEnv and ret.impact ~= nil then
        --有环境抵制
        if ret.impact:HasEnvResist() then
            envEffect = CalcParams.EnvResistEffect
        elseif ret.impact:HasEnvEnhance() then
            envEffect = CalcParams.EnvEnhnaceEffect
        end

        if envEffect ~= 10000 then
            ret.value = math.ceil(ret.value * (envEffect / 10000))
        end
    end

    if ret.value < 0 then ret.value = 0 end
    ret.validValue = common.clamp(ret.value,0,self.hp)
    self:SetHP(self.hp - ret.value)

    local hitType = ret.hitType

    if hitType == nil then
        hitType = HitType.Damage
        if ret.isCrit then
            hitType = HitType.CritDamage
        end

        if envEffect > 10000 then
            hitType = HitType.EnvEnhance
            if ret.isCrit  then
                hitType = HitType.EnvEnhanceCrit
            end
        elseif envEffect < 10000 then
            hitType = HitType.EnvResist
            if ret.isCrit  then
                hitType = HitType.EnvResistCrit
            end
        end
    end

    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            senderID = ret.senderId,
            targetID = self.id,
            val = ret.value,
            hitType = hitType
        }
    })

    --伤害反弹
    self:DoReflectDamge(ret)

    self:SendMessage('msgRecvDamage',ret)
    self.battle:BroadCastMsg("bcRoleDamage",self.id,ret)

    local sender = self.battle:GetRoleById(ret.senderId)
    if sender ~= nil then
        --造成伤害处理
        sender:SendMessage("msgDamageOther",ret)

        --通用吸血
        if ret.notDrainLife ~= true then
            local drainPercent = sender:GetAttrValue(Attr.HidenAttrType.DrainLife)
            if drainPercent > 0 then
                local heal = math.ceil(ret.value * (drainPercent / 10000))
                if heal > 0 then
                    sender:SendMessage("msgDrainLife",heal)

                    sender:RecvHeal({
                        value = heal,
                        senderId = sender.id,
                    })
                end
            end
        end
    end

    
    if self.battle.gm_HPAutoGain then
        self:SetHP(self:GetMaxHP())
    end

    if self.hp <= 0 then
        self:Die(ret.senderId)
    end

    self.battle:StatistDamage(ret)
end

function Role:DoReflectDamge(ret)
    if ret.senderId == nil then
        return
    end
    
    if ret.notReflect then
        return
    end

    if ret.senderId == self.id then
        return
    end

    local reflectPercent = self:GetAttrValue(Attr.HidenAttrType.DamageReflect)
    if reflectPercent <= 0 then
        return
    end
    local reflectChance = self:GetAttrValue(Attr.HidenAttrType.DamageReflectChance)
    if reflectChance <= 0 then
        return
    end    
    if not self.battle:IsRandLt(reflectChance) then
        return
    end
    local reflectVal = math.ceil(ret.value * (reflectPercent / 10000))
    if reflectVal <= 0 then
        return
    end

    local target = self.battle:GetRoleById(ret.senderId)
    if target ~= nil then
        target:RecvDamage({
            value = reflectVal,
            senderId = self.id,
            hitType = HitType.Damage,
            notReflect = true,
            notRefix = true,
            isReflect = true,
        })
    end
end

--修正自己受到的伤害
function Role:RefixDamage(ret)

    if ret.impact ~= nil and ret.impact:IsNormalAtk() then
        --普攻伤害减免
        local natkReduce = self:GetAttrValue(Attr.HidenAttrType.NormalAtkDamageReduce)
        if natkReduce ~= 0 then
            ret.value = math.ceil(ret.value * ((10000-natkReduce) / 10000))
        end
    end

    self.buffContainer:CallImpactFunc("RefixDamage",ret)
    self.passiveBuffContainer:CallImpactFunc("RefixDamage",ret)
    --其他人修正
    self.battle:BroadCastMsg('bcRefixDamage',self,ret)
end

--修正自己产生的伤害
function Role:RefixSendDamage(ret)
    self.buffContainer:CallImpactFunc("RefixSendDamage",ret)
    self.passiveBuffContainer:CallImpactFunc("RefixSendDamage",ret)
    --其他人修正
    self.battle:BroadCastMsg('bcRefixSendDamage',self,ret)
end

--AP增加
function Role:RecvApInc(ret)
    if not self:IsAlive() then
        return
    end

    if ret.value < 0 then ret.value = 0 end

    self:SetAP(self.ap + ret.value)
    
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            senderID = ret.senderId,
            targetID = self.id,
            val = ret.value,
            hitType = HitType.IncAp,
        }
    })

    self.battle:RoleApChange(self)

    self:SendMessage('msgRecvApInc',ret)
end

--AP减少
function Role:RecvApDesc(ret)
    if not self:IsAlive() then
        return
    end

    if ret.value < 0 then ret.value = 0 end

    self:SetAP(self.ap - ret.value)

    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            senderID = ret.senderId,
            targetID = self.id,
            val = ret.value,
            hitType = HitType.DescAp,
        }
    })

    self.battle:RoleApChange(self)

    self:SendMessage('msgRecvApDesc',ret)
end

function Role:Die(killerId)
    self.deading = true
    self:SendMessage('msgBeforeDead',killerId)
    self.battle:BroadCastMsg('bcBeforeDead',self,killerId)
    --可能被修正掉
    if self.hp > 0 then
        self.deading = false
       return 
    end
    --确实死亡了
    self:Dead(killerId)
end

function Role:Dead(killerId)
    if self.dead then
        return
    end
    self.hp = 0
    --掉落
    if self.drop ~= nil then
        --有需要展示的掉落
        if self.drop.drops ~= nil and #self.drop.drops > 0 then
            self.battle:AddEvent({
                type = BattleEventType.DropItem,
                dropItemEvent = {
                    roleId = self.id,
                    dropList = self.drop,
                },
            })
        end
        self.battle:CollectDrop(self.drop)
        self.drop = nil
    end
    
    --广播给buff等
    self:SendMessage('msgDead',killerId)
    --通知battle
    self.battle:BroadCastMsg("bcRoleDead",self.id,killerId)
    --通知击杀者
    local killer = self.battle:GetRoleById(killerId)
    if killer ~= nil and killer.isValid then
        killer:OnKillOther(self)
    end

    self.dead = true

    self:TryBubbleDirect(BubbleType.Die)

    --产生事件
    self.battle:AddEvent({
        type = BattleEventType.Kill,
        killRoleEvent = {
            killerID = killerId,
            roleID = self.id,
        }
    })

    --走完了完整的死亡，肯定要删除了
    self.isValid = false

    --击杀了boss
    if self.roleBaseTab ~= nil and self.roleBaseTab.Type ~= 0 then
        self.battle:OnSpecRoleKilled(self,self.roleBaseTab.Type)
    end
    --Boss分部主体死亡
    if self:IsAvatarPartsMain() then
        self.battle:OnAvatarPartsMainDead(self, killerId)
    end

    self.battle:OnRoleDead(self,killerId)
    self:OnDel()
end

function Role:OnAvatarPartsMainDead(role,killerId)
	if role == nil then
		return
	end
	if not self:IsAlive() then
		return
	end
	if role.id == self.id then
		return
    end
    --是否是同组Boss分部
    if self.roleBaseTab.AvatarBinding ~= role.roleBaseTab.AvatarBinding then
		return
    end
    if self.AvatarBindingTab == nil then
        return
    end
    --自己是分部吗
    if self.roleBaseTab.Id ~= self.AvatarBindingTab.PartsId[1] and self.roleBaseTab.Id ~= self.AvatarBindingTab.PartsId[2] then
        return
    end
    --强制死亡
    common.debuglog("OnAvatarPartsMainDead self dead. AvatarBinding ",self.roleBaseTab.AvatarBinding)
    self.hp = 0
    self:Dead(killerId)
end

function Role:OnKillOther(role)
    self:SendMessage('msgKill',role)
end

function Role:RecvHeal(ret)
    --如果确实死亡了，除非是复活治疗，否则不接受治疗
    if self.dead and not ret.isResurrect then
        return
    end

    if not self:CanBeHeal() then
        return
    end

    if ret.value < 0 then ret.value = 0 end

    ret.validValue = common.clamp(ret.value,0,self:GetMaxHP() - self.hp)

    self:SetHP(self.hp + ret.value)

    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            senderID = ret.senderId,
            targetID = self.id,
            val = ret.value,
            hitType = HitType.Heal
        }
    })

    self.battle:StatistHeal(ret)
end

function Role:GetSkillIdFitEnv(skillId)
    local tab = tabMgr.GetByID('SkillEx',skillId)
    if tab ~= nil then
        local curEnv = self.battle.curEnvType

        --是否中了诅咒(始终用较差的效果)
        if self:IsCursed() then
            if tab.BetterEnv >= 0 and tab.BetterEnv == curEnv  then
                return tab.OtherID
            end
        end

        if self:IsAlwaysDay() then
            if tab.EnvLimit ~= 0 then
                return tab.OtherID
            else
                return skillId
            end
        elseif self:IsAlwyasNight() then
            if tab.EnvLimit ~= 1 then
                return tab.OtherID
            else
                return skillId
            end
        end

        --环境是否合适
        if tab.EnvLimit ~= -1 and curEnv ~= tab.EnvLimit then
            return tab.OtherID
        end
    end
    return skillId
end


function Role:SendMessage(msg,...)
    if not self.isValid then
        return
    end

    if not self:IsAlive() and not MsgIgnoreDead[msg] then
        return
    end

    self.buffContainer:SendMessage(msg,...)
    self.passiveBuffContainer:SendMessage(msg,...)

    if not self.isValid then
        return
    end

    if self.aiExtend ~= nil then
        self.aiExtend:SendMessage(msg,...)
    end
    if self.spGainer ~= nil then
        self.spGainer:SendMessage(msg,...)
    end
end

function Role:ProcessTaunt()
    local tauntBuff = self.buffContainer:GetBuffByImpactSubClass(ImpactSubClass.Taunt)
    if tauntBuff == nil then
        warn("not in taunt")
        return
    end
    
    if tauntBuff.sender == nil then
        warn("taunt sender not found")        
        return
    end

    --不能攻击自己
    if self == tauntBuff.sender then
        return
    end

    --是否合法
    if SkillProcess.TargetFilter(tauntBuff.sender) then
        return
    end

    --强制使用普攻
    self:ActUseSkillByIndex(1,tauntBuff.sender.id)
end

function Role:ProcessChaos()
    
    if self.skills[1] == nil then
        return
    end


    local roles = self.battle:GetRoleAlies(self)

    if roles == nil then
        return
    end

     --忽略掉不能选的角色
    common.removec(roles,SkillProcess.TargetFilter)
    --不能是自己
    common.removec(roles,function(r) return r == self end)
    --随机一个目标
    local target = self.battle:RandomSelectOne(roles)
    --不能攻击自己
    if target == self then
        return
    end
    if target == nil then
        return
    end
    --对目标使用普攻
    self:ActUseSkillByIndex(1,target.id)
end

function Role:OnImpactFadeIn(impact)
    if impact == nil then
        return
    end

    self:SendMessage("OnRoleImpactFadeIn",impact)
end

function Role:OnImpactFadeOut(impact,autoFadeOut)
    if impact == nil then
        return
    end

    self:SendMessage("OnRoleImpactFadeOut",impact,autoFadeOut)
    self.battle:BroadCastMsg("bcRoleImpactFadeOut",impact,autoFadeOut)
end

function Role:OnSendImpactEffected(impact)
    if impact == nil then
        return
    end
    self:SendMessage("msgOnSendImpactEffected",impact)
end

function Role:IsImmue(impactTab)
    --检查rolebase自身是否有抗性
    if ImpactUtils.IsSubClass(impactTab,self.roleBaseTab.ImpactSubClassResist) then
        return true
    end

    return self.buffContainer:IsImmue(impactTab) or self.passiveBuffContainer:IsImmue(impactTab)
end

function Role:IsImmueHit(hitTab)
    return self.buffContainer:IsImmueHit(hitTab) or self.passiveBuffContainer:IsImmueHit(hitTab)
end

function Role:GetBuffsByCond( func )
    if self.buffContainer == nil then
        return nil 
    end
    return self.buffContainer:GetBuffsByCond(func)
end

function Role:GetBuffsByImpactClass(impactClass)
    local buffContainer = self.buffContainer
    if buffContainer == nil then
        return nil
    end
    return buffContainer:GetBuffsByImpactClass(impactClass)
end

function Role:GetBuffsByImpactId(id)
    local buffContainer = self.buffContainer
    if buffContainer == nil then
        return nil
    end
    return buffContainer:GetBuffsByImpactId(id)
end

function Role:GetBuffsByImpactSubClass(subClass)
    local buffContainer = self.buffContainer
    if buffContainer == nil then
        return nil
    end
    return buffContainer:GetBuffsByImpactSubClass(subClass)
end

function Role:NotifyImmue()
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.Immue
        }
    })
end

function Role:NotifyResist()
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.Resisit
        }
    })
end

function Role:NotifyAbsorb(val)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.Absorb,
            val = val,
        }
    })
end

function Role:NotifyDispell(impactClass,count)

    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.Dispell,
            val = impactClass,
        }
    })
end

function Role:NotifyHit(hitType,val)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = hitType,
            val = val,
        }
    })
end

function Role:OnDispellBuffs(impactClass,count)
    
    --驱散了特定buff后
    self:SendMessage("msgDispellBuffs",impactClass,count)

end

--通用弹字典冒字
function Role:NotifyHitText(strId)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.Tips,
            val = strId,
        }
    })
end

--被动技能生效
function Role:NotifyPassiveEffected(skillExId)
    self.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = self.id,
            hitType = HitType.PassiveEffected,
            val = skillExId,
        }
    })
end

function Role:NotifyCD()
    self.battle:AddEvent({
        type = BattleEventType.SkillCooldownChangeEvent,
        skillCooldownEvent = {
            roleId = self.id,
            skillCooldownInfos = self.cdCounter:Pack()
        }
    })
end

--是否虚无，不能被选中
function Role:IsNothingness()
    return self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Nothingness)
end

--诅咒，放的技能始终是较差的版本
function Role:IsCursed()
    return self.buffContainer:AnyBuffIsSubClass(ImpactSubClass.Cursed)
end

function Role:IsAlwaysEnvEnhance()
    return self.buffContainer:AnyBuffIsTag(ImpactTags.AlwaysEnvEnhance)
end

function Role:IsAlwaysDay()
    return self.buffContainer:AnyBuffIsTag(ImpactTags.AlwaysDay)
end

function Role:IsAlwyasNight()
    return self.buffContainer:AnyBuffIsTag(ImpactTags.AlwaysNight)
end

function Role:CanRelive()
    return not self.buffContainer:AnyBuffIsTag(ImpactTags.NeverRelive)
end

function Role:IsPassiveDisable()
    if self.passiveDisable and self.passiveDisable > 0 then
        return true
    end
    return false
end

function Role:IncPassiveDisableCount()
    if self.passiveDisable == nil then
        self.passiveDisable = 0
    end
    
    self.passiveDisable = math.max(self.passiveDisable,0)
    local last = self.passiveDisable
    self.passiveDisable = last + 1
    self:SetAttrsDirty()
    -- if last <= 0 and self.passiveDisable > 0 then
    --     self.passiveBuffContainer:TmpRemoveAll()
    -- end
end

function Role:DescPassiveDisableCount()
    if self.passiveDisable == nil then
        return
    end
    
    local last = self.passiveDisable
    self.passiveDisable = self.passiveDisable - 1
    self.passiveDisable = math.max(self.passiveDisable,0)
    self:SetAttrsDirty()
    -- if last > 0 and self.passiveDisable <= 0 then
    --     self.passiveBuffContainer:TmpAddAll()
    -- end
end

function Role:HasImpact(impactId,sender)
    --只考虑正常buff
    return self.buffContainer:HasImpact(impactId,sender)
end

function Role:GetValidSkillTargets(skillId)
    local tabEx = tabMgr.GetByID('SkillEx',skillId)
    if tabEx == nil then
        return false
    end
    local tabBase = tabMgr.GetByID('SkillBase',tabEx.BaseID)
    if tabBase == nil then
        return false
    end
    local roles = nil
    local battle = self.battle
    local targetType = tabBase.TargetType
    if targetType == SkillTargetType.Enemy then
        roles = battle:GetRoleEnemies(self)
    elseif targetType == SkillTargetType.Our then
        roles = battle:GetRoleAlies(self)
    elseif targetType == SkillTargetType.Self then
        roles = {self}
    else
        return nil
    end
	
	--读取脚本化技能配置 
	if tabEx.__script_type == nil and tabEx.ScriptID ~= -1 then
        tabEx = SS.Parse(tabEx,nil)
    end
	
    --忽略掉不能选的角色
    common.removecEx(roles,SkillProcess.TargetFilterEx,self,tabEx)
    return roles
end

function Role:GetValidSkillTargetsByIndex(index)
    if index < 1 or index > #self.skills then
        return nil
    end
    return self:GetValidSkillTargets(self.skills[index])
end

function Role:RandomSelectSkillTarget(skillId)
    local roles = self:GetValidSkillTargets(skillId)
    if roles == nil or #roles == 0 then
        return nil
    end
    return self.battle:RandomSelectOne(roles)
end

function Role:RandomSelectSkillTargetByIndex(index)
    local roles = self:GetValidSkillTargetsByIndex(index)
    if roles == nil or #roles == 0 then
        return nil
    end
    return self.battle:RandomSelectOne(roles)
end

function Role:MoveToFirst(ignoreHero)
    local battle = self.battle
    if battle == nil or battle.actQ == nil then
        return
    end
    battle.actQ:Remove(self) --从普通队列移除
    battle.actQ:InsertPriority(self,ignoreHero) --插入到优先队列
    battle:RoleApChange(self)
end

function Role:Summon(summonId,isPlaySpawnAnim,battlePos,battlePosArea,spawnRule)
    if self.battle == nil then
        return nil
    end

    local battle = self.battle

    local initData = self:LoadSummonFromTab(summonId,battlePos,battlePosArea)
    if initData == nil then
        return nil
    end
    initData.spawnRule = spawnRule
    local role = battle:CreateRole(initData,isPlaySpawnAnim)
    if role ~= nil then
        role.summonOnwerId = self.id
        self.summonRoles = self.summonRoles or {}
        self.summonRoles[role.id] = role
    end
    return role
end

--驱除自己所有的召唤物
function Role:ClearAllSummon()
    if self.battle ~= nil and self.summonRoles ~= nil then
        for k,v in pairs(self.summonRoles) do
            self.battle:DelRole(v)
        end
    end
end

function Role:GetSummonRoleCount()
    if self.summonRoles == nil then
        return 0
    end
    local count = 0
    for _,_ in pairs(self.summonRoles) do
        count = count + 1
    end
    return count
end

function Role:TestSummon()
    self:Summon(1,true,0,BattleAreaType.BlueSummon)
end

function Role:LoadSummonFromTab(summonId,battlePos,battlePosArea)

    if self.battle == nil then
        return nil
    end

    local monsterTab = tabMgr.GetByID("SummonMonster",summonId)
    if monsterTab == nil then
        return nil
    end
    
    local roleInfo = self.battle:LoadRoleBase(monsterTab.RoleBaseId)
    if roleInfo == nil then
        return nil
    end

    --根据角色属性，和表格，计算召唤物属性
    local attrsMsg = { attrs = {}}
    local attrs = attrsMsg.attrs
    --需要从表里初始化的属性
    local initAttrs = {
        'MaxHP',
        'Attack',
        'Defense',
        'Speed',
        'CritChance',
        'CritEffect',
        'ImpactChance',
        'ImpactResist',
    }

    for _,attrName in ipairs(initAttrs) do
        local attrIndex = AttrType[attrName]
        local roleVal = self:GetAttrValue(attrIndex)
        local tabRefixAdd = monsterTab[attrName]
        local tabRefixPercent = monsterTab[attrName .. 'Rate']
        local val = math.ceil( roleVal * (tabRefixPercent / 10000) )+ tabRefixAdd
        table.insert( attrs,{type = attrIndex,value = val} )
    end
    roleInfo.attrs = attrsMsg

    local initData = {}
    initData.roleInfo = roleInfo
    initData.battlePos = battlePos
    initData.side = self.side
    if monsterTab.IsPlayerControl == 1 then
        initData.userObjId = self.userObjId
    end
    --initData.ai = monsterTab.AI
    if battlePosArea == nil then
        if side == BattleSide.bs_Red then
            initData.battlePosArea = BattleAreaType.Red
        elseif side == BattleSide.bs_Blue then
            initData.battlePosArea = BattleAreaType.Blue
        else
            initData.battlePosArea = BattleAreaType.Red
        end
    else
        initData.battlePosArea = battlePosArea
    end

    return initData
end

function Role:ReplaceSkills(skill1,skill2,skill3,notify)
    if self.orgSkills == nil then
        --拷贝一份，只拷贝一次
        self.orgSkills = {}
        for _,skillId in ipairs(self.skills) do
            table.insert(self.orgSkills,skillId)
        end
    end
    local replacedSkills = {skill1,skill2,skill3}
    for i,r in ipairs(replacedSkills) do
        if r ~= -1 then
            self.skills[i] = r
        end
    end
    if notify then
        self.battle:AddEvent({
            type = BattleEventType.ChangeSkills,
            changeSkillsEvent = {
                roleId = self.id,
                skillIds = self.skills
            }
        }) 
    end
end

--根据skillBass替换技能，会按照技能等级替换skillEx
function Role:ReplaceSkillsByBase(skill1,skill2,skill3,notify)
    local s1 = SkillProcess.Utils.GetSkillByBase(skill1,self:GetSkillLvByIndex(1))
    local s2 = SkillProcess.Utils.GetSkillByBase(skill2,self:GetSkillLvByIndex(2))
    local s3 = SkillProcess.Utils.GetSkillByBase(skill3,self:GetSkillLvByIndex(3))

    self:ReplaceSkills(s1,s2,s3,notify)
end

function Role:ResumeSkills()
    if self.orgSkills ~= nil then
        --拷贝回去
        for i,k in ipairs(self.orgSkills) do
            self.skills[i] = k
        end

        self.battle:AddEvent({
            type = BattleEventType.ChangeSkills,
            changeSkillsEvent = {
                roleId = self.id,
                skillIds = self.skills
            }
        })
    end
end

function Role:TryBubbleDirect(bubbleType)
    local item = self:TryBubble(bubbleType)
    if item == nil then
        return
    end

    self.battle:Bubble(item)
end

--概率触发喊话
function Role:TryBubble(bubbleType)

    if not self.battle.enableBubble then
        return nil
    end

    --必须有卡牌id
    if self.cardId == nil or self.cardId == -1 then
        return nil
    end

    local chance = 0
    if bubbleType == BubbleType.BattleStart 
        or bubbleType == BubbleType.Win
        or bubbleType == BubbleType.Lose
        or bubbleType == BubbleType.Talk
    then
        chance = 10000
    elseif bubbleType == BubbleType.UseSkill then
        local tab = tabMgr.GetByID("BattleBubbleCard",self.cardId)
        if tab ~= nil then
            chance = tab.UseSkillChance
        end
    elseif bubbleType == BubbleType.Die then
        local tab = tabMgr.GetByID("BattleBubbleCard",self.cardId)
        if tab ~= nil then
            chance = tab.DieChance
        end
    end

    if chance < 10000 then
        if not self.battle:IsRandLt(chance) then
            return nil
        end
    end

    local action = {
        roleId = self.id,
        type = bubbleType,
        userObjId = self.userObjId,
    }

    if bubbleType == BubbleType.Talk then
        --寻找情缘符灵，触发双方搭话
        local tab = tabMgr.GetByID("BattleBubbleCard",self.cardId)
        if tab ~= nil then
            local validRoles = self.battle:FindRoles(function(r)
                if not r:IsAlive() then
                    return false
                end
                if r.cardId == nil then
                    return false
                end
                if r == self then
                    return false
                end
                if r.side ~= self.side then
                    return false
                end
                for _,cardId in ipairs(tab.OtherCardId) do
                    if r.cardId == cardId then
                        return true
                    end
                end
                return false
            end)

            if validRoles ~= nil and #validRoles > 0 then
                local items = {}
                
                for _,role in ipairs(validRoles) do
                    for i,cardId in ipairs(tab.OtherCardId) do
                        if role.cardId == cardId then
                            talkChance = tab.ReChance[i]
                            table.insert(items,{talkChance,role})
                            break
                        end
                    end
                end

                local role = self.battle:RandomSelectOne_Chnace(items)
                if role ~= nil then
                    action.nextBubble = {
                        roleId = role.id,
                        type = BubbleType.Chat,
                        userObjId = role.userObjId,
    
                        nextBubble = {
                            roleId = self.id,
                            type = BubbleType.Response,
                            userObjId = self.userObjId,
                        }
                    }
                end
            end
        end
    end
    
    return action
end

-- 破冰
function Role:OnIceBreak(ret)
    local damage = math.ceil(self:GetMaxHP() * (CalcParams.IceBreakDamage / 10000))
    if damage <= 0 then
        return
    end

    self:RecvDamage({
        value = damage,
        senderId = ret.senderId,
        hitType = HitType.Damage,
        notReflect = true,
        notRefix = true,
        isReflect = false,
    })
end

return Role