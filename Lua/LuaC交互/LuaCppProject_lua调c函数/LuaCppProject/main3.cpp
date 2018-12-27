#include "Tools.h"
#include "lauxlib.h"
#include "lua.h"

//C函数：有返回无参
int TestCFunction_Return(lua_State *lua)
{
	printf("Hello I am C Fcuntion_Return\n");
	lua_pushstring(lua, "Hello,I came from c");//入栈

	return 1;//返回1个参数给lua

	//现在从栈顶数两个参数，是要return 给lua的
}

int main(int argc, char** argv)
{
	lua_State *lua = luaL_newstate(); 
	luaL_openlibs(lua);

	SetCFunction(lua, TestCFunction_Return, "CFunc3");

	if (luaL_dofile(lua, "main3.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

