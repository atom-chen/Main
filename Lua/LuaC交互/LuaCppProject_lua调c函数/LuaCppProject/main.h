#pragma once
#include <iostream>

//����c����
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua), const char* funcName);

