#include "main.h"



void main()
{
	//1 初始化
	lua_State *lua = luaL_newstate();  //创建lua虚拟机
	luaL_openlibs(lua);//引入基本库

	WriteArrayTable(lua);
	//把数据写进lua虚拟机
	WriteGlobalDataToLua(lua);
	WriteKVTable(lua);
	if (luaL_dofile(lua, "main.lua") != 0) //传入要执行的文件s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	ReadArrayTable(lua);
	ReadKVTable(lua);
	ReadGlobalDataFromLua(lua);

	lua_close(lua);
}


