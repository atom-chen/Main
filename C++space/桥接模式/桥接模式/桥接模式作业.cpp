#include<iostream>
#include <string>
using namespace std;

class Color{
public:
	virtual void getColor()
	{

	}
};

class AbShape{
public:	
	virtual string draw() = 0;
};
class yuan :public AbShape{
public:	
	virtual string draw()
	{
		return "Ô²ÐÎ";
	}
};
class ju :public AbShape
{
public:
	virtual string draw()
	{
		return "¾ØÐÎ";
	}
};


void main()
{

}