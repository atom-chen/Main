#pragma once
#include <iostream>

//����c����
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}



int TestCFunction(lua_State *lua);
int TestCFunction_Para(lua_State *lua);
int TestCFunction_Return(lua_State *lua);

/*int Test(int argc, char** argv)
{
	lua_State *lua = luaL_newstate();  //����lua�����
	luaL_openlibs(lua);//���������
	if (luaL_dofile(lua, "main.lua") != 0) //����Ҫִ�е��ļ�s
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_getglobal(lua, "LuaFunc");//��luaFunc����ջͷ
	lua_pcall(lua, 0, 0, NULL);//lua,��������������ֵ��������������

	lua_getglobal(lua, "LuaFunc2");//��luaFunc����ջͷ
	lua_pushstring(lua, "Hello");//1
	lua_pushinteger(lua, 1);//2
	lua_pushnumber(lua, 1.5);//3
	lua_pcall(lua, 3, 0, NULL);//lua,��������������ֵ��������������

	lua_getglobal(lua, "LuaFunc3");
	lua_pcall(lua, 0, 2, NULL);
	//ȡջ���ķ���ֵ
	int ret1 = lua_tointeger(lua, 1);
	float ret2 = lua_tonumber(lua, 2);
	printf("ret=%u,%f\n", ret1, ret2);


	lua_getglobal(lua, "debug");//��lua�Դ����������ᵽջ��
	lua_getfield(lua, -1, "traceback");
	lua_getglobal(lua, "LuaFunc4");
	//��ӡ��־
	if (lua_pcall(lua, 0, 2, -2))
	{
		const char* erroeInfo = lua_tostring(lua, -1);
		printf("%s\n", erroeInfo);
	}



	lua_close(lua);
	return 0;
}
*/