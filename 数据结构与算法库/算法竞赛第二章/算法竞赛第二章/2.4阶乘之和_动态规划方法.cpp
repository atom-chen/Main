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
	//初始化
	array = new long long[n+1];
	array[0] = 1;
	array[1] = 1;
	for (int i = 2; i < n; i++)
	{
		array[i] = 0;
	}
	//计算和
	long long total = 0;
	//自底向上
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
	//如果值存在
	if (array[n] != 0)
	{
		return array[n];
	}
	//如果不存在 就算出来
	else{
		array[n] = n*jiecheng2(n-1);
	}
	return array[n];

}

void main()
{
	int n = 0;
	cin >> n;
	cout<<n<<"的阶乘之和为"<<jiecheng(n)<<endl;
}
*/
