require "Tools" --��utils��
a={}

a.x=100--��ֵkv��ʽ1
a["y"]=200--����kv��ʽ2
Tools.PrintTable(a)

print();
a={1,2,3}
table.insert(a,1,4) --�����в������ݣ�����val��pos��λ��
table.insert(a,6) --�����в������ݣ�����val�����ĩβ
Tools.PrintTable(a)

print();
table.remove(a,5)--�Ƴ�posλ���ϵ���ֵ
Tools.PrintTable(a)