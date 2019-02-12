--炸弹机制
--存活时间到时，触发子效果
--参数
--1,是否受减cd影响(吃到+cd的技能时，存活时间减少，加快引爆；吃到-cd的技能时，延迟引爆)
--2,爆炸特效
--3,伤害系数
--4,伤害固定值
--5,防御穿透(10000表示无视防御，9000表示无视90%的防御)
--6，子效果1,
--7，自效果1概率
--8，子效果2,
--9，自效果2概率

local Impact = require("BattleCore/Impact/Impact")
local common = require("common")
local warn = common.mk_warn('Impact')
local AttrType = require("BattleCore/Common/AttrType")
local CalcParams = require("BattleCore/CalcParams")
local SkillProcess = require('BattleCore/SkillProcess/SkillProcess')
local HitType = require('BattleCore/Common/HitType')
local BattleEventType = require('BattleCore/Common/BattleEventType')

require("class")

local Impact_37 = class("Impact_37",Impact)

function Impact_37:OnActive()
    Impact_37.__base.OnActive(self)
    if self.sender == nil then
        return
    end
    self.senderAtk = self.sender:GetAttrValue(AttrType.Attack)
    self.senderDmgEnhance = self.sender:GetAttrValue(AttrType.DmgEnhance)
    self.senderId = self.sender.id

    local param = self.tab.Param

    self.refixByCD = param[1] == 1
    self.blastEffect = param[2]
    self.dmgPower = param[3]
    self.dmgAdd = param[4]
    self.defenseIgnore = param[5]
    
end

function Impact_37:msgIncCooldown(val)
    if not self.refixByCD then
        return
    end
    self.liveTime = self.liveTime - val
    self.recver:NotifyBuffChange()
end

function Impact_37:msgDescCooldown(val)
    if not self.refixByCD then
        return
    end
    self.liveTime = self.liveTime + val
    self.recver:NotifyBuffChange()
end

--消散时生效
function Impact_37:OnImpactFadeOut(autoFadeOut)
    if autoFadeOut ~= true then
        return
    end

    self:Blast()
end

function Impact_37:Blast()

    local sender = self.sender
    local recver = self.recver

    if recver == nil then
        return
    end

    --炸弹爆炸
    recver.battle:AddEvent({
        type = BattleEventType.Hit,
        hitEvent = {
            targetID = recver.id,
            hitType = HitType.BombBlast,
            val = self.blastEffect,
        }
    })

    local ret = self:CalcDamage()
    if ret ~= nil then
        recver:RecvDamage(ret)
    end
 
    local tab = self.tab

    local enhance = 0
    local resisit = 0
    if sender ~= nil and sender:IsAlive() then
        enhance = sender:GetAttrValue(AttrType.ImpactChance)
    end
    if recver ~= nil and recver:IsAlive() then
        resisit = recver:GetAttrValue(AttrType.ImpactResist)
    end

    --额外子效果
    for i=0,1 do
        local impactId = tab.Param[6+i*2]
        if impactId ~= -1 then
            local chance = tab.Param[7+i*2]
        
            local ret = SkillProcess.CheckImpactChance(recver.battle
                        ,chance
                        ,enhance
                        ,resisit)

            local canSend = false
            if ret == 0 then
                canSend = true
            elseif ret == -1 then
                canSend = false
            elseif ret == -2 then
                canSend = false
                --提示抵抗
                recver:NotifyResist()
            end
                    
            if canSend then
                Impact.SendImpactToTarget(impactId,recver,self.sender)
            end
        end
    end
end

function Impact_37:CalcDamage()
    local tab = self.tab
    local recver = self.recver

    if recver == nil or not recver:IsAlive() then
        return nil
    end

    local power = self.dmgPower
    local ex = self.dmgAdd

    local p = (CalcParams.P2)
    --(攻击 * 300) / (对方防御 + 300）
    local baseAttack = math.ceil( (self.senderAtk * p) / ((recver:GetAttrValue(AttrType.Defense) * (10000 - self.defenseIgnore) )  + p) )

    local refix = self.senderDmgEnhance - recver:GetAttrValue(AttrType.DmgReduce) + 10000
    refix = math.max(CalcParams.P4,refix)

    --每步确保都要取整
    local step0 = math.ceil(baseAttack)
    local step1 = math.ceil(step0 * (refix / 10000))
    local step2 = math.ceil(step1 * (power / 10000))

    local final = step2 + ex

    final = math.max(0,final)

    local hitType = HitType.Damage

    local ret = {
        impact = self,
        value = final,
        isCrit = false,
        senderId = self.senderId,
        hitType = hitType,
    }

    return ret
end

return Impact_37