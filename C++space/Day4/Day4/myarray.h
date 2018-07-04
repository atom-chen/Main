#pragma  once

#include <iostream>
using namespace std;

class Array
{
public:
	Array(int length);
	Array(const Array& obj);
	~Array();

public:
	void setData(int index, int valude);
	int getData(int index);
	int length();

private:
	int m_length;
	int *m_space;

public:

	//��������ֵ����ֵ����Ҫ����һ������
	//Ӧ�÷���һ������(Ԫ�ر���) ������һ��ֵ
	int& operator[](int i);

	//����=
	Array& operator=(Array &a1);

	//���� ==
	bool operator==(Array &a1);


	//���� !=
	bool operator!=(Array &a1);
};

//Ҫ���������²�����
// []  ==  !=  