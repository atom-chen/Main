#pragma once
#include <iostream>
using namespace std;

#include <string>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

//����c����
extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

namespace LUA
{
	class LuaEnviron
	{
	private:
		static lua_State *pLuaVm;
	public:
		static void Init();                                      //��ʼ��Lua�����
		static void Destroy();                                  //�ر�lua�����
	public:
		static void CallScript(std::string scriptName);          //����lua�ű�



	private:
		LuaEnviron& operator=(const LuaEnviron& other) = delete;
		LuaEnviron()=delete;
		LuaEnviron(const LuaEnviron& other) = delete;
	};
}
