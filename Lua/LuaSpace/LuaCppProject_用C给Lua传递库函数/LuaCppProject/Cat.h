#pragma once
#include "main.h"

class Cat
{
public:
	Cat();
	void Eat();

public:
	static int API_Eat(lua_State* L);                   //Lua��Cat�ĳ�Ա����
	static int API_NewCat(lua_State* L);             	//Lua��Cat�Ĺ��췽��
	static int Export(lua_State* L);                   //����è��
public:
	char mName[256];
};
