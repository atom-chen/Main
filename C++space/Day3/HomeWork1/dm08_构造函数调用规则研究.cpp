
#include <iostream>
using namespace std;

class Test
{
public:
	
// 	Test(const Test& obj) //copy���캯�� ����: ��һ�������ʼ������һ������
// 	{
// 		a = obj.a + 100;
// 		b = obj.b + 100;
// 	}

// 	Test(int _a, int _b)
// 	{
// 		;
// 	}
	Test()
	{

	}
	
	void printT()
	{
		cout << "a:" << a << "b: "<<b<< endl; 
	}

protected:
private:
	int a;
	int b;
};

//�����ж����˿������캯��ʱ��c++�����������ṩ�޲������캯��
//�����ж������в������캯����,c++�����������ṩ�޲������캯��

//�ڶ�����ʱ, ֻҪ��д�˹��캯��,�����Ҫ��

void main81()
{
	//Test t1; //�����޲ι��캯��
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}