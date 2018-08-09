--优先攻击特定目标
--自动设置优先攻击目标
--参数：
--使用RoleBase的Type
--使用RoleBaseId


local AI = require("BattleCore/AIExtend/AIBase")
local HitType = require('BattleCore/Common/HitType')
local tabMgr = require('TabManager')
local BattleEventType = require('BattleCore/Common/BattleEventType')
local RoleType = require('BattleCore/Common/RoleType')

require("class")

local AI_1 = class("AI_1",AI)

function AI_1:OnInit()
    if self.role == nil then
        return
    end

    if self.role.aiTab == nil then
        return
    end

    self.roleType = self.role.aiTab.Param[1]
    self.roleBaseId = self.role.aiTab.Param[2]
end

function AI_1:SelectSkillTarget(skillId)

    local roles = self.role:GetValidSkillTargets(skillId)
    if roles == nil or #roles == 0 then
        return nil
    end

    --根据规则查找
    local target = nil
    if self.roleType ~= -1 then
        for _,role in ipairs(roles) do
            if role.roleBaseTab ~= nil and role.roleBaseTab.Type == self.roleType then
                target = role
                break
            end
        end
    elseif self.roleBaseId ~= -1 then
        for _,role in ipairs(roles) do
            if role.roleBaseID == self.roleBaseId then
                target = role
                break
            end
        end
    end

    return target

end

return AI_1