#include<iostream>
#include <string>
using namespace std;

template<class T>
class linearList{
public:
	virtual bool isEmpty() = 0;;
	virtual int size() = 0;
	virtual int indexOf(T obj) = 0;
	virtual T get(int index) = 0;
	virtual void add(T &obj) = 0;
	virtual void add(T &obj, int index) = 0;
	virtual T remove() = 0;
	virtual T remove(int index) = 0;
	virtual T remove(T &obj) = 0;
private:
	int length;
	T *array;
	//最后一个元素的位置
	int nowIndex;
};
template<class T>
class ArrayList :public linearList < T > {
public:
	ArrayList(int length=10)
	{
		this->length = length;
		array = new T[length];
	}
	ArrayList(const ArrayList &obj)
	{
		//拷贝
		array = new T[obj.size()];
		length = obj.length;
		nowIndex = nowIndex;
		//拷贝值
		for (int i = 0; i <=nowIndex;i++)
		{
			array[i] = obj.array[i];
		}
	}
public:
	virtual bool isEmpty()
	{
		return nowIndex == 0;
	}
	virtual int size()
	{
		return nowIndex+1;
	}
	virtual int indexOf(T obj)
	{
		//遍历
		for (int i = 0; i < length; i++)
		{
			if (obj == array[i])
			{
				return i;
			}
		}
		return -1;
	}
	virtual T get(int index)
	{
		return array[index];
	}
	virtual void add(T &obj)
	{
		//满检测
		if (isOver())
		{
			expansion();
		}
		//插入到尾部
		array[++nowIndex] = obj;
	}
	virtual void add(T &obj, int index)
	{
		//满检测
		if (isOver())
		{
			expansion();
		}
		//腾位置
		for (int i = nowIndex; i >= index; i++)
		{
			array[i + 1] = array[i];
		}
		array[index] = obj;
	}
	virtual T remove()
	{
		T temp = array[nowIndex];
		array[nowIndex] = NULL;
		nowIndex--;
		return temp;
	}
	virtual T remove(int index)
	{
		if (index >nowIndex)
		{
			return NULL;
		}
		T temp = array[index];
		array[index] = NULL;
		for (int i = index; i <=nowIndex; i++)
		{
			array[index] = array[i+1];
		}
		nowIndex--;
	}
	virtual T remove(T &obj)
	{
		return NULL;
	}
private:
	bool isOver()
	{
		return nowIndex == length - 1;
	}
	void expansion()
	{
		//扩充
		T *temp = new T[length * 2];
		//拷贝数据
		for (int i = 0; i < length; i++)
		{
			temp[i] = array[i];
		}
		delete[] array;
		length *= 2;
		array = temp;
	}
};
void main()
{

}