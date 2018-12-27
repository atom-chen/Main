#include "Tools.h"


int main(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  
	luaL_openlibs(lua);
	if (luaL_dofile(lua, "main02.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}


	lua_getglobal(lua, "LuaFunc2");//���ĺ�������ջͷ
	lua_pushstring(lua, "Hello");              //1
	lua_pushinteger(lua, 1);                   //2
	lua_pushnumber(lua, 1.5);                  //3

	lua_pcall(lua, 3, 0, NULL);     //����lua����: ��������������ֵ��������������

	//��ӡ��־
	if (lua_pcall(lua, 0, 2, -2))
	{
		const char* erroeInfo = lua_tostring(lua, -1);
		printf("%s\n", erroeInfo);
	}
	lua_close(lua);
	return 0;
}