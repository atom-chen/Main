
#include <iostream>
using namespace std;
#include "string"
#include "algorithm"

//string的初始化
void main21()
{
	string s1 = "aaaa";
	string s2("bbbb");
	string s3 = s2; //通过拷贝构造函数 来初始化对象s3
	string s4(10, 'a');

	cout << "s1:" << s1 << endl;
	cout << "s2:" << s2 << endl;
	cout << "s3:" << s3 << endl;
	cout << "s4:" << s4 << endl;
}

//string的 遍历
void main22()
{
	string s1 = "abcdefg";

	//1 数组方式
	for (int i=0; i<s1.length(); i++)
	{
		cout << s1[i] << " ";
	}
	cout << endl;

	//2 迭代器
	for (string::iterator it = s1.begin(); it != s1.end(); it++ )
	{
		cout << *it << " ";
	}
	cout << endl;

	try
	{
		for (int i=0; i<s1.length() + 3; i++)
		{
			cout << s1.at(i) << " ";  //抛出异常
		}
	}
	catch ( ... )
	{
		cout << "发生异常\n" ;
	}

	cout << "at之后" << endl;
	/*
	try
	{
		for (int i=0; i<s1.length() + 3; i++)
		{
			cout << s1[i] << " "; //出现错误 不向外面抛出异常 引起程序的中断
		}
	}
	catch ( ... )
	{
		cout << "发生异常\n" ;
	}
	*/
	
}

//字符指针和string的转换
void main23()
{
	string s1 = "aaabbbb";

	//1 s1===>char *
	//printf("s1:%s \n", s1.c_str());

	//2 char *====>sting 


	//3 s1的内容 copy buf中
	//char buf1[128] = {0};
	//s1.copy(buf1, 3, 0);  //注意 只给你copy3个字符 不会变成C风格的字符串
	//cout << "buf1:" << buf1 << endl; 
}

//字符串的 连接
void main24()
{
	string s1 = "aaa";
	string s2 = "bbb";
	s1 = s1 + s2;
	cout << "s1:" << s1 << endl;

	string s3 = "333";
	string s4 = "444";
	s3.append(s4);
	cout << "s3:" << s3 << endl;
}


//字符串的查找和替换
void main25()
{
	string s1 = "wbm hello wbm 111  wbm 222  wbm 333 ";
	//			 ▲
	//第一次 出现wbm index

	int index = s1.find("wbm", 0); //位置下标 从0开始
	cout << "index: " << index << endl;

	//案例1 求wbm出现的次数 每一次出现的数组下标
	int offindex = s1.find("wbm", 0);
	while (offindex != string::npos)
	{
		cout << "offindex:" << offindex << endl;
		offindex = offindex + 1;
		offindex = s1.find("wbm", offindex); //wang bao ming 
	}

	//案例2  把小写wbm===>WBM

	string s3 = "aaa  bbb ccc";
	s3.replace(0, 3, "AAA");
	cout << "s3" << s3 << endl;

	offindex = s1.find("wbm", 0);
	while (offindex != string::npos)
	{
		cout << "offindex:" << offindex << endl;
		s1.replace(offindex,3, "WBM");
		offindex = offindex + 1;
		offindex = s1.find("wbm", offindex); //
	}

	cout << "s1替换后的结果: " << s1 << endl;

}


//截断（区间删除）和插入
void main26()
{
	string s1 = "hello1 hello2 hello1";
	string::iterator it = find(s1.begin(), s1.end(), 'l');
	if (it != s1.end() )
	{
		s1.erase(it);
	}
	cout << "s1删除l以后的结果:" << s1 << endl;

	s1.erase(s1.begin(), s1.end() );
	cout << "s1全部删除:" << s1 << endl;
	cout << "s1长度 " << s1.length() << endl;

	string s2 = "BBB";

	s2.insert(0, "AAA"); // 头插法
	s2.insert(s2.length(), "CCC");

	cout << s2 << endl;
}

void main27()
{
	string s1 = "AAAbbb";
	//1函数的入口地址 2函数对象 3预定义的函数对象
	transform(s1.begin(), s1.end(),s1.begin(), toupper);
	cout << "s1" << s1 << endl;

	string s2 = "AAAbbb";
	transform(s2.begin(), s2.end(), s2.begin(), tolower);
	cout << "s2:" << s2 << endl;

}


void main2222()
{
	//main21();
	//main22();
	//main23();
	//main24();
	//main25();
	//main26();
	main27();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}