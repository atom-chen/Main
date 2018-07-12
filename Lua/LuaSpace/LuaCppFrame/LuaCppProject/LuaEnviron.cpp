#include "LuaEnviron.h"
using namespace LUA;

//lua_State *pLuaVm;

lua_State LuaEnviron::*pLuaVm;
namespace LUA
{
	void LuaEnviron::Init()
	{
		if (pLuaVm == nullptr)
		{
			pLuaVm = luaL_newstate();
			luaL_openlibs(pLuaVm);
		}
	}

	void LuaEnviron::Destroy()
	{
		if (pLuaVm != nullptr)
		{
			lua_close(pLuaVm);
		}
	}

	void LuaEnviron::CallScript(std::string scriptName)
	{
		if (luaL_dofile(pLuaVm,("Scripts/"+scriptName).c_str()) != 0)          //传入要执行的文件s
		{
			printf("lua error:%s", lua_tostring(pLuaVm, -1));
		}
	}


}
