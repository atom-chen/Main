
local AI = require("BattleCore/AIExtend/AIBase")

local createAI = function(id,role)
    if id < 0 then
        return nil
    end

    local clazz = require("BattleCore/AIExtend/Logic/AI_" .. id)
    if clazz == nil then
        return nil
    end
    local ai = new(clazz)
    ai:Init(role)
    return ai
end

AI.CreateAI = createAI