Tools={}  --����һ���������ԶԴ�������ܽ����

function Tools.add(a,b)   --����utils���ĺ���
return a+b;
end

function Tools.PrintTable(a)
for k,v in pairs(a) do
print("key=",k,"value=",v)
end
end
