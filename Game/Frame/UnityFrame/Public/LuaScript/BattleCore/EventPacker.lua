--事件打包
--表现时需要知道，发生的事件，由哪个点触发的

require("class")
local common = require("common")

local EventPacker = class("EventPacker")

function EventPacker:ctor()
    self.groups = {}
    self.events = nil
    self.totalEvents = {}
end

function EventPacker:NewGroup()
    self.events = {}
    table.insert(self.groups,self.events)
end

function EventPacker:Pop()
    local count = #self.groups
    if count == 0 then return nil end
    table.remove(self.groups,count)
    local events = self.events
    self.events = self.groups[#self.groups]
    return events
end

function EventPacker:Peek()
    return self.events
end

function EventPacker:ClearTop()
    self.events = {}
end

function EventPacker:AddEvent(e)
    if self.events == nil then
        common.warn("events is nil,call NewGroup first")
        return
    end

    table.insert(self.events,e)

    --记录所有的事件
    if #self.groups == 1 then
        table.insert(self.totalEvents,e)
    end
end

return EventPacker