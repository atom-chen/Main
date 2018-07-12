#pragma once
#include <iostream>
using namespace std;

#include <string>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

//引入c程序
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
		static void Init();                                      //初始化Lua虚拟机
		static void Destroy();                                  //关闭lua虚拟机
	public:
		static void CallScript(std::string scriptName);          //调用lua脚本



	private:
		LuaEnviron& operator=(const LuaEnviron& other) = delete;
		LuaEnviron()=delete;
		LuaEnviron(const LuaEnviron& other) = delete;
	};
}
