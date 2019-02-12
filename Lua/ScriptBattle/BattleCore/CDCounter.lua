--冷却计时

require("class")


local CDCounter = class("CDCounter")

function CDCounter:ctor()
    self.timers = {}
end

function CDCounter:Begin(id,time)
    if time <= 0 then
        return
    end
    self.timers[id] = time + 1
end

function CDCounter:GetCooldown(id)
    return self.timers[id] or 0
end

function CDCounter:SetCooldown(id,time)
    if self.timers[id] == nil then
        return
    end
    self.timers[id] = time
end

function CDCounter:ClearCooldown(id)
    if self.timers[id] == nil then
        return false
    end
    self.timers[id] = nil
    return true
end

function CDCounter:Clear()
    if self.timers == nil then
        return false
    end

    self.timers = {}
    return true
end

function CDCounter:Cooldown()

    if self.timers == nil then
        return false
    end

    local dirty = false
    for k,v in pairs(self.timers) do
        local newVal = v - 1
        if newVal <= 0 then
            self.timers[k] = nil
        else
            self.timers[k] = newVal
        end
        dirty = true
    end
    return dirty
end

function CDCounter:Pack()
    local l = {}
    for k,v in pairs(self.timers) do
        table.insert( l, {
            cooldownId = k,
            cooldownLeft = v,
        } )
    end
    return l
end

function CDCounter:IncCooldown(id,time)
    local curr = self:GetCooldown(id)
    
    if curr > 0 then
        curr = curr + time
        if curr <= 0 then
            self:ClearCooldown(id)
        else
            self:SetCooldown(id,curr)
        end
    else
        if time < 0 then
            return
        else
            self:Begin(id,time)
        end
    end
end

function CDCounter:IncAllCooldown(time)
    local ids = {}
    for id,_ in pairs(self.timers) do
        table.insert(ids,id)
    end
    for _,id in ipairs(ids) do
        self:IncCooldown(id,time)
    end
end

return CDCounter