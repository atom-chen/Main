require "Tools" --��utils��

local file,err=io.open("luaFileStreamTest.txt","rb")   --wb:��ģʽΪ�����ƣ����ǣ�д
if(not err) then
local buffer=file:read("*a") -- a=all���ļ�һ���Զ�����
print(buffer)
file:close()
end

local file,err=io.open("luaFileStreamTest.txt","rb")   --wb:��ģʽΪ�����ƣ����ǣ�д
if(not err) then
local buffer=file:read("*l") --l=line
print(buffer)
local buffer=file:read("*l") --l=line
print(buffer)
file:close()
end

