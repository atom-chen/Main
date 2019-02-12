--优先攻击特定目标
--自动设置优先攻击目标
--参数：
--1，第几次行动
--2,技能id
--3,第几次行动
--4,技能id
--5,第几回合
--6,技能id
--....

local AI = require("BattleCore/AIExtend/AIBase")
local HitType = require('BattleCore/Common/HitType')
local tabMgr = require('TabManager')

require("class")

local AI_2 = class("AI_2",AI)

function AI_2:OnInit()
    if self.role == nil then
        return
    end

    if self.role.aiTab == nil then
        return
    end

    self.cmds = {}

    local params = self.role.aiTab.Param
    for i=0,3 do
        local t = {
            params[i*2 + 1],params[i*2 +2]
        }
        table.insert( self.cmds,t )
    end

    self.count = 0
end

function AI_2:msgRoundJustBegin()
    self.count = self.count + 1
end

function AI_2:ProcessAI()
    
    if self.role == nil or not self.role.isValid or not self.role:IsAlive() then
        return
    end

    --到了特定回合
    for _,cmd in ipairs(self.cmds) do
        if cmd[1] == self.count then
            if self.role:AI_TryCast(cmd[2]) then
                return
            end
            break
        end
    end

    --走常规AI流程
    self.role:ProcessNormalAI()
end

return AI_2