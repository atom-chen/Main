
#include <iostream>
using namespace std;

// 1.cpp

// g++ -S 1.cpp  -o 1.s
template <typename T>
void myswap(T &a, T &b)
{
	T c = 0;
	c = a;
	a = b;
	b = c;
	cout << "hello ....����ģ�庯�� ��ӭ calll ��" << endl;
}

int main()
{
	{

		int x = 10; 
		int y = 20;

		myswap<int>(x, y); //1 ����ģ�� ��ʾ���� ����

		printf("x:%d y:%d \n", x, y);
	}


	{
		char a = 'a'; 
		char b = 'b';

		myswap<char>(a, b); //1 ����ģ�� ��ʾ���� ����
		printf("a:%c b:%c \n", a, b);
	}
	return 0;
}
