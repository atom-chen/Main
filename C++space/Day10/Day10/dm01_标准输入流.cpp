#include<iostream>
#include <string>
using namespace std;

void main01()
{
	char mybuf[1024];
	int myInt;
	long myLong;
	cin >> myInt;
	cin >> myLong;
	
	cin >> mybuf;
	cout << "myInt" << myInt << "myLong"<<myLong<<
		"mybuf"<<mybuf<<endl;
	 
}

void main02()
{ 
	char ch;
	while ((ch = cin.get()) != EOF)
	{
		cout<<ch<<endl;
	}
}

void main03()
{
	char a, b, c;
	cin.get(a).get(b).get(c);
	cout<<a<<b<<c<<endl;
}
void main04()
{
	char buf1[256];
	char buf2[256];
	cin >> buf1;
	cin.getline(buf2, 256);
	cout<<buf1<<endl;
	cout << buf2 << endl;
}

void main06()
{
	char buf1[256];
	char buf2[256];
	cin >> buf1;
	
	cin.ignore(20);
	int a = cin.peek();
	cout << buf1 << endl;
	cout << a   << endl;
}

