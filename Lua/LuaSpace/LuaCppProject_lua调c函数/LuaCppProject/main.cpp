#include "main.h"
#include "lauxlib.h"
#include "lua.h"


int main(int argc, char** argv)
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������
	lua_pushcfunction(lua, Cat::Export);
	lua_pcall(lua, 0, 0, 0);

	SetCFunction(lua, TestCFunction, "CFunc");
	SetCFunction(lua, TestCFunction_Para, "CFunc2");
	SetCFunction(lua, TestCFunction_Return, "CFunc3");
	SetCFunction(lua, TestCFunction_Export, "Alice","RunTimePlatform");
	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

