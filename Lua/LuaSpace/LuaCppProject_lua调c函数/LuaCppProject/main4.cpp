#include "main.h"
#include "lauxlib.h"
#include "lua.h"


int main(int argc, char** argv)
{
	lua_State *lua = luaL_newstate(); 
	luaL_openlibs(lua);


	if (luaL_dofile(lua, "main3.lua") != 0) 
	{
		printf("lua error:%s", lua_tostring(lua, -1));
	}

	lua_close(lua);
	return 0;
}

