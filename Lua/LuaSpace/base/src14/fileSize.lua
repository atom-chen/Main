require "Tools" --��utils��

local file,err=io.open("luaFileStreamTest.txt","rb")   --wb:��ģʽΪ�����ƣ����ǣ�д
if(not err) then
local fileSize=file:seek("end") --���ļ�ָ���Ƶ��ļ���ĩβ�����ҷ��ش�ʱ�ļ�ָ���λ��
print(fileSize)

file:seek("set",0) --���ļ�ָ���Ƶ�0��λ�ã�ͷ��)
local buffer=file:read("*a")
print(buffer)
file:close()
end

