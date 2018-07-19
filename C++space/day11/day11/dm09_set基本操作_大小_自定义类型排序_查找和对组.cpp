#define  _CRT_SECURE_NO_WARNINGS
#include <iostream>
using namespace std;
#include "set"
#include <xfunctional>

//1 ���� ����(Ĭ�ϴ�С����) Ψһ (��ڶ����� �������ݽṹ�ı���) 
void main91()
{
	set<int> set1;
	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set1.insert(tmp);
	}
	set1.insert(100);
	set1.insert(100);
	set1.insert(100);

	//��ӡ���
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	//ɾ������
	cout << "\nɾ������";
	while (!set1.empty())
	{
		set<int>::iterator it = set1.begin();
		printf("%d ", *it);
		set1.erase(set1.begin());
	}
}

//2 ���� ��С���� �Ӵ�С
void main92()
{
	set<int, greater<int>> set1;
	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set1.insert(tmp);
	}
	set1.insert(100);
	set1.insert(100);
	set1.insert(100);

	//��ӡ���
	for (set<int, greater<int>>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	//ɾ������
	cout << "\nɾ������";
	while (!set1.empty())
	{
		set<int, greater<int>>::iterator it = set1.begin();
		printf("%d ", *it);
		set1.erase(set1.begin());
	}
}

//3 �Զ����������� ����
//03 �º��� �������� ����() ���� ���бȽϴ�С
//��Ŀ��ѧ������ѧ�ţ��������ԣ���Ҫ��������뼸��ѧ������set�����У�
//ʹ�������е�ѧ����ѧ�ŵ���������
class Student
{
public:
	Student(char *name, int age)
	{
		strcpy(this->name, name);
		this->age = age;
	}
protected:
public:
	char name[64];
	int age ;
};

//��������
struct StuFunctor
{
	bool operator()(const Student &left, const Student &right)
	{
		return (left.age < right.age); 
	}
};

//
int main93()
{
	set<Student, StuFunctor> set1;
	Student s1("��1", 32);

	set1.insert(s1);
	set1.insert(Student("��2", 32) );
	set1.insert(Student("��3", 53) );
	set1.insert(Student("��4", 34) );

	//��ӡ���
	for (set<Student, StuFunctor >::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< (*it).name << " ";
	}
	system("pause");
	return 0;
}


//04��ʾ��set���ϲ��ҹ���
int main94()
{
	int		i = 0;
	set<int> set1;

	for (i=1; i<10; i++)
	{
		set1.insert(i);
	}
	//��ӡ���
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	set<int>::iterator it1 =  set1.lower_bound(5); //���ڵ���5������
	set<int>::iterator it2 =  set1.upper_bound(5); //����5�ĵ�����

	//ͨ������������Ԫ�صĲ���
	cout<<"it1 "<<*it1<<" "<<"it2 "<<*it2<<endl;

	//
	pair <set<int>::iterator, set<int>::iterator> pairIt = set1.equal_range(5);

	set<int>::iterator it3 = pairIt.first; //��ȡ��һ��
	set<int>::iterator it4 = pairIt.second; //��ȡ�ڶ���

	cout<<"it3 "<<*it3<<" "<<"it4 "<<*it4<<endl;
	system("pause");
	return 0;
}

