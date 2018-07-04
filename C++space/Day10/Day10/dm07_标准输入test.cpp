
#include <iostream>
using namespace std;
#include "string"

/*
标准输入流对象cin
	cin.get() //一次只能读取一个字符
	cin.get(一个参数) //读一个字符
	cin.get(三个参数) //可以读字符串
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

	cin >> mybuf; // 遇见空格停止接受 数据 

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

	cout << "cin.get(a) 如果缓冲区没有数据,则程序阻塞 \n";
	cin.get(a);
	cin.get(b);
	cin.get(c);

	cout << a << b << c << "因为缓冲区有数据,程序不会阻塞\n";

	cin.get(a).get(b).get(c);

	cout << a << b << c;

	system("pause");
}

//getline函数可以接受 空格
void main74()
{
	char buf1[256];
	char buf2[256];

	cout << "请输入一个字符串 含有多个空格 aa bb cc dd\n";

	cin >> buf1;

	cin.getline(buf2, 256);

	cout << "buf1:" << buf1 << "buf2:" << buf2 << endl; 
	system("pause");
}

void main75()
{
	char buf1[256];
	char buf2[256];

	cout << "请输入一个字符串 含有多个空格aa  bbccdd\n";

	cin >> buf1;
	cin.ignore(20);
	int myint = cin.peek();
	cout << "myint:" << myint << endl; 

	cin.getline(buf2, 256);

	cout << "buf1:" << buf1 << "\nbuf2:" << buf2 << endl; 
	system("pause");
}

//案例:输入的整数和字符串分开处理
int main78() 
{
	cout << "Please, enter a number or a word: ";
	char c = std::cin.get();

	if ( (c >= '0') && (c <= '9') ) //输入的整数和字符串 分开处理
	{
		int n; //整数不可能 中间有空格 使用cin >>n
		cin.putback (c);
		cin >> n;
		cout << "You entered a number: " << n << '\n';
	}
	else
	{
		string str;
		cin.putback (c);
		//cin.getline(str);
		getline (cin, str); // //字符串 中间可能有空格 使用 cin.getline();
		cout << "You entered a word: " << str << '\n';
	}
	system("pause");
	return 0;
}
