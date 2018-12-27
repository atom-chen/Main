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

//在Lua层调用eat必备函数
int Cat::API_Eat(lua_State* L)
{
	Cat* cat = static_cast<Cat *>(lua_touserdata(L, 1));
	if (cat != nullptr)
	{
		cat->Eat();
	}
	return 0;
}

//Lua层创建中Cat的构造方法
int Cat::API_NewCat(lua_State* L)
{
	void* pMemory = lua_newuserdata(L, sizeof(Cat));   //分配内存
	if (pMemory == nullptr)
	{
		return 0;
	}
	new(pMemory)Cat;      //不会开辟内存，只会在我们预开辟的内存里调用构造函数
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
