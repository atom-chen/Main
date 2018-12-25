#include "main.h"


int main03(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������
	if (luaL_dofile(lua, "main03.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_getglobal(lua, "LuaFunc3");
	lua_pcall(lua, 0, 2, NULL);
	//ȡջ���ķ���ֵ
	int ret1 = lua_tointeger(lua, 1);
	float ret2 = lua_tonumber(lua, 2);
	printf("ret=%u,%f\n", ret1, ret2);

	//��ӡ��־
	if (lua_pcall(lua, 0, 2, -2))
	{
		const char* erroeInfo = lua_tostring(lua, -1);
		printf("%s\n", erroeInfo);
	}



	lua_close(lua);
	return 0;
}
