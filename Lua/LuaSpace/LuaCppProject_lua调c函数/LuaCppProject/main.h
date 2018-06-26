#pragma once
#include <iostream>
#define CATTABLE "Cat_Meta_Table"

//引入c程序
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

void ReadGlobalDataFromLua(lua_State *lua);
void WriteGlobalDataToLua(lua_State *lua);
void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua), const char* funcName);
void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua), const char* packageName, const char* funcName);

void CallLuaFunction(const char* funcName);

int TestCFunction(lua_State *lua);
int TestCFunction_Para(lua_State *lua);
int TestCFunction_Return(lua_State *lua);
int TestCFunction_Export(lua_State *lua);

class Cat
{
public:
	Cat()
	{
		memset(mName, 0, sizeof(mName));
		strcpy(mName, "Baibai");
	}
	void Eat()
	{
		printf("%s Eat\n", mName);
	}
	static int API_Eat(lua_State* L)
	{
		Cat* cat = (Cat*)lua_touserdata(L, 1);
		cat->Eat();
		return 0;
	}
	//Lua中Cat的构造方法
	static int API_NewCat(lua_State* L)
	{
		void* pMemory = lua_newuserdata(L, sizeof(Cat));
		new(pMemory)Cat;
		luaL_getmetatable(L, CATTABLE);
		lua_setmetatable(L, -2);
		return 1;
	}
	//导出猫类
	static int Export(lua_State* L)
	{
		SetCFunction(L, API_NewCat, "Cat", "New");
		luaL_newmetatable(L, CATTABLE);
		lua_pushvalue(L, -1);
		lua_setfield(L, -2, "__index");
		SetCFunction(L, API_Eat, NULL, "Eat");
		return 0;
	}
protected:
private:
public:
	char mName[256];
};

