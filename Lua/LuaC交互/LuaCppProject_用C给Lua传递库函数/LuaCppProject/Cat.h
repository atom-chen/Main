#pragma once
#include "Tools.h"
#define CATTABLE "Cat_Meta_Table"
class Cat
{

public:
	Cat();
	void Eat();

	//--------------Lua�㣬�������ڵ�����API
public:
	static int API_Eat(lua_State* L);                   //Lua��Cat�ĳ�Ա����
	static int API_NewCat(lua_State* L);             	//Lua��Cat�Ĺ��췽��
	static int Export(lua_State* L);                   //����è��
public:
	char mName[256];
};
