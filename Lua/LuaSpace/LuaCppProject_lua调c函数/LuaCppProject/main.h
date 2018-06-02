#pragma once
#include <iostream>

//����c����
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

void ReadGlobalDataFromLua(lua_State *lua);
void WriteGlobalDataToLua(lua_State *lua);
void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua), const char* funcName);

void CallLuaFunction(const char* funcName);

int TestCFunction(lua_State *lua);
int TestCFunction_Para(lua_State *lua);
int TestCFunction_Return(lua_State *lua);

