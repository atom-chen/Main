#include "main.h"
void ReadGlobalDataFromLua(lua_State* lua)
{
	//--------------begin read global data from lua stack------------------------
	lua_getglobal(lua, "c");//��ʱc��Ϊջ��Ԫ��
	printf("c= %f\n", lua_tonumber(lua, -1));//��ջ��(index=-1)����ת��Ϊnumber

	lua_getglobal(lua, "d");
	printf("d= %s\n", lua_tostring(lua, -1));

	lua_getglobal(lua, "a");//a��Ϊջ��Ԫ��
	printf("a= %d\n", lua_tointeger(lua, -1));

	lua_getglobal(lua, "b");//b��Ϊջ��Ԫ��
	printf("b= %d\n", lua_toboolean(lua, -1));

	printf("stack size= %d\n", lua_gettop(lua));//��ӡlua�Ķ�ջ��С
}
void WriteGlobalDataToLua(lua_State* lua)
{
	printf("stack size= %d\n", lua_gettop(lua));//��ӡlua�Ķ�ջ��С
	//---------------------------begin write global data to lua srack-------------------------
	lua_pushinteger(lua, 1);//��lua��ջд��1
	lua_setglobal(lua, "a");//����lua��ջջ��Ԫ�أ���ֵ������a

	lua_pushboolean(lua, false);
	lua_setglobal(lua, "b");

	lua_pushnumber(lua, 1.5f);
	lua_setglobal(lua, "c");

	lua_pushstring(lua, "hello");
	lua_setglobal(lua, "d");

	lua_newtable(lua);//����һ����
	lua_setglobal(lua, "e");
}