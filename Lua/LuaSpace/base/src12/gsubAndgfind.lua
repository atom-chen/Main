
a={}

a.x=100--插值kv方式1
a["y"]=200--插入kv方式2
print(a.x,a.y)

a={1,2,3}
table.insert(a,1,4) --往表中插入数据，插入val到pos的位置
table.insert(a,6) --往表中插入数据，插入val到表的末尾
print(a[1])
print(a[5])


