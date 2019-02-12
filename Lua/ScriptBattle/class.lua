--面向对象


local new = function(clazz)
    local t = {}
    setmetatable(t,clazz)
    if clazz.ctor ~= nil then
        clazz.ctor(t)
    end
    return t
end

local class = function(className,baseClass)
    local clazz = {}
    if baseClass ~= nil then
        setmetatable(clazz,{__index = baseClass})
        clazz.__base = baseClass
    end
    clazz.__name = className
    clazz.__index = clazz
    clazz.__tostring = function(t)
        setmetatable(t,nil)
        local str = string.format( "class:%s , %s",className,tostring(t))
        setmetatable(t,clazz)
        return str
    end
    return clazz
end

local is = function(obj,clazz)
    local objClazz = getmetatable(obj)

    if objClazz == clazz then
        return true
    end

    if objClazz == nil then
        return false
    end

    --向上检测
    local t = objClazz
    repeat
        t = t.__base
        if t == clazz then
            return true
        end
    until t == nil
    
    return false
end

_G.class = class
_G.new = new
_G.is = is


--测试

-- local C1 = class("C1")
-- function C1:Test()
--     print("c1",self)
-- end
-- function C1:TestBase()
--     print("only base have")
-- end

-- local C2 = class("C2",C1)
-- function C2:Test()
--     C2.__base.Test(self)
--     print("c2",self)
-- end

-- local C3 = class("C3",C1)
-- function C3:Test()
--     print("c3",self)
-- end
-- function C3:ctor()
--     print("call c3 ctor")
--     print('~~~~~~~~~~')
-- end

-- local obj1 = new(C1)
-- obj1:Test()
-- print('-----------------------')
-- local obj2 = new(C2)
-- obj2:Test()
-- print('-----------------------')
-- local obj3 = new(C3)
-- obj3:Test()
-- print('-----------------------')

-- obj2:TestBase()
-- obj3:TestBase()

-- print("obj1 is C1",is(obj1,C1))
-- print("obj2 is C3",is(obj2,C3))

-- local C4 = class("C4",C2)
-- local obj4 = new(C4)
-- print("obj4 is C4",is(obj4,C4))
-- print("obj4 is C2",is(obj4,C2))
-- print("obj4 is C1",is(obj4,C1))
-- print("obj4 is C3",is(obj4,C3))

-- print(obj4)
-- print("???????????????")
-- obj4:Test()
-- print("??????????????")