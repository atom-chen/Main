#include "main.h"
#include "lauxlib.h"
#include "lua.h"

//C�������з����޲�
int TestCFunction_Return(lua_State *lua)
{
	printf("Hello I am C Fcuntion_Return\n");
	lua_pushstring(lua, "Hello,I came from c");//��ջ

	return 1;//����1��������lua

	//���ڴ�ջ����������������Ҫreturn ��lua��
}

int main(int argc, char** argv)
{
	lua_State *lua = luaL_newstate(); 
	luaL_openlibs(lua);

	SetCFunction(lua, TestCFunction_Return, "CFunc3");

	if (luaL_dofile(lua, "main3.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

