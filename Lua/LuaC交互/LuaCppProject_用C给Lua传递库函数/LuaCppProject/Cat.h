#pragma once
#include "Tools.h"
#define CATTABLE "Cat_Meta_Table"
class Cat
{

public:
	Cat();
	void Eat();

	//--------------Lua层，设置用于导出的API
public:
	static int API_Eat(lua_State* L);                   //Lua中Cat的成员函数
	static int API_NewCat(lua_State* L);             	//Lua中Cat的构造方法
	static int Export(lua_State* L);                   //导出猫类
public:
	char mName[256];
};
