
#include <iostream>
using namespace std;

#include "string"
#include <vector>
#include <list>
#include "set"
#include <algorithm>
#include "functional"
#include "iterator"  //�������������ͷ�ļ�
#include<numeric>


void printV(vector<int> &v)
{
	for (vector<int>::iterator it=v.begin(); it!=v.end(); it++)
	{
		cout << *it << " ";
	}
}

void printList(list<int> &v)
{
	for (list<int>::iterator it=v.begin(); it!=v.end(); it++)
	{
		cout << *it << " ";
	}
}

void showElem(int &n)
{
	cout << n << " ";
}

class CMyShow
{
public:
	CMyShow()
	{
		num = 0;
	}
	void operator()(int &n)
	{
		num ++;
		cout << n << " ";
	}
	void printNum()
	{
		cout << "num:" << num << endl;
	}
protected:
private:
	int num;
};

void main41_foreach()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	printV(v1);
	cout << endl;

	/*
	template<class _InIt,
	class _Fn1> inline
		_Fn1 for_each(_InIt _First, _InIt _Last, _Fn1 _Func)
	{	// perform function for each element
		_DEBUG_RANGE(_First, _Last);
		_DEBUG_POINTER(_Func);
		return (_For_each(_Unchecked(_First), _Unchecked(_Last), _Func));
	} */

	//�������� �ص�������ڵ�ַ
	for_each(v1.begin(), v1.end(), showElem);
	cout << endl;

	for_each(v1.begin(), v1.end(), CMyShow());
	cout << endl;

	 CMyShow mya;
	 CMyShow my1 = for_each(v1.begin(), v1.end(),mya); //��my1��ʼ��
	 mya.printNum();  //ma1��my1 ��������ͬ�Ķ���
	 my1.printNum();

	 my1 = for_each(v1.begin(), v1.end(),mya);  //��my1��ֵ
	 my1.printNum();
}


int  increase(int i)
{
	return i+100;
}

void main42_transform()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	printV(v1);
	cout << endl;

	//transform ʹ�ûص�����
	transform(v1.begin(), v1.end(), v1.begin(),  increase ); 
	printV(v1);
	cout << endl;

	//transform ʹ�� Ԥ����ĺ�������
	transform(v1.begin(), v1.end(), v1.begin(),  negate<int>() ); 
	printV(v1);
	cout << endl;

	//transform ʹ�� ���������� �ͺ�������
	list<int> mylist;
	mylist.resize( v1.size() );

	transform(v1.begin(), v1.end(), mylist.begin(),  bind2nd( multiplies<int>(), 10 ) ); 
	printList(mylist);
	cout << endl;

	//transform Ҳ���԰������� ֱ���������Ļ
	transform(v1.begin(), v1.end(), ostream_iterator<int>(cout, " " ), negate<int>() );
	cout << endl;
}


//һ������£�for_each��ʹ�õĺ������󣬲��������ã�û�з���ֵ
//transform��ʹ�õĺ������󣬲���һ�㲻ʹ�����ã����ǻ��з���ֵ
int showElem2(int n)
{
	cout << n << " ";
	return n;
}

void main43_transform_pk_foreach()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	vector<int> v2 = v1;

	for_each(v1.begin(), v1.end(), showElem);


	//transform �� ���������Ҫ��
	/*
c:\program files\microsoft visual studio 10.0\vc\include\algorithm(1119): 
	�μ������ڱ���ĺ��� ģ�� ʵ����
		��_OutIt std::_Transform1<int*,_OutIt,
		void(__cdecl *)(int &)>(_InIt,_InIt,_OutIt,_Fn1,
		std::tr1::true_type)��������
1>          with
1>          [
1>              _OutIt=std::_Vector_iterator<std::_Vector_val<int,std::allocator<int>>>,
1>              _InIt=int *,
1>              _Fn1=void (__cdecl *)(int &)
1>          ]
	*/

	/*
	template<class _InIt,
	class _OutIt,
	class _Fn1> inline
		_OutIt _Transform(_InIt _First, _InIt _Last,
		_OutIt _Dest, _Fn1 _Func)
	{	// transform [_First, _Last) with _Func
		for (; _First != _Last; ++_First, ++_Dest)
			*_Dest = _Func(*_First);  //������ Ϊʲô Ҫ�з���ֵ
		return (_Dest);
	}
	*/
	transform(v2.begin(), v2.end(), v2.begin(), showElem2);
}

void main44_adjacent_find()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(2);
	v1.push_back(2);
	v1.push_back(3);
	v1.push_back(5);

	vector<int>::iterator it =  adjacent_find(v1.begin(), v1.end() );
	if (it == v1.end())
	{
		cout << "û���ҵ� �ظ���Ԫ��" << endl;
	}
	else
	{
		cout << *it << endl;
	}
	int index = distance(v1.begin(), it);
	cout << index << endl;
	
}

