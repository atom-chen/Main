#include "main.h"


void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua),const char* funcName)
{
	lua_pushcfunction(lua, CFunc);
	lua_setglobal(lua, funcName);
}

