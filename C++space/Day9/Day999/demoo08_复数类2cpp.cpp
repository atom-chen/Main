#include<iostream>
using namespace std;
/*
template<class T>
class Complex
{
public:
	Complex(T a, T b);
	void printCom();
	Complex& operator+(const Complex &obj);
	friend ostream& operator<< <T>(ostream &out, const Complex<T> &obj);
	Complex& operator-(const Complex &obj);

protected:
private:
T a;
T b;
};

template<class T>
Complex<T>& Complex<T>::operator-(const Complex<T> &obj)
{
	this->a = this->a - obj.a;
	this->b = this->b - obj.b;
	return *this;
}
//重载函数提出来
template<class T>
Complex<T>& Complex<T>::operator+(const Complex<T> &obj)
{
	this->a = this->a + obj.a;
	this->b = this->b + obj.b;
	return *this;
}


//构造函数提出来
template<class T>
Complex<T>::Complex(T a, T b)
{
	this->a = a;
	this->b = b;
}
//普通函数提出来；
template<class T>
void Complex<T>::printCom()
{
	cout << a << "+" << b << "i" << endl;
}

//友元重载提出来
template<class T>
ostream& operator<<(ostream &out, const Complex<T> &obj)
{
	cout << "a=" << obj.a << "b=" << obj.b << endl;
	return out;
}


void main()
{
Complex<int> c1(1, 2);
Complex<int> c2(3, 4);
Complex<int> c3 = c1 + c2;
//c3.printCom();
 cout << c3 << endl;

}

*/