function xFoo(...)
local args={...}--���в���������args���������
for i,v in ipairs(args) do
print(i,v)
end
end


xFoo(1,"HelloLua")
xFoo(1,2,3,4,5)