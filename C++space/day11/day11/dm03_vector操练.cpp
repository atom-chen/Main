
#include <iostream>
using namespace std;
#include "vector"

//����Ԫ�ص� ��Ӻ�ɾ��
void main31()
{
	vector<int> v1;

	cout << "length:" << v1.size() <<endl;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	cout << "length:" << v1.size() <<endl;

	cout << "ͷ��Ԫ��" << v1.front() << endl;


	//�޸� ͷ��Ԫ��
	//��������ֵ����ֵ Ӧ�÷���һ������
	v1.front() = 11;
	v1.back() = 55;

	while (v1.size() > 0)
	{
		cout <<"β��Ԫ��" << v1.back() ; //��ȡβ��Ԫ��
		v1.pop_back(); //ɾ��β��Ԫ��
	}
}

//vector�ĳ�ʼ��
void main32()
{
	vector<int> v1;
	v1.push_back(1);
	v1.push_back(3);
	v1.push_back(5);
	v1.push_back(7);

	vector<int> v2 = v1;  //�����ʼ��

	vector<int> v3(v1.begin(), v1.begin()+2 );
}

void printV(vector<int> &v)
{
	for (int i=0; i<v.size(); i++)
	{
		cout << v[i] << " ";
	}
}

//vector�ı��� ͨ������ķ�ʽ 
void main33()
{
	vector<int> v1(10);   //��ǰ���ڴ�׼����

	for (int i=0; i<10; i++)
	{
		v1[i] = i + 1;
	}

// 	for (int i=0; i<10; i++)
// 	{
// 		printf("%d ", v1[i]);
// 	}

	printV(v1);

}

//push_back��ǿ������
void main34() 
{
	vector<int> v1(10);   //��ǰ���ڴ�׼����
	v1.push_back(100);
	v1.push_back(200);
	cout << "size: " << v1.size() << endl;
	printV(v1);
}


//1������ end()����� 
//   1	3	5
//	 ��	
//	          ��
//�� it == v1.end()��ʱ�� ˵����������Ѿ����������...
//end()��λ�� Ӧ���� 5�ĺ���

//2 ������������
/*
typedef iterator pointer;
typedef const_iterator const_pointer;
typedef _STD reverse_iterator<iterator> reverse_iterator;
typedef _STD reverse_iterator<const_iterator> const_reverse_iterator;
*/
void main35()
{
	vector<int> v1(10);   
	for (int i=0; i<10; i++)
	{
		v1[i] = i + 1;
	}

	//�������
	for (vector<int>::iterator it = v1.begin(); it != v1.end(); it ++ )
	{
		cout << *it << " ";
	}

	//�������
	for (vector<int>::reverse_iterator rit = v1.rbegin(); rit!=v1.rend(); rit++ )
	{
		cout << *rit << " ";
	}

}

//vector  ɾ��
void main36()
{
	vector<int> v1(10);   
	for (int i=0; i<10; i++)
	{
		v1[i] = i + 1;
	}

	//����ɾ��
	v1.erase(v1.begin(), v1.begin()+3);
	printV(v1);

	//����Ԫ�ص�λ�� ָ��λ��ɾ��
	v1.erase(v1.begin()); //��ͷ��ɾ��һ��Ԫ��
	printV(v1);

	cout << endl;

	//����Ԫ�ص�ֵ 
	v1[1] = 2;
	v1[3] = 2;
	printV(v1);

	for (vector<int>::iterator it =v1.begin(); it != v1.end();)
	{
		if (*it == 2)
		{
			it = v1.erase(it);  //�� ɾ����������ָ���Ԫ�ص�ʱ��,eraseɾ����������it�Զ����ƶ�
		}
		else
		{
			it ++;
		}
	}
	printV(v1);

	cout << endl;
	v1.insert(v1.begin(), 100);
	v1.insert(v1.end(), 200);
	printV(v1);

}
void main333()
{
	//main31();
	//main32();
	//main33();
	//main34();
	//main35();
	main36();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}