#define  _CRT_SECURE_NO_WARNINGS
#include <iostream>
using namespace std;
#include "set"

//1 ���� Ԫ��Ψһ �Զ�����(Ĭ������� �Ǵ�С����) ���ܰ���[]��ʽ����Ԫ�� 
// �����  

//setԪ�ص����/����/ɾ����������
void main91()
{
	set<int>  set1;
	
	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set1.insert(tmp);
	}
	//����Ԫ�� �ظ���
	set1.insert(100);
	set1.insert(100);
	set1.insert(100);
	
	for (set<int>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << *it << " ";
	}

	//ɾ������ 
	while ( !set1.empty())
	{
		set<int>::iterator it = set1.begin();
		cout << *it << " ";
		set1.erase(set1.begin());
	}
}

//2 �Ի������������� set���Զ������� 
void main92()
{
	set<int> set1;  
	set<int, less<int>> set2;   //Ĭ������������� 

	set<int, greater<int>> set3;  //�Ӵ� �� С

	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set3.insert(tmp);
	}

	//�Ӵ� �� С
	for (set<int, greater<int>>::iterator it = set3.begin(); it != set3.end(); it++  )
	{
		cout << *it << endl;
	}
}


class Student
{
public:
	Student(char *name, int age)
	{
		strcpy(this->name, name);
		this->age = age;
	}
public:
	char name[64];
	int		age;
};

//�º��� 
struct FuncStudent
{
	bool operator()(const Student &left, const Student &right)
	{
		if (left.age < right.age)  //�����ߵ�С �ͷ����� ��С�����������������
		{
			return true;
		}
		else
		{
			return false; 
		}
	}
};

//3 �Զ����������͵����� �º������÷�
void main93()
{
	Student s1("s1", 31);
	Student s2("s2", 22);
	Student s3("s3", 44);
	Student s4("s4", 11);
	Student s5("s5", 31);

	set<Student, FuncStudent> set1;
	set1.insert(s1);
	set1.insert(s2);
	set1.insert(s3);
	set1.insert(s4);
	set1.insert(s5); //�������31�� �ܲ���ɹ�  
	//���֪�� ���� �Ľ��

	//����
	for (set<Student, FuncStudent>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << it->age << "\t" <<  it->name << endl;
	}
}

//typedef pair<iterator, bool> _Pairib;
//4 ����ж� set1.insert�����ķ���ֵ
//Pair���÷� 
void main94()
{
	Student s1("s1", 31);
	Student s2("s2", 22);
	Student s3("s3", 44);
	Student s4("s4", 11);
	Student s5("s5", 31);

	set<Student, FuncStudent> set1;
	pair<set<Student, FuncStudent>::iterator, bool> pair1 = set1.insert(s1);
	if (pair1.second == true)
	{
		cout << "����s1�ɹ�" << endl;
	}
	else
	{
		cout << "����s1ʧ��" << endl;
	}

	set1.insert(s2);

	//���֪�� ���� �Ľ��
	pair<set<Student, FuncStudent>::iterator, bool> pair5 = set1.insert(s5); //�������31�� �ܲ���ɹ�  
	if (pair5.second == true)
	{
		cout << "����s1�ɹ�" << endl;
	}
	else
	{
		cout << "����s1ʧ��" << endl;
	}

	//����
	for (set<Student, FuncStudent>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << it->age << "\t" <<  it->name << endl;
	}
}


//find����  equal_range 
//���ؽ����һ��pair
void main95()
{
	set<int> set1;  

	for (int i=0; i<10; i++)
	{
		set1.insert(i+1);
	}

	//�Ӵ� �� С
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++  )
	{
		cout << *it << " ";
	}
	cout << endl;

	set<int>::iterator it0 =  set1.find(5);
	cout << "it0:" << *it0 << endl;

	int num1 = set1.count(5);
	cout << "num1:" << num1 << endl;

	set<int>::iterator it1 =   set1.lower_bound(5); // ���ڵ���5��Ԫ�� �� ��������λ��
	cout << "it1:" << *it1 << endl;
	
	set<int>::iterator it2 =   set1.lower_bound(5); // ����5��Ԫ�� �� ��������λ��
	cout << "it2:" << *it2 << endl;

	//
	//typedef pair<iterator, bool> _Pairib;
	//typedef pair<iterator, iterator> _Pairii;
	//typedef pair<const_iterator, const_iterator> _Paircc;
	//��5Ԫ��ɾ����
	set1.erase(5); 
	pair<set<int>::iterator, set<int>::iterator>  mypair = set1.equal_range(5);
	set<int>::iterator it3 = mypair.first;
	cout << "it3:" << *it3 << endl;  //5  //6

	set<int>::iterator it4 =  mypair.second; 
	cout << "it4:" << *it4 << endl;  //6  //6

}

void main999()
{
	//main91();
	//main92();
	//main93();
	//main94();
	//main95();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}