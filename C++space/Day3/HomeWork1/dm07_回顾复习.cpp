
#include <iostream>
using namespace std;

void objplaymain71();

class Test
{
public:
	Test()
	{
		a = 0; 
		b = 0;
		cout << "�޲������캯��  �Զ�������" <<endl;
	}
	Test(int _a) //�в������캯��
	{
		a = _a;
		b = 0;
	}

	Test(const Test& obj) //copy���캯�� ����: ��һ�������ʼ������һ������
	{
		a = obj.a + 100;
		b = obj.b + 100;
	}
	void printT()
	{
		cout << "a:" << a << "b: "<<b<< endl; 
	}

	~Test()
	{
		cout<<"������������ �����������ڽ���ʱ,�ᱻc++�������Զ�����" <<endl;
 	}

protected:
private:
	int a;
	int b;
};

// ��3�ֵ���ʱ��
void printTest(Test t)
{
	;
}

// 1 �� 2 
void objplaymain72()
{
	//
	Test t1(1); //ok
	Test t2(t1); 
	Test t3 = t1; //�����copy���캯��
	printTest(t3);
}


//copy���캯���ĵ�4�ֵ���ʱ��
//����һ��Ԫ��  ��������
Test getTestObj()
{
	Test t(1);
	return t;
}

void TestNoNameObj()
{
	Test  myt1 =getTestObj(); //�����������ʼ�� ����һ������  
	Test  myt2(1);
	myt2 = getTestObj(); //���������� �� ����һ������ ��ֵ ������������

}

void main701()
{
	//objplaymain();
	objplaymain72();
	TestNoNameObj();
	cout<<"hello..."<<endl;
	system("pause");
}

void objplaymain71()
{
	Test t1; //ok
	//Test t2() ; //�����޲������캯���� ���󷽷�
	//t2.printT();

	//

	Test  t3(1);	//c++�������Զ��ĵ��ù��캯��
	Test t4 = 4;	//c++�������Զ��ĵ��ù��캯��
	Test t5 = Test(5); //����Ա�ֹ��ĵ��ù��캯��

	//
	Test t6 = t1;
	return ;
}
