
#include <iostream>
using namespace std;

#include <queue>


//队列中基本数据类型
void main61()
{
	queue<int>  q;
	q.push(1);
	q.push(2);
	q.push(3);

	cout << "队头元素:" << q.front() << endl;
	cout << "队列的大小" << q.size() <<endl;
	while ( !q.empty())
	{
		int tmp = q.front();
		cout << tmp << " ";
		q.pop();
	}
}

//队列的算法 和 数据类型的分离

//teacher结点
class Teacher
{
public:
	int		age;
	char	name[32];
public:
	void printT()
	{
		cout << "age:" << age << endl;
	}
};

void main62()
{
	Teacher t1, t2, t3;
	t1.age = 31;
	t2.age = 32;
	t3.age = 33;
	queue<Teacher> q;
	q.push(t1);
	q.push(t2);
	q.push(t3);

	while (!q.empty())
	{
		Teacher tmp = q.front();
		tmp.printT();
		q.pop();
	}
}
void main63()
{
	Teacher t1, t2, t3;
	t1.age = 31;
	t2.age = 32;
	t3.age = 33;
	queue<Teacher *> q;
	q.push(&t1);
	q.push(&t2);
	q.push(&t3);

	while (!q.empty())
	{
		Teacher *tmp = q.front();
		tmp->printT();
		q.pop();
	}
}

void main666()
{
	//main61();
	//main62();
	main63();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}