a={x=1,y=2,z=3}

for k,v in pairs(a) do--kv��tableҪ��kvȥ����
print(k,v)
end

for k,v in pairs(a) do
print(k,a[k])
end

print(a.x)  --ͨ��keyȡvalue

print(a["x"]) --ͨ���±�ȡvalue

