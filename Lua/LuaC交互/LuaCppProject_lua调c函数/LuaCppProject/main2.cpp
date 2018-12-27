#include "Tools.h"
#include "lauxlib.h"
#include "lua.h"

//C函数：有参无返回
int TestCFunction_Para(lua_State *lua)
{
	printf("Hello I am C Fcuntion_Para\n");
	const char* arg1 = lua_tostring(lua, 1);               //C拿到lua传递的参数
	printf("arg1=%s \n", arg1);
	int arg2 = lua_tointeger(lua, 2);                     
	printf("arg2=%d \n", arg2);

	//int arg2_1 = lua_tointeger(lua, -1);
	//const char* arg1_1 = lua_tostring(lua, -2);
	return 0;
}

int main2(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  
	luaL_openlibs(lua);

	SetCFunction(lua, TestCFunction_Para, "CFunc2");

	if (luaL_dofile(lua, "main2.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

