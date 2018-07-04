
#include <iostream>
using namespace std;
#include "string"
#include "algorithm"

//string�ĳ�ʼ��
void main21()
{
	string s1 = "aaaa";
	string s2("bbbb");
	string s3 = s2; //ͨ���������캯�� ����ʼ������s3
	string s4(10, 'a');

	cout << "s1:" << s1 << endl;
	cout << "s2:" << s2 << endl;
	cout << "s3:" << s3 << endl;
	cout << "s4:" << s4 << endl;
}

//string�� ����
void main22()
{
	string s1 = "abcdefg";

	//1 ���鷽ʽ
	for (int i=0; i<s1.length(); i++)
	{
		cout << s1[i] << " ";
	}
	cout << endl;

	//2 ������
	for (string::iterator it = s1.begin(); it != s1.end(); it++ )
	{
		cout << *it << " ";
	}
	cout << endl;

	try
	{
		for (int i=0; i<s1.length() + 3; i++)
		{
			cout << s1.at(i) << " ";  //�׳��쳣
		}
	}
	catch ( ... )
	{
		cout << "�����쳣\n" ;
	}

	cout << "at֮��" << endl;
	/*
	try
	{
		for (int i=0; i<s1.length() + 3; i++)
		{
			cout << s1[i] << " "; //���ִ��� ���������׳��쳣 ���������ж�
		}
	}
	catch ( ... )
	{
		cout << "�����쳣\n" ;
	}
	*/
	
}

//�ַ�ָ���string��ת��
void main23()
{
	string s1 = "aaabbbb";

	//1 s1===>char *
	//printf("s1:%s \n", s1.c_str());

	//2 char *====>sting 


	//3 s1������ copy buf��
	//char buf1[128] = {0};
	//s1.copy(buf1, 3, 0);  //ע�� ֻ����copy3���ַ� ������C�����ַ���
	//cout << "buf1:" << buf1 << endl; 
}

//�ַ����� ����
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


//�ַ����Ĳ��Һ��滻
void main25()
{
	string s1 = "wbm hello wbm 111  wbm 222  wbm 333 ";
	//			 ��
	//��һ�� ����wbm index

	int index = s1.find("wbm", 0); //λ���±� ��0��ʼ
	cout << "index: " << index << endl;

	//����1 ��wbm���ֵĴ��� ÿһ�γ��ֵ������±�
	int offindex = s1.find("wbm", 0);
	while (offindex != string::npos)
	{
		cout << "offindex:" << offindex << endl;
		offindex = offindex + 1;
		offindex = s1.find("wbm", offindex); //wang bao ming 
	}

	//����2  ��Сдwbm===>WBM

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

	cout << "s1�滻��Ľ��: " << s1 << endl;

}


//�ضϣ�����ɾ�����Ͳ���
void main26()
{
	string s1 = "hello1 hello2 hello1";
	string::iterator it = find(s1.begin(), s1.end(), 'l');
	if (it != s1.end() )
	{
		s1.erase(it);
	}
	cout << "s1ɾ��l�Ժ�Ľ��:" << s1 << endl;

	s1.erase(s1.begin(), s1.end() );
	cout << "s1ȫ��ɾ��:" << s1 << endl;
	cout << "s1���� " << s1.length() << endl;

	string s2 = "BBB";

	s2.insert(0, "AAA"); // ͷ�巨
	s2.insert(s2.length(), "CCC");

	cout << s2 << endl;
}

void main27()
{
	string s1 = "AAAbbb";
	//1��������ڵ�ַ 2�������� 3Ԥ����ĺ�������
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