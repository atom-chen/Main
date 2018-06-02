

function LuaFunc()--被C调用的函数
print("hello,i am lua")
end

function LuaFunc2(a,b,c)
print(a,b,c)
end

function LuaFunc3()
return 1,1.5
end

function Error()
x=x.y+10
end

function LuaFunc4()
Error();
return 1,1.5
end

local function LuaFunc5()
print("luafunct")
end

--Test(LuaFunc5)