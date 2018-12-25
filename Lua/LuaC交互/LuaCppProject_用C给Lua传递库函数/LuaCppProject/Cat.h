#pragma once
#include "main.h"

class Cat
{
public:
	Cat();
	void Eat();

public:
	static int API_Eat(lua_State* L);                   //Lua中Cat的成员函数
	static int API_NewCat(lua_State* L);             	//Lua中Cat的构造方法
	static int Export(lua_State* L);                   //导出猫类
public:
	char mName[256];
};
