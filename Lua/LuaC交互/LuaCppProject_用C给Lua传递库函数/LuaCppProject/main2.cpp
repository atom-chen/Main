#include "Cat.h"


int main(int argc, char** argv)
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������

	lua_pushcfunction(lua, Cat::Export);
	lua_pcall(lua, 0, 0, 0);

	if (luaL_dofile(lua, "main2.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}