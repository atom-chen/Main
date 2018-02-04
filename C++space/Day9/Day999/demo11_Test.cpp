#include<iostream>
using namespace std;
#include "MyVector.hpp"

class Teacher
{
public:
	Teacher(int age = 33)
	{
		this->age = age;
	}
	void print()
	{
		cout <<age<<endl;
	}
protected:
private:
	int age;
};
void main55()
{
	Teacher t1(31);
	Teacher t2(32);
	Teacher t3(33);
	MyVector<Teacher> vector1;
	vector1[0] = t1;
	vector1[1] = t2;
	vector1[2] = t3;
	t1.print();
	t2.print();
	t3.print();
	system("pause");
}
void main02()
{
	MyVector<char> vector1(26);
	vector1[0] = 97;
	cout << vector1[0] << endl;
	vector1[1] = 98;
	cout << vector1[1] << endl;

}
void main01()
{
	MyVector<int> vector1(10);
	for (int i = 0; i < vector1.size(); i++)
	{
		vector1[i] = i + 1;
		cout << vector1[i] << endl;
	}
	MyVector<int> vector2(10);
	vector2 = vector1;
	cout << &vector2 << endl;
	cout << &vector1 << endl;
	for (int i = 0; i < vector2.size(); i++)
	{
		vector2[i] = i + 1;
		cout << vector2[i] << endl;
	}

	cout << vector2 << endl;

	cout << endl;
	system("pause");
}