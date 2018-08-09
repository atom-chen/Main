--光环buff，给场上的角色增加buff，新创建的角色也会收到buff
--光环buff移除后，子buff全部移除
--参数：
--1.子buff
--2.阵营（1我方，2敌方）

local Impact = require("BattleCore/Impact/Impact")

require("class")
local common = require("common")

local Impact_62 = class("Impact_62",Impact)

function Impact_62:OnActive()
    self.childBuffTb = {}
    self.childImpactId = self.tab.Param[1]
    self.sideType = self.tab.Param[2]
end

function Impact_62:OnImpactFadeIn()
    local roles = nil
    if self.sideType == 1 then
        roles = self.recver.battle:GetRoleAlies(self.recver)
    else
        roles = self.recver.battle:GetRoleEnemies(self.recver)
    end

    for _,role in ipairs(roles) do
       self:AddImpactTo(role) 
    end
    self:ImpactEffected()
end

function Impact_62:OnImpactFadeOut()
    self:ClearChilds()
end

function Impact_62:bcRoleCreate(role)
    if self.sideType == 1 then
        if role.side ~= self.recver.side then
            return
        end
    else
        if role.side == self.recver.side then
            return
        end
    end
    self:AddImpactTo(role)
    self:ImpactEffected()
end

function Impact_62:bcRoleDead(role)
    self.childBuffTb[role.id] = nil
end

function Impact_62:msgDead()
    self:ClearChilds()
end

function Impact_62:ClearChilds()
    for roleId,b in pairs(self.childBuffTb) do
        local role = self.recver.battle:GetRoleById(roleId)
        if role ~= nil and role:IsAlive() then
            role:RemoveBuff(b)
        end
    end
    self.childBuffTb = {}
end

function Impact_62:AddImpactTo(role)

    if not role:IsAlive() or role.isValid then
        return
    end

    --重复的替换
    if self.childBuffTb[role.id] ~= nil then
        role:RemoveBuff(self.childBuffTb[role.id],true)
        self.childBuffTb[role.id] = nil
    end

    self.childBuffTb[role.id] = Impac.SendImpactToTarget(
        self.childImpactId,role,self.recver
        , nil
        , self.skillInfo
    )
end

return Impact_62