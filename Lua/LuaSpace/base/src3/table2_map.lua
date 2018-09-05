a={x=1,y=2,z=3}

for k,v in pairs(a) do--kv型table要用kv去遍历
print(k,v)
end

for k,v in pairs(a) do
print(k,a[k])
end

print(a.x)  --通过key取value

print(a["x"]) --通过下标取value

