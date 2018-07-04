
#include <iostream>
using namespace std;

#include "string"
#include <vector>
#include <list>
#include "set"
#include <algorithm>
#include "functional"


//plus<int> Ԥ����õĺ������� ��ʵ�ֲ�ͬ���͵����ݵ� + ����
//ʵ���� �������� ���㷨�ķ��� ===��ͨ������������ʵ�ֵġ�������

//˼������ô��֪�� plus<type> ����������
void main21()
{
	/*
	template<class _Ty>
	struct plus
		: public binary_function<_Ty, _Ty, _Ty>
	{	// functor for operator+
		_Ty operator()(const _Ty& _Left, const _Ty& _Right) const
		{	// apply operator+ to operands
			return (_Left + _Right);
		}
	};
	*/
	plus<int> intAdd;
	int x = 10; 
	int y = 20;
	int z = intAdd(x, y); // x + y 
	cout << "z:" << z << endl;

	plus<string> stringAdd;

	string s1 = "aaa";
	string s2 = "bbb";
	string s3 = stringAdd(s1, s2);
	cout << "s3:" << s3 << endl;

	vector<string> v1;
	v1.push_back("bbb");
	v1.push_back("aaa");
	v1.push_back("ccc");
	v1.push_back("zzz");
	v1.push_back("ccc");
	v1.push_back("ccc");

	/*
	template<class _Ty>
	struct greater
		: public binary_function<_Ty, _Ty, bool>
	{	// functor for operator>
		bool operator()(const _Ty& _Left, const _Ty& _Right) const
		{	// apply operator> to operands
			return (_Left > _Right);
		}
	};
	*/
	sort(v1.begin(), v1.end(), greater<string>() );

	for (vector<string>::iterator it=v1.begin(); it!=v1.end(); it++)
	{
		cout << *it << endl;
	}

	//�� ccc ���ֵĸ���
	string sc = "ccc";

	//equal_to<string>() ���������� left��������������right��������sc
	//bind2nd���������� ����Ԥ���庯������ �� �ڶ����������а�
	int num = count_if(v1.begin(), v1.end(), bind2nd(equal_to<string>(), sc) );
	cout << "num: " << num << endl;
}

class IsGreat
{
public:
	IsGreat(int i)
	{
		m_num = i;
	}
	bool operator()(int &num)
	{
		if (num > m_num)
		{
			return true;
		}
		return false;
	}
private:
	int m_num;
};

void main22()
{
	vector<int> v1;
	for (int i=0; i<10; i++)
	{
		v1.push_back(i+1);
	}
	for (vector<int>::iterator it=v1.begin(); it!=v1.end(); it++ )
	{
		cout << *it << " ";
	}
	cout << endl;

	int num1 = count(v1.begin(), v1.end(), 3);
	cout << "num1:" << num1 <<endl;

	//ͨ�� ν�� �����2 �ĸ���
	int num2 = count_if(v1.begin(), v1.end(),  IsGreat(2));
	cout << "num2:" << num2 <<endl;

	/*
	template<class _Ty>
	struct greater
		: public binary_function<_Ty, _Ty, bool>
	{	// functor for operator>
		bool operator()(const _Ty& _Left, const _Ty& _Right) const
		{	// apply operator> to operands
			return (_Left > _Right);
		}
	};
	*/
	//ͨ�� Ԥ����ĺ������� �����2 �ĸ���
	//greater<int>() ���������� ���������������Ԫ�� ���Ҳ����̶���2 ��ͨ��bind2nd���ģ�
	int num3 = count_if(v1.begin(), v1.end(), bind2nd (greater<int>(), 2) );
	cout << "num3:" << num3 <<endl;

	//�� �����ĸ���
	int num4 = count_if(v1.begin(), v1.end(), bind2nd (modulus<int>(), 2) );
	cout << "�����ĸ���num4:" << num4 <<endl;

	//�� ż���ĸ��� ȡ����(negator) 
	int num5 = count_if(v1.begin(), v1.end(), not1( bind2nd (modulus<int>(), 2) ) );
	cout << "ż���ĸ��� num5:" << num5 <<endl;

}
void main2222()
{
	//main21();
	main22(); //�����������ۺϰ���
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}