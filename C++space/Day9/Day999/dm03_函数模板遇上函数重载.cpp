
#include <iostream>
using namespace std;



//�� ���Ͳ����� ===, �������Ա���б���
// ���ͱ�� 
//template ����C++������ ��Ҫ��ʼ���ͱ���� .����T, ��Ҫ��㱨��
template <typename T>
void myswap(T &a, T &b)
{
	T c = 0;
	c = a;
	a = b;
	b = c;
	cout << "hello ....����ģ�庯�� ��ӭ calll ��" << endl;
}

void myswap(int a, char c)
{
	cout << "a:" << a << "c:" << c << endl;
	cout << "������ͨ���� ��ӭ����" << endl;
}


void main()
{
	int		a = 10;
	char	c = 'z';

	myswap(a, c); // ��ͨ�����ĵ���:  ���Խ�����ʽ������ת�� 
	myswap(c, a); //

	myswap(a, a); // ����ģ�庯���ĵ���(����:���Ͳ�����): ���ϸ�İ������ͽ���ƥ��,��������Զ�����ת��

	
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}