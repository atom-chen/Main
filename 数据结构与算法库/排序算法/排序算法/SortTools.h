#include<iostream>
#include<string>
#include<vector>
#include <iterator>
using namespace std;

template<class T>
inline void exchange(T& t1,T& t2)
{
	T temp = t1;
	t1 = t2;
	t2 = temp;
}

template<class T>
inline bool isSort(vector<T> arr,bool(*compare)(const T& t1, const T& t2))
{
	for (int i = 0; i < arr.size() - 1; i++)
	{
		if (compare(arr[i],arr[i+1]))
		{
			return false;
		}
	}
	return true;
}

template<class T>
inline void print(vector<T> arr)
{
	for (auto iter = arr.begin(); iter != arr.end();)
	{
		cout << *iter << " ";
		iter++;
	}
	cout <<endl;
}

inline void print(int* arr,unsigned length)
{
	for (unsigned i = 0; i < length; i++)
	{
		cout << arr[i] << " ";
	}
	cout<<endl;
	
}