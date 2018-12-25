
print("lua said:",a,b,c,d,e);--打印写入的全局变量

for i,v in ipairs(t) do     --数组类型表
print("lua said:",i,v)
end

for k,v in pairs(t2) do    --遍历k v类型表
print("lua said:",k,v)
end