#include<iostream>
#include <string>
using namespace std;
/*
long long *array;
int jiecheng(int n)
{
	
	if (n > 1e6)
	{
		return 0;
	}
	//��ʼ��
	array = new long long[n+1];
	array[0] = 1;
	array[1] = 1;
	for (int i = 2; i < n; i++)
	{
		array[i] = 0;
	}
	//�����
	long long total = 0;
	//�Ե�����
	for (int i = 2; i <= n; i++)
	{
		array[i] = i*array[i - 1];
	}
	for (int i = 0; i <= n; i++)
	{
		cout <<i<<"!="<<array[i]<<endl;
		total += array[i];\
	}
	return total;
}

int jiecheng2(int n)
{
	//���ֵ����
	if (array[n] != 0)
	{
		return array[n];
	}
	//��������� �������
	else{
		array[n] = n*jiecheng2(n-1);
	}
	return array[n];

}

void main()
{
	int n = 0;
	cin >> n;
	cout<<n<<"�Ľ׳�֮��Ϊ"<<jiecheng(n)<<endl;
}
*/