// 0 1  2  3 ......n-1
//���ַ� 1K = 1024  10��  �ٶȿ�

void main45_binary_search()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	v1.push_back(9);

	bool b = binary_search(v1.begin(), v1.end(), 7);
	if (b == true)
	{
		cout << "�ҵ���" << endl;
	}
	else
	{
		cout << "û����" << endl;
	}

}

void main46_count()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	v1.push_back(7);
	v1.push_back(9);
	v1.push_back(7);

	int num = count(v1.begin(), v1.end(), 7);
	
	cout << num << endl;
	

}

bool GreatThree(int iNum)
{
	if (iNum > 3)
	{
		return true;
	}
	return false;
}
void main46_countif()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	v1.push_back(7);
	v1.push_back(9);
	v1.push_back(7);

	int num = count_if(v1.begin(), v1.end(), GreatThree);
	cout << "num:" << num << endl;
}


void main47_find_findif()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	v1.push_back(7);
	v1.push_back(9);
	v1.push_back(7);

	vector<int>::iterator it =  find(v1.begin(), v1.end(), 5);
	cout << "*it:" << *it << endl; 

	//��һ������3��λ��
	vector<int>::iterator it2 =  find_if(v1.begin(), v1.end(), GreatThree);
	cout << "*it2:" << *it2 << endl; 
}

void main_merge()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	vector<int> v2;
	v2.push_back(2);
	v2.push_back(4);
	v2.push_back(6);

	vector<int> v3;
	v3.resize(v1.size() + v2.size() );

	merge(v1.begin(), v1.end(), v2.begin(), v2.end(), v3.begin() );

	printV(v3);
}

class Student
{
public:
	Student(string name, int id)
	{
		m_name = name;
		m_id = id;
	}
	void printT()
	{
		cout << "name: " << m_name << " id " << m_id << endl;
	}
public:
	string	m_name;
	int		m_id;
};

bool CompareS(Student &s1, Student &s2)
{
	return (s1.m_id < s2.m_id);
}


void main_sort()
{
	Student s1("�ϴ�", 1);
	Student s2("�϶�", 2);
	Student s3("����", 3);
	Student s4("����", 4);
	vector<Student> v1;
	v1.push_back(s4);
	v1.push_back(s1);
	v1.push_back(s3);
	v1.push_back(s2);

	for (vector<Student>::iterator it=v1.begin(); it!=v1.end(); it++)
	{
		it->printT() ;
	}

	//sort �����Զ��庯������ �����Զ����������͵����� 
	//�滻 �㷨��ͳһ�� (ʵ�ֵ��㷨���������͵ķ���) ===>�����ֶκ�������
	sort(v1.begin(), v1.end(), CompareS );

	for (vector<Student>::iterator it=v1.begin(); it!=v1.end(); it++)
	{
		it->printT() ;
	}

}

void main_random_shuffle()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);

	random_shuffle(v1.begin(), v1.end());
	printV(v1);

	string str = "abcdefg";
	random_shuffle(str.begin(), str.end());
	cout << "str: " << str << endl;
}

void main_reverse()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	reverse(v1.begin(), v1.end());
	printV(v1);
}

void main52_copy()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);

	vector<int> v2;
	v2.resize(v1.size() );

	copy(v1.begin(), v1.end(), v2.begin());
	printV(v2);
}


bool great_equal_5(int &n)
{
	if (n>=5)
	{
		return true;
	}
	return false;
}
void main53_replace_replaceif()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);
	v1.push_back(3);
	replace(v1.begin(), v1.end(), 3, 8);

	// >=5
	replace_if(v1.begin(), v1.end(), great_equal_5, 1);

	printV(v1);

}

void main54_swap()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	vector<int> v2;
	v2.push_back(2);
	v2.push_back(4);
	v2.push_back(6);
	

	swap(v1, v2);
	printV(v1);

}

void main55_accumulate()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	int tmp = accumulate(v1.begin(), v1.end(), 100);
	cout << tmp << endl;
}

void main56_fill()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	fill(v1.begin(), v1.end(), 8);
	printV(v1);
	
}

void main57_union()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);

	vector<int> v2;
	v2.push_back(2);
	v2.push_back(4);
	v2.push_back(6);

	vector<int> v3;
	v3.resize(v1.size() + v2.size());


	set_union(v1.begin(), v1.end(), v2.begin(), v2.end(), v3.begin());
	printV(v3);
}
void main()
{
	//main41_foreach();
	//main42_transform();
	// main43_transform_pk_foreach();
	//main44_adjacent_find();
	//main45_binary_search();
	//main46_count();
	//main46_countif();
	//main47_find_findif();

	//main_merge(); //48
	//main_sort();
	//main_random_shuffle();
	//main_reverse();//51

	//main52_copy();
	//main53_replace_replaceif();
//main54_swap();
	//main55_accumulate();
	//main56_fill();
	main57_union();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}