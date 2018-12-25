function xFoo(...)
local args={...}--所有参数都到了args这个表里面
for i,v in ipairs(args) do
print(i,v)
end
end


xFoo(1,"HelloLua")
xFoo(1,2,3,4,5)