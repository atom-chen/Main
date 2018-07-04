
#include <iostream>
using namespace std;

#include "string"
#include <vector>
#include <list>
#include "set"
#include <algorithm>
#include "functional"


//二元函数对象
template <typename T>
class SumAdd
{
public:
	T operator()(T t1, T t2)
	{
		return t1 + t2;
	}
};

int mysum(int &t1, int &t2)
{
	return t1 + t2;
}

void main34()
{
	//v1 v2 ==> v3
	vector<int> v1, v2;
	vector<int> v3;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	v2.push_back(2);
	v2.push_back(4);
	v2.push_back(6);

	v3.resize(10);

	/*
		template<class _InIt1,
		class _InIt2,
		class _OutIt,
		class _Fn2> inline
			_OutIt transform(_InIt1 _First1, _InIt1 _Last1,
			_InIt2 _First2, _OutIt _Dest, _Fn2 _Func)
		{	// transform [_First1, _Last1) and [_First2, ...) with _Func
			_DEBUG_RANGE(_First1, _Last1);
			_DEBUG_POINTER(_Dest);
			_DEBUG_POINTER(_Func);
			if (_First1 != _Last1)
				return (_Transform2(_Unchecked(_First1), _Unchecked(_Last1),
				_First2, _Dest, _Func,
				_Is_checked(_Dest)));
			return (_Dest);
		}
	*/


	//  transform(v1.begin(), v1.end(), v2.begin(), v3.begin(), SumAdd<int>() );
	transform(v1.begin(), v1.end(), v2.begin(), v3.begin(), mysum );

	for (vector<int>::iterator it=v3.begin(); it!=v3.end(); it++ )
	{
		cout << *it << " ";
	}
	cout << endl;
}

void main3333()
{
	main34();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}