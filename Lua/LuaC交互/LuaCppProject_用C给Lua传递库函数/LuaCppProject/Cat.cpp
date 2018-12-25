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

int Cat::API_Eat(lua_State* L)
{
	Cat* cat = (Cat*)lua_touserdata(L, 1);
	cat->Eat();
	return 0;
}

//Lua中Cat的构造方法
int Cat::API_NewCat(lua_State* L)
{
	void* pMemory = lua_newuserdata(L, sizeof(Cat));
	new(pMemory)Cat;
	luaL_getmetatable(L, CATTABLE);
	lua_setmetatable(L, -2);
	return 1;
}

//导出猫类
int Cat::Export(lua_State* L)
{
	SetCFunction(L, API_NewCat, "Cat", "New");
	luaL_newmetatable(L, CATTABLE);
	lua_pushvalue(L, -1);
	lua_setfield(L, -2, "__index");
	SetCFunction(L, API_Eat, NULL, "Eat");                 //将API_EAT映射到Lua的成员函数Eat
	return 0;
}
