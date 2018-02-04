#include <iostream>
using namespace std;


template<class T>
T* mySort(T *array, int size)
{
	T *t=new T;
	return t;
}
//¥Ú”°
template<class T>
void print(T array[], int size)
{
	if (array == NULL || size==0)
	{
		return;
	}
	for (int i = 0; i < size; i++)
	{
		cout << array[i] << "  ";
	}
}


void main22()
{
	int array[] = { 11, 33, 44, 33, 22, 2, 3, 6, 9 };
	int size = sizeof(array) / sizeof(int);
	print(array, size);
}