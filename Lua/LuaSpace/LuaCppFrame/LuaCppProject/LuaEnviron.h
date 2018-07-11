#pragma once
#include <iostream>
using namespace std;

#include <string>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

//ÒýÈëc³ÌÐò
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
	public:
		static void Init();


	private:
		static lua_State *pLuaVm;
	private:
		LuaEnviron& operator=(const LuaEnviron& other) = delete;
		LuaEnviron()=delete;
		LuaEnviron(const LuaEnviron& other) = delete;
	};
}