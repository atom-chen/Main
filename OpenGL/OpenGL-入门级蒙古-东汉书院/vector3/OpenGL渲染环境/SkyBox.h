#pragma  once

#include "ggl.h"
class SkyBox
{
public:
	bool Init(const char* bmpPath);//�ṩ6�����bitmap��ַ����ʼ����պ�
	void DrawCommon();//����ָ��
	void Draw();//������պ�
protected:
private:
	GLuint m_Texture[6];//6����
	GLint m_FastDrawCall;//��ʾ�б�
};