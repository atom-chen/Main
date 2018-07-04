#include <iostream>
using namespace std;

template <typename T>
class MyVector
{

	friend ostream & operator<< <T>(ostream &out,  const MyVector &obj);
public:
	MyVector(int size = 0);  //���캯��
	MyVector(const MyVector &obj); // �������캯��
	~MyVector(); //��������

public:

	T& operator[] (int index);
	// a3 = a2 = a1;
	MyVector &operator=(const MyVector &obj);

	

public:
	int getLen()
	{
		return m_len;
	}

protected:
	T *m_space;
	int m_len;
};


