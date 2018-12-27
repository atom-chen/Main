#include "Cat.h"


Cat::Cat()
{
	memset(mName, 0, sizeof(mName));
	strcpy(mName, "Baibai");
}

void Cat::Eat()
{
	printf("%s Eat\n", mName);
}

//��Lua�����eat�ر�����
int Cat::API_Eat(lua_State* L)
{
	Cat* cat = static_cast<Cat *>(lua_touserdata(L, 1));
	if (cat != nullptr)
	{
		cat->Eat();
	}
	return 0;
}

//Lua�㴴����Cat�Ĺ��췽��
int Cat::API_NewCat(lua_State* L)
{
	void* pMemory = lua_newuserdata(L, sizeof(Cat));   //�����ڴ�
	if (pMemory == nullptr)
	{
		return 0;
	}
	new(pMemory)Cat;      //���Ὺ���ڴ棬ֻ��������Ԥ���ٵ��ڴ�����ù��캯��
	luaL_getmetatable(L, CATTABLE);
	lua_setmetatable(L, -2);
	return 1;
}

//����è��
int Cat::Export(lua_State* L)
{
	SetCFunction(L, API_NewCat, "Cat", "New");
	luaL_newmetatable(L, CATTABLE);
	lua_pushvalue(L, -1);
	lua_setfield(L, -2, "__index");
	SetCFunction(L, API_Eat, NULL, "Eat");                 //��API_EATӳ�䵽Lua�ĳ�Ա����Eat
	return 0;
}
