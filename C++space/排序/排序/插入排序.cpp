#include<iostream>
using namespace std;
template<class T>
void insertSort(T array[], int size)
{
	//假设第一个有序
	int count = 1;
	for (int i = 0; i < size;i++)
	{
		int j = i;
		int k = j - 1;//k指向要排序元素的前一个元素
		//如果比前一个数小 则一直往前移动
		while (array[j] < array[k] && k>=0)
		{
			mySwap(array[k], array[j]);
			j--;
			k--;
		}
		count++;
	}
}


