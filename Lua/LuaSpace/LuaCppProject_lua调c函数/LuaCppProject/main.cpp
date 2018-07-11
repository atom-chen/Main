#include "main.h"
#include "lauxlib.h"
#include "lua.h"
//C函数：无参无返回
int TestCFunction(lua_State *lua)
{
	printf("Hello I am C Fcuntion\n");
	return 0;
}

int main1(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  
	luaL_openlibs(lua);

	SetCFunction(lua, TestCFunction, "CFunc");

	if (luaL_dofile(lua, "main.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

