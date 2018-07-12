#include "LuaEnviron.h"
using namespace LUA;


int main()
{
	LuaEnviron::Init();
	LuaEnviron::CallScript("TestCall.lua");
	LuaEnviron::Destroy();
	return 0;
}
