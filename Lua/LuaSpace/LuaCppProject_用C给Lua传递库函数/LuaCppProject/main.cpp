#include "main.h"
#include "lauxlib.h"
#include "lua.h"

int TestCFunction_Export(lua_State *lua)
{
	lua_pushstring(lua, "Lua is beautiful");             //��lua��ջд������
	return 1;
}

int main1(int argc, char** argv)
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������

	SetCFunction(lua, TestCFunction_Export, "Alice","RunTimePlatform");

	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

