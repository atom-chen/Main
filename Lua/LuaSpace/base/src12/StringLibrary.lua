a="A123456789a"
print(string.len(a)) --字符串长度

print(string.lower(a))  --转换为小写返回

print(string.upper(a))  --转换为大写返回

print(string.rep(a,2))--repeat:将字符串重复n此

print(string.byte(a,2)) --取出第n个字符的ANSI值

print(string.char(97)) --取出ANSI码表第97号字符

print(string.sub(a,2,7)) --(2-7)的子字符串

print(string.find(a,"123",1)) --从父串的第n个数开始找子串，返回找到的起始位置和终止位置

print(string.find(a,"[0-9]+",1)) --通过正则表达式找         [0-9]+表示找数字，并且可能有由多个数字组成的字符串