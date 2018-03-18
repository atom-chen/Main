#pragma  once

#include "ggl.h"
class SkyBox
{
public:
	bool Init(const char* bmpPath);//提供6个面的bitmap地址，初始化天空盒
	void Draw();//绘制天空盒
	GLuint* GetTexture();
protected:
private:
	GLuint m_Texture[6];//6个面
};