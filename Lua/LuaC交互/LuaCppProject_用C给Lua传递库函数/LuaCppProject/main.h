#pragma once
#include <iostream>
#define CATTABLE "Cat_Meta_Table"

//ÒýÈëc³ÌÐò
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua), const char* packageName, const char* funcName);
int TestCFunction_Export(lua_State *lua);



