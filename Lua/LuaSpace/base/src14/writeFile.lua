require "Tools" --��utils��

local file,err=io.open("luaFileStreamTest.txt","wb")   --wb:��ģʽΪ�����ƣ����ǣ�д
if(not err) then
file:write("Lua fileStream Test����д\n")
file:close()
end

local file,err=io.open("luaFileStreamTest.txt","ab")   --wb:��ģʽΪ�����ƣ�׷�ӣ�д
if(not err) then
file:write("Lua fileStream Test2׷��д\n")
file:close()
end