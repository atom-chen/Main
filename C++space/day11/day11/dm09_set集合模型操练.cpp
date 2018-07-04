#define  _CRT_SECURE_NO_WARNINGS
#include <iostream>
using namespace std;
#include "set"

//1 集合 元素唯一 自动排序(默认情况下 是从小到大) 不能按照[]方式插入元素 
// 红黑树  

//set元素的添加/遍历/删除基本操作
void main91()
{
	set<int>  set1;
	
	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set1.insert(tmp);
	}
	//插入元素 重复的
	set1.insert(100);
	set1.insert(100);
	set1.insert(100);
	
	for (set<int>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << *it << " ";
	}

	//删除集合 
	while ( !set1.empty())
	{
		set<int>::iterator it = set1.begin();
		cout << *it << " ";
		set1.erase(set1.begin());
	}
}

//2 对基本的数据类型 set能自动的排序 
void main92()
{
	set<int> set1;  
	set<int, less<int>> set2;   //默认情况下是这样 

	set<int, greater<int>> set3;  //从大 到 小

	for (int i=0; i<5; i++)
	{
		int tmp = rand();
		set3.insert(tmp);
	}

	//从大 到 小
	for (set<int, greater<int>>::iterator it = set3.begin(); it != set3.end(); it++  )
	{
		cout << *it << endl;
	}
}


class Student
{
public:
	Student(char *name, int age)
	{
		strcpy(this->name, name);
		this->age = age;
	}
public:
	char name[64];
	int		age;
};

//仿函数 
struct FuncStudent
{
	bool operator()(const Student &left, const Student &right)
	{
		if (left.age < right.age)  //如果左边的小 就返回真 从小到大按照年龄进行排序
		{
			return true;
		}
		else
		{
			return false; 
		}
	}
};

//3 自定义数据类型的排序 仿函数的用法
void main93()
{
	Student s1("s1", 31);
	Student s2("s2", 22);
	Student s3("s3", 44);
	Student s4("s4", 11);
	Student s5("s5", 31);

	set<Student, FuncStudent> set1;
	set1.insert(s1);
	set1.insert(s2);
	set1.insert(s3);
	set1.insert(s4);
	set1.insert(s5); //如果两个31岁 能插入成功  
	//如何知道 插入 的结果

	//遍历
	for (set<Student, FuncStudent>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << it->age << "\t" <<  it->name << endl;
	}
}

//typedef pair<iterator, bool> _Pairib;
//4 如何判断 set1.insert函数的返回值
//Pair的用法 
void main94()
{
	Student s1("s1", 31);
	Student s2("s2", 22);
	Student s3("s3", 44);
	Student s4("s4", 11);
	Student s5("s5", 31);

	set<Student, FuncStudent> set1;
	pair<set<Student, FuncStudent>::iterator, bool> pair1 = set1.insert(s1);
	if (pair1.second == true)
	{
		cout << "插入s1成功" << endl;
	}
	else
	{
		cout << "插入s1失败" << endl;
	}

	set1.insert(s2);

	//如何知道 插入 的结果
	pair<set<Student, FuncStudent>::iterator, bool> pair5 = set1.insert(s5); //如果两个31岁 能插入成功  
	if (pair5.second == true)
	{
		cout << "插入s1成功" << endl;
	}
	else
	{
		cout << "插入s1失败" << endl;
	}

	//遍历
	for (set<Student, FuncStudent>::iterator it=set1.begin(); it!=set1.end(); it++ )
	{
		cout << it->age << "\t" <<  it->name << endl;
	}
}


//find查找  equal_range 
//返回结果是一个pair
void main95()
{
	set<int> set1;  

	for (int i=0; i<10; i++)
	{
		set1.insert(i+1);
	}

	//从大 到 小
	for (set<int>::iterator it = set1.begin(); it != set1.end(); it++  )
	{
		cout << *it << " ";
	}
	cout << endl;

	set<int>::iterator it0 =  set1.find(5);
	cout << "it0:" << *it0 << endl;

	int num1 = set1.count(5);
	cout << "num1:" << num1 << endl;

	set<int>::iterator it1 =   set1.lower_bound(5); // 大于等于5的元素 的 迭代器的位置
	cout << "it1:" << *it1 << endl;
	
	set<int>::iterator it2 =   set1.lower_bound(5); // 大于5的元素 的 迭代器的位置
	cout << "it2:" << *it2 << endl;

	//
	//typedef pair<iterator, bool> _Pairib;
	//typedef pair<iterator, iterator> _Pairii;
	//typedef pair<const_iterator, const_iterator> _Paircc;
	//把5元素删除掉
	set1.erase(5); 
	pair<set<int>::iterator, set<int>::iterator>  mypair = set1.equal_range(5);
	set<int>::iterator it3 = mypair.first;
	cout << "it3:" << *it3 << endl;  //5  //6

	set<int>::iterator it4 =  mypair.second; 
	cout << "it4:" << *it4 << endl;  //6  //6

}

void main999()
{
	//main91();
	//main92();
	//main93();
	//main94();
	//main95();
	cout<<"hello..."<<endl;
	system("pause");
	return ;
}