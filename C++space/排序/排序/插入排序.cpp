#include<iostream>
using namespace std;
template<class T>
void insertSort(T array[], int size)
{
	//�����һ������
	int count = 1;
	for (int i = 0; i < size;i++)
	{
		int j = i;
		int k = j - 1;//kָ��Ҫ����Ԫ�ص�ǰһ��Ԫ��
		//�����ǰһ����С ��һֱ��ǰ�ƶ�
		while (array[j] < array[k] && k>=0)
		{
			mySwap(array[k], array[j]);
			j--;
			k--;
		}
		count++;
	}
}


