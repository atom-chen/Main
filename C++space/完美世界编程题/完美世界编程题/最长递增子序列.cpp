#include<iostream>
#include <string>
using namespace std;
//给定一个长度为N的数组，找出一个最长的单调自增子序列（不一定连续，但是顺序不能乱）
int L(int *array, int size)
{
	//备忘录，记录第i个元素为起始点的最优解
	int *remember = new int[size];
	remember[0] = 0;

	int sub=L(array, size, 0, remember);
	delete[] remember;
	return sub;
}
int& max(int &a, int &b);
int& L(int *array, int size, int i,int *remember)
{
	//如果备忘录中有
	if (remember[i] >= 0)
	{
		return remember[i];
	}
	else{
		//没有，则去算
		//当前串一定以某个数字开头
		//i开头的最优解=1+array[i+1,...,size-1]的最优解
		for (int j = i; j < size; j++)
		{
			
		}
	}
}
int& max(int &a, int &b)
{
	if (a >= b)
		return a;
	else
		return b;
}
