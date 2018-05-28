#include "main.h"

int TestCFunction(lua_State *lua)
{
	printf("Hello I am C Fcuntion\n");
	return 0;
}

int TestCFunction_Para(lua_State *lua)
{
	printf("Hello I am C Fcuntion_Para\n");
	const char* arg1 = lua_tostring(lua,1);//获取索引为1的数据
	printf("arg1=%s \n", arg1);
	int arg2 = lua_tointeger(lua, 2);//获取索引为2的数据
	printf("arg2=%d \n", arg2);
	int arg2_1 = lua_tointeger(lua, -1); //逆向获取
	const char* arg1_1 = lua_tostring(lua, -2);
	return 0;

}
int TestCFunction_Return(lua_State *lua)
{
	printf("Hello I am C Fcuntion_Return\n");
	lua_pushstring(lua, "Hello,I came from c");//入栈

	return 1;//返回2个参数给lua
	//现在从栈顶数两个参数，是要return 给lua的
}

void SetCFunction(lua_State *lua, int(*CFunc)(lua_State *lua),const char* funcName)
{
	lua_pushcfunction(lua, CFunc);
	lua_setglobal(lua, funcName);
}

