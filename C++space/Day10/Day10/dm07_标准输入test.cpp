
#include <iostream>
using namespace std;
#include "string"

/*
��׼����������cin
	cin.get() //һ��ֻ�ܶ�ȡһ���ַ�
	cin.get(һ������) //��һ���ַ�
	cin.get(��������) //���Զ��ַ���
	cin.getline()
	cin.ignore()
	cin.peek()
	cin.putback()
*/


void main71()
{
	char	mybuf[1024];
	int		myInt;
	long	myLong;

	cin >> myInt;

	cin >> myLong;

	cin >> mybuf; // �����ո�ֹͣ���� ���� 

	cout << "myInt:" << myInt << "myLong" << myLong << "mybuf:" << mybuf << endl;

	system("pause");
}

//get()
void main72()
{
	char ch;
	while ( (ch=cin.get() )!= EOF )
	{
		cout << ch << endl;
	}
	system("pause");
}

void main73()
{
	char a, b, c;

	cout << "cin.get(a) ���������û������,��������� \n";
	cin.get(a);
	cin.get(b);
	cin.get(c);

	cout << a << b << c << "��Ϊ������������,���򲻻�����\n";

	cin.get(a).get(b).get(c);

	cout << a << b << c;

	system("pause");
}

//getline�������Խ��� �ո�
void main74()
{
	char buf1[256];
	char buf2[256];

	cout << "������һ���ַ��� ���ж���ո� aa bb cc dd\n";

	cin >> buf1;

	cin.getline(buf2, 256);

	cout << "buf1:" << buf1 << "buf2:" << buf2 << endl; 
	system("pause");
}

void main75()
{
	char buf1[256];
	char buf2[256];

	cout << "������һ���ַ��� ���ж���ո�aa  bbccdd\n";

	cin >> buf1;
	cin.ignore(20);
	int myint = cin.peek();
	cout << "myint:" << myint << endl; 

	cin.getline(buf2, 256);

	cout << "buf1:" << buf1 << "\nbuf2:" << buf2 << endl; 
	system("pause");
}

//����:������������ַ����ֿ�����
int main78() 
{
	cout << "Please, enter a number or a word: ";
	char c = std::cin.get();

	if ( (c >= '0') && (c <= '9') ) //������������ַ��� �ֿ�����
	{
		int n; //���������� �м��пո� ʹ��cin >>n
		cin.putback (c);
		cin >> n;
		cout << "You entered a number: " << n << '\n';
	}
	else
	{
		string str;
		cin.putback (c);
		//cin.getline(str);
		getline (cin, str); // //�ַ��� �м�����пո� ʹ�� cin.getline();
		cout << "You entered a word: " << str << '\n';
	}
	system("pause");
	return 0;
}
