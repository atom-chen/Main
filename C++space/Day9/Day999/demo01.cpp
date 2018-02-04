#include <iostream>
using namespace std;

/*
void mySwap(int &a, int &b)
{
	int temp = a;
	a = b;
	b = temp;
}
void mySwap(char &a, char &b)
{
	char temp = a;
	a = b;
	b = temp;
}
*/
template <class T>
void mySwap(T &a,T &b)
{
	T temp;
	temp = a;
	a = b;
	b = temp;
}

void main11()
{
	int a = 10, b = 20;
	mySwap<int>(a, b);

	char x = 'a';
	char y = 'b';
	mySwap<char>(x, y);
	cout << "a" << a << "b" << b   << endl;
	system("pause");
}

