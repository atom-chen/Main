a={5,4,3,2,1}

for i,v in ipairs(a) do --数组迭代器
print(i,v)
end

for i,v in ipairs(a) do
print(i,a[i])
end

for k,v in pairs(a) do  --pair迭代器
print(k,v)
end

for k,v in pairs(a) do  
print(k,a[k])
end

print("table size=",table.getn(a)); --获取数组类型table长度