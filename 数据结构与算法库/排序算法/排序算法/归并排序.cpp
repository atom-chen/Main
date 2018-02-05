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
	//�ȷ�
	MereSort<T>(array, lo, mid);
	MereSort<T>(array, mid + 1, hi);
	//�ٺ�
	Mere<T>(array, lo, hi);

}
template<class T>
//���ֲ����������ϲ�
void Mere(T *array, const int& lo, const int& hi)
{
	T *temp = new T[hi - lo + 1];
	//����
	for (int i = 0; i < hi - lo + 1; i++)
	{
		temp[i] = array[lo + i];
	}
	//��array��[lo,hi]�����������
	int midIndex = (hi - lo) / 2;
	int endIndex = hi - lo;
	int b = 0;                         //���±�
	int m = midIndex + 1;              //���±�
	for (int i = lo; i < hi + 1; i++)
	{
		if (b > midIndex)
		{
			//���ұߵ�
			array[i] = temp[m++];
		}
		else if (m > endIndex)
		{
			//����ߵ�
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