#include<iostream>
using namespace std;
#include "Array.hpp"
/*
class Teacher{
public:
	Teacher(const char* name, int age)
	{
		cout << name << age << "�в����Ĺ��췽��" << endl;
		this->age = age;
		this->name = new char[strlen(name) + 1];
		strcpy(this->name, name);
	}
	Teacher(int age = 30)
	{
		cout << age << "�޲����Ĺ��췽��" << endl;
		this->age = age;
		this->name = new char[1];
		strcpy(this->name, "");
	}
	Teacher(const Teacher &obj)
	{
		cout << obj.name << obj.age << "copy����" << endl;
		this->age = obj.age;
		this->name = new char[strlen(obj.name) + 1];
		strcpy(this->name, obj.name);
	}
	~Teacher()
	{
		cout << "������������" << endl;
		if (name != NULL)
		{
			delete[] name;
			name = NULL;
			age = -1;
		}
	}

public:
	void showInfo()
	{
		cout << "���ƣ�" << this->name << "���䣺" << this->age << endl;
	}
	void showAdd()
	{
		cout << name << "��ַ:" << &name << endl;
	}
public:
	//����
	friend ostream& operator<<(ostream& out, const Teacher &obj);

	Teacher& operator=(const Teacher &obj)
	{
		cout << "���� =������" << endl;
		cout << *name << " " << name << " " << &name << endl;
		if (this->name != NULL)
		{
			age = -1;
			delete[] name;
			name = NULL;
		}
		age = obj.age;
		name = new char[strlen(obj.name) + 1];
		//����
		strcpy(name, obj.name);
		return *this;
	}
private:
	char *name;
	int age;

};
ostream& operator<<(ostream& out, const Teacher &obj)
{
	out << obj.name << " " << obj.age << endl;
	return out;
}
void main()
{
	Array<Teacher*> array1(3);
	Teacher t1("Riven", 90);
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

	array1[0] = &t1;
	array1[1] = &t2;
	array1[2] = &t3;
	for (int i = 0; i < 3; i++)
	{
		cout <<array1[i]<<"=" <<*(array1[i])<<endl;
	}
	system("pause");
}
*/
