require("class")

local AI = class("AIBase")

function AI:Init(role)
    self.role = role
    self.battle = role.battle
    self:OnInit()
end

function AI:OnInit()
end

function AI:SendMessage(msg,...)
    local func = self[msg]
    if func == nil then
        return
    end
    func(self,...)
end

function AI:ProcessAI()
    --走常规AI流程
    self.role:ProcessNormalAI()
end

function AI:SelectSkillTarget(skillId)
    return nil
end

return AI