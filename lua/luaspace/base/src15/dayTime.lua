require "Tools" --��utils��

print(os.time());  --��ĳһʱ�����������

print(os.time({year=2018,month=5,day=23}));  --����ʱ��Ϊֹ������

print(os.time({year=2018,month=5,day=23,hour=11})); 

print(os.clock()) --���ص�����ʼ�������������ĵ�ʱ��

--time���ص��ַ����ǿ���������Ҫ��date����и�ʽ��
---------------------------------------date Begin------------------------------------------
local t=os.date("*t",os.time()) --�õ���ǰʱ���pair
Tools.PrintTable(t)

t=os.date("%a",os.time()) --�õ���ǰ���ڵļ�д
print("\n",t)

t=os.date("%A",os.time()) --�õ���ǰ���ڵ�ȫ��
print("\n",t)

t=os.date("%Y-%m-%d",os.time()) --��ǵ����
print("\n",t)