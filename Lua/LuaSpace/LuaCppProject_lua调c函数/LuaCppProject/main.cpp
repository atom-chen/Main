#include "main.h"


int main(int argc, char** argv)
{
	//1 ��ʼ��
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������
	SetCFunction(lua, TestCFunction, "CFunc");
	SetCFunction(lua, TestCFunction_Para, "CFunc2");
	SetCFunction(lua, TestCFunction_Return, "CFunc3");
	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

