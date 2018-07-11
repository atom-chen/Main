#include "main.h"


int lua_foo = LUA_REFNIL;
int TestFunc(lua_State *lua)
{
	lua_foo = luaL_ref(lua, LUA_REGISTRYINDEX);
	return 0;
}
void main1()
{
	lua_State *lua = luaL_newstate(); 
	luaL_openlibs(lua);
	if (luaL_dofile(lua, "main.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_getglobal(lua, "LuaFunc");                      //将luaFunc弹到栈头
	lua_pcall(lua, 0, 0, NULL);                          //lua,参数个数，返回值个数，错误处理函数

	//打印日志
	if (lua_pcall(lua, 0, 2, -2))
	{
		const char* erroeInfo = lua_tostring(lua, -1);
		printf("%s\n", erroeInfo);
	}

	lua_close(lua);
}



