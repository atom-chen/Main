#include "main.h"


int lua_foo = LUA_REFNIL;
int TestFunc(lua_State *lua)
{
	lua_foo = luaL_ref(lua, LUA_REGISTRYINDEX);
	return 0;
}



int main(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库
	if (luaL_dofile(lua, "main.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_getglobal(lua, "LuaFunc");//将luaFunc弹到栈头
	lua_pcall(lua, 0, 0, NULL);//lua,参数个数，返回值个数，错误处理函数

	lua_getglobal(lua, "LuaFunc2");//将luaFunc弹到栈头
	lua_pushstring(lua, "Hello");//1
	lua_pushinteger(lua, 1);//2
	lua_pushnumber(lua, 1.5);//3
	lua_pcall(lua, 3, 0, NULL);//lua,参数个数，返回值个数，错误处理函数

	lua_getglobal(lua, "LuaFunc3");
	lua_pcall(lua, 0, 2, NULL);
	//取栈顶的返回值
	int ret1 = lua_tointeger(lua, 1);
	float ret2 = lua_tonumber(lua, 2);
	printf("ret=%u,%f\n", ret1, ret2);

	
	lua_getglobal(lua, "debug");//将lua自带错误处理函数提到栈顶
	lua_getfield(lua, -1, "traceback");
	lua_getglobal(lua, "LuaFunc4");
	//打印日志
	if (lua_pcall(lua, 0, 2, -2))
	{
		const char* erroeInfo = lua_tostring(lua, -1);
		printf("%s\n", erroeInfo);
	}



	lua_close(lua);
	return 0;
}



int main2(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库
	lua_pushcfunction(lua, TestFunc);
	lua_setglobal(lua, "Test");
	if (luaL_dofile(lua, "main3.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}