#pragma  once

#include "ggl.h"
class SkyBox
{
public:
	bool Init(const char* bmpPath);//�ṩ6�����bitmap��ַ����ʼ����պ�
	void Draw();//������պ�
	GLuint* GetTexture();
protected:
private:
	GLuint m_Texture[6];//6����
};