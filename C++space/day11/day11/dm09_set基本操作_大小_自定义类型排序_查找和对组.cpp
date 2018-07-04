#define  _CRT_SECURE_NO_WARNINGS
#include <iostream>
using namespace std;
#include "set"

//1 集合 有序(默认从小到大) 唯一 (红黑二叉树 这种数据结构的变体) 
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

	//打印输出
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	//删除集合
	cout << "\n删除集合";
	while (!set1.empty())
	{
		set<int>::iterator it = set1.begin();
		printf("%d ", *it);
		set1.erase(set1.begin());
	}
}

//2 集合 从小到大 从大到小
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

	//打印输出
	for (set<int, greater<int>>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	//删除集合
	cout << "\n删除集合";
	while (!set1.empty())
	{
		set<int, greater<int>>::iterator it = set1.begin();
		printf("%d ", *it);
		set1.erase(set1.begin());
	}
}

//3 自定义数据类型 排序
//03 仿函数 函数对象 重载() 操作 进行比较大小
//题目：学生包含学号，姓名属性，现要求任意插入几个学生对象到set容器中，
//使得容器中的学生按学号的升序排序
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

//函数对象
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
	Student s1("张1", 32);

	set1.insert(s1);
	set1.insert(Student("张2", 32) );
	set1.insert(Student("张3", 53) );
	set1.insert(Student("张4", 34) );

	//打印输出
	for (set<Student, StuFunctor >::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< (*it).name << " ";
	}
	system("pause");
	return 0;
}


//04演示：set集合查找功能
int main94()
{
	int		i = 0;
	set<int> set1;

	for (i=1; i<10; i++)
	{
		set1.insert(i);
	}
	//打印输出
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++)
	{
		cout<< *it << " ";
	}
	cout << endl;

	set<int>::iterator it1 =  set1.lower_bound(5); //大于等于5迭代器
	set<int>::iterator it2 =  set1.upper_bound(5); //大于5的迭代器

	//通过迭代器进行元素的操作
	cout<<"it1 "<<*it1<<" "<<"it2 "<<*it2<<endl;

	//
	pair <set<int>::iterator, set<int>::iterator> pairIt = set1.equal_range(5);

	set<int>::iterator it3 = pairIt.first; //获取第一个
	set<int>::iterator it4 = pairIt.second; //获取第二个

	cout<<"it3 "<<*it3<<" "<<"it4 "<<*it4<<endl;
	system("pause");
	return 0;
}



void main()
{
	//main91();
	//main92();
	//main93();
	main94();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}