#include<iostream>
#include<string>
#include<vector>
using namespace std;

template<class T>
void MereSort(T *array, int lo, int hi)
{
	if (lo >= hi)
	{
		return;
	}
	unsigned mid = lo + (hi - lo) / 2;
	//先分
	MereSort<T>(array, lo, mid);
	MereSort<T>(array, mid + 1, hi);
	//再合
	Mere<T>(array, lo, hi);

}
template<class T>
//将局部有序的数组合并
void Mere(T *array, const int& lo, const int& hi)
{
	T *temp = new T[hi - lo + 1];
	//拷贝
	for (int i = 0; i < hi - lo + 1; i++)
	{
		temp[i] = array[lo + i];
	}
	//往array的[lo,hi]定义域放数据
	int midIndex = (hi - lo) / 2;
	int endIndex = hi - lo;
	int b = 0;                         //左下标
	int m = midIndex + 1;              //右下标
	for (int i = lo; i < hi + 1; i++)
	{
		if (b > midIndex)
		{
			//放右边的
			array[i] = temp[m++];
		}
		else if (m > endIndex)
		{
			//放左边的
			array[i] = temp[b++];
		}
		else if (temp[b] <= temp[m])
		{
			array[i] = temp[b++];
		}
		else{
			array[i] = temp[m++];
		}
	}
	delete[] temp;


}