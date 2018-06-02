#include "main.h"
void ReadGlobalDataFromLua(lua_State* lua)
{
	//--------------begin read global data from lua stack------------------------
	lua_getglobal(lua, "c");//此时c成为栈顶元素
	printf("c= %f\n", lua_tonumber(lua, -1));//将栈顶(index=-1)数据转换为number

	lua_getglobal(lua, "d");
	printf("d= %s\n", lua_tostring(lua, -1));

	lua_getglobal(lua, "a");//a成为栈顶元素
	printf("a= %d\n", lua_tointeger(lua, -1));

	lua_getglobal(lua, "b");//b成为栈顶元素
	printf("b= %d\n", lua_toboolean(lua, -1));

	printf("stack size= %d\n", lua_gettop(lua));//打印lua的堆栈大小
}
void WriteGlobalDataToLua(lua_State* lua)
{
	printf("stack size= %d\n", lua_gettop(lua));//打印lua的堆栈大小
	//---------------------------begin write global data to lua srack-------------------------
	lua_pushinteger(lua, 1);//往lua堆栈写入1
	lua_setglobal(lua, "a");//弹出lua堆栈栈顶元素，赋值给变量a

	lua_pushboolean(lua, false);
	lua_setglobal(lua, "b");

	lua_pushnumber(lua, 1.5f);
	lua_setglobal(lua, "c");

	lua_pushstring(lua, "hello");
	lua_setglobal(lua, "d");

	lua_newtable(lua);//创建一个表
	lua_setglobal(lua, "e");
}