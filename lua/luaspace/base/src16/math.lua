math.randomseed(os.time());--�������������
for i=0,20,1 do
local num=math.random()  --Ĭ�ϻ������[0,1]�������
if(num>=0.9) then
print(num)
end
end 


print(math.random(100))  --[1,100]

print(math.random(5,20)) --[5,20]

--���Ǻ����Ի�����Ϊ����


print(math.floor(1.1)) --����ȡ��
print(math.ceil(1.5)) --����ȡ��

print(math.max(1,2)) --ȡ��
print(math.min(1,2))  --ȡС
