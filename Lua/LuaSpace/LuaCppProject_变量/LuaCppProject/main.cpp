#include "main.h"



void main()
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������

	WriteArrayTable(lua);
	//������д��lua�����
	WriteGlobalDataToLua(lua);
	WriteKVTable(lua);
	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	ReadArrayTable(lua);
	ReadKVTable(lua);
	ReadGlobalDataFromLua(lua);
	lua_close(lua);
}


