#pragma  once

#include "ggl.h"
#include "Vector3.h"
class SkyBox
{
public:
	bool Init(const char* bmpPath);//提供6个面的bitmap地址，初始化天空盒
	void DrawCommon();//绘制指令
	void Draw(const Vector3& cameraPos);//绘制天空盒
protected:
private:
	GLuint m_Texture[6];//6个面
	GLint m_FastDrawCall;//显示列表
};