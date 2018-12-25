#include "Cat.h"


int main(int argc, char** argv)
{
	//1 初始化
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库

	lua_pushcfunction(lua, Cat::Export);
	lua_pcall(lua, 0, 0, 0);

	if (luaL_dofile(lua, "main2.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}