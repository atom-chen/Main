#include<iostream>
using namespace std;
#include "Array.hpp"

class Teacher{
public:
	Teacher(const char* name, int age)
	{
		cout << name << age << "�в����Ĺ��췽��" << endl;
		this->age = age;
		strcpy(this->name, name);
	}
	Teacher(int age = 30)
	{
		cout <<age << "�޲����Ĺ��췽��" << endl;
		this->age = age;
		strcpy(this->name, "");
	}
	Teacher(const Teacher &obj)
	{
		cout << obj.name << obj.age <<"copy����" << endl;
		this->age = obj.age;
		strcpy(this->name, obj.name);
	}
	~Teacher()
	{
		cout << "������������" << endl;
		if (name != NULL)
		{
			delete [] name;
			age = -1;
		}
	}
	
public:
	void showInfo()
	{
		cout << "���ƣ�" << this->name <<"���䣺" <<this->age << endl;
	}
	void showAdd()
	{
		cout <<name<<"��ַ:"<< &name << endl;
	}
public:
	//����
	friend ostream& operator<<(ostream& out, const Teacher &obj);

	Teacher& operator=(const Teacher &obj)
	{
		cout << "���� =������" << endl;
		cout << *name << " " << name <<" "<<&name<<endl;
		if (this->name != NULL)
		{
			age = -1;
		}
		age = obj.age;
		
		//����
		strcpy(name, obj.name);
		return *this;
	}
private:
	char name[32];
	int age;

};
ostream& operator<<(ostream& out, const Teacher &obj)
{
	out << obj.name << " " << obj.age << endl;
	return out;
}
struct A{
	int a;
	short b;
	int c;
	char d;
};
struct B 
{
	int a;
	short b;
	char d;
	int c;
};
void main00()
{
	cout<<sizeof(A)<<endl;
	cout<<sizeof(B)<<endl;
}
void main1()
{
	//Array<Teacher> *array1;
	Teacher t1("Riven",90);
	Teacher t2 = t1;
	cout << t1 << endl;
	cout << t2 << endl;
	t1.showAdd();
	t2.showAdd();
	
	Teacher t3("A", 99);
	t3 = t1;
	t1.showAdd();
	cout << t1 << endl;
	t3.showAdd();
	cout << t3 << endl;
	

	system("pause");
}

void main9()
{
	Array<int> array1(10);
	for (int i = 0; i < array1.size(); i++)
	{
		array1[i] = i + 1;
		cout << array1[i] << endl;
	}
	Array<int> array2 = array1;
	cout << &array1 << endl;
	cout << &array2 << endl;
	for (int i = 0; i < array2.size(); i++)
	{
		array2[i] = i + 1;
		cout << array2[i] << endl;
	}

	cout<<array2<<endl;
	
}