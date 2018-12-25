#include "main.h"

//��luaջ��������
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

//��luaջ��д����
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

//��luaд����������table
void WriteArrayTable(lua_State* lua)
{
	//�������ͱ�
	lua_newtable(lua);
	lua_pushinteger(lua, 10);
	lua_rawseti(lua, -2, 1);//��1λ�ø�ֵΪ10

	lua_pushinteger(lua, 11);
	lua_rawseti(lua, -2, 2);//2λ�ø�ֵ11
	lua_setglobal(lua, "t");
}

//��luaջ������������table
void ReadArrayTable(lua_State* lua)
{
	lua_getglobal(lua, "t");
	lua_rawgeti(lua, -1, 1);
	printf("C Said: %d\n", lua_tointeger(lua, -1));
	lua_rawgeti(lua, -2, 2);
	printf("C Said: %d\n", lua_tointeger(lua, -1));
}

//��kv���ͱ�д����
void WriteKVTable(lua_State* lua)
{
	//kv���ͱ�
	lua_newtable(lua);
	//��ֵ��1
	lua_pushstring(lua, "x");
	lua_pushinteger(lua, 0);
	lua_settable(lua, -3);//key=x value=0��λ����-3

	//��ֵ��2
	lua_pushinteger(lua, 11);
	lua_setfield(lua, -2, "y");//key=y value=11��λ����-2

	lua_setglobal(lua, "t2");
}

//��kv���͵�table
void ReadKVTable(lua_State* lua)
{
	lua_getglobal(lua, "t2");
	//��ȡ��1
	lua_getfield(lua, -1, "x");//��keyΪx��value����ջ��
	printf("c said: x is %d\n", lua_tointeger(lua,-1));

	//��ȡ��2
	lua_pushstring(lua, "y");
	lua_gettable(lua, -3);
	printf("c said: y is %d\n", lua_tointeger(lua, -1));
}