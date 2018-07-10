
#include <iostream>
using namespace std;
#include "dm09_complex.h"

namespace demo09
{
	//���캯����ʵ�� д��������ⲿ
	template <typename T>
	Complex<T>::Complex(T a, T b)
	{
		this->a = a;
		this->b = b;
	}

	template <typename T>
	void Complex<T>::printCom()
	{
		cout << "a:" << a << " b: " << b << endl;
	}


	template <typename T>
	Complex<T>  Complex<T>::operator+ (Complex<T> &c2)
	{
		Complex tmp(a + c2.a, b + c2.b);
		return tmp;
	}


	template <typename T>
	ostream & operator<<(ostream &out, Complex<T> &c3)
	{
		out << c3.a << " + " << c3.b << "i" << endl;
		return out;
	}
}
//////////////////////////////////////////////////////////////////////////

// ���� ��Ԫ����
// template <typename T>
// Complex<T> MySub(Complex<T> &c1, Complex<T> &c2)
//{
//	Complex<T> tmp(c1.a - c2.a, c1.b - c2.b);
//	return tmp;
//}