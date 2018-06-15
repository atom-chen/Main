#pragma once
#include <iostream>

//ÒýÈëc³ÌÐò
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

void ReadGlobalDataFromLua(lua_State *lua);
void WriteGlobalDataToLua(lua_State *lua);
void WriteArrayTable(lua_State* lua);
void ReadArrayTable(lua_State* lua);
void WriteKVTable(lua_State* lua);
void ReadKVTable(lua_State* lua);

int TestFunction(lua_State *lua);
int TestCFunction_Para(lua_State *lua);
int TestCFunction_Return(lua_State *lua);

