#include "main.h"




//1、lua堆栈可以双向访问
//2、传给lua的函数，返回值为传给lua的参数个数
int main(int argc, char** argv)
{
	//1 初始化
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库
	WriteGlobalDataToLua(lua);
	SetCFunction(lua, TestCFunction,"CFunc");
	SetCFunction(lua, TestCFunction_Para,"CFunc2");
	SetCFunction(lua, TestCFunction_Return, "CFunc3");
	if (luaL_dofile(lua, "main.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}
	ReadGlobalDataFromLua(lua);
	lua_close(lua);
	return 0;
}

