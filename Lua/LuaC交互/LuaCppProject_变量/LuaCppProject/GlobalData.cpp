#include "main.h"

//读lua栈顶的数据
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

//往lua栈顶写数据
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

//往lua写入数组类型table
void WriteArrayTable(lua_State* lua)
{
	//数组类型表
	lua_newtable(lua);
	lua_pushinteger(lua, 10);
	lua_rawseti(lua, -2, 1);//将1位置赋值为10

	lua_pushinteger(lua, 11);
	lua_rawseti(lua, -2, 2);//2位置赋值11
	lua_setglobal(lua, "t");
}

//读lua栈顶的数组类型table
void ReadArrayTable(lua_State* lua)
{
	lua_getglobal(lua, "t");
	lua_rawgeti(lua, -1, 1);
	printf("C Said: %d\n", lua_tointeger(lua, -1));
	lua_rawgeti(lua, -2, 2);
	printf("C Said: %d\n", lua_tointeger(lua, -1));
}

//往kv类型表写数据
void WriteKVTable(lua_State* lua)
{
	//kv类型表
	lua_newtable(lua);
	//赋值法1
	lua_pushstring(lua, "x");
	lua_pushinteger(lua, 0);
	lua_settable(lua, -3);//key=x value=0，位置在-3

	//赋值法2
	lua_pushinteger(lua, 11);
	lua_setfield(lua, -2, "y");//key=y value=11，位置在-2

	lua_setglobal(lua, "t2");
}

//读kv类型的table
void ReadKVTable(lua_State* lua)
{
	lua_getglobal(lua, "t2");
	//获取法1
	lua_getfield(lua, -1, "x");//把key为x的value防到栈顶
	printf("c said: x is %d\n", lua_tointeger(lua,-1));

	//获取法2
	lua_pushstring(lua, "y");
	lua_gettable(lua, -3);
	printf("c said: y is %d\n", lua_tointeger(lua, -1));
}