#include <iostream>

//����c����
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

int main(int argc, char** argv)
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������

	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}
	lua_close(lua);
	return 0;
}

