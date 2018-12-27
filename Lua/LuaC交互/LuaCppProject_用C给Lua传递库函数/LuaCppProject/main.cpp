#include "Tools.h"
#include "lauxlib.h"
#include "lua.h"

int TestCFunction_Export(lua_State *lua)
{
#if defined(_WIN32)
	lua_pushstring(lua, "Windows");             //往lua堆栈写入数据
	return 1;
#else if defined(_UNIX)
	lua_pushstring(lua, "Unix");             //往lua堆栈写入数据
	return 2;
#endif
}

int main1(int argc, char** argv)
{
	//1 初始化
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库

	SetCFunction(lua, TestCFunction_Export, "Alice","RunTimePlatform");

	if (luaL_dofile(lua, "main.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

