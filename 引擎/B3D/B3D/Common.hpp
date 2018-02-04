#pragma once
#include "B3DHead.h"


/************************************************************************/
/* 功能：定义引擎的全局常量以及函数
   作者：
                                                                   */
/************************************************************************/
namespace B3D
{
	//窗口大小
	int WINDOWS_WIDTH = 800;
	int WINDOWS_HEIGHT = 600;
	
	//默认名称
	const string Default_Name = "Default";
	const string DefaultPath = "Media/";
	
	const float PI = 3.1415926535f;
	const float PI2 = 2 * PI;
	const float PI_DIV_2 = PI*0.5f;
	const float PI_DEV4 = PI*0.25f;
	const float PI_INV = 0.318309886f;

	inline string GetPath(const string& FileURI)        //拿到文件URL
	{
		return DefaultPath + FileURI;
	}

#define KEY_DOWN(vk_code)((GetAsyncKeyState(vk_code)&0x8000)?1:0);
#define KEY_UP(vk_code)((GetAsyncKeyState(vk_code)&0x8000)?0:1);

#define COLOR_16BIT(r,g,b)(((r)&0xff<<16)| ((g)&0xff)<<8) |(b)&0xff)










}