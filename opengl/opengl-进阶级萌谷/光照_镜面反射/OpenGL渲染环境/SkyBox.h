#pragma  once

#include "ggl.h"
#include "Vector3.h"
class SkyBox
{
public:
	bool Init(const char* bmpPath);//�ṩ6�����bitmap��ַ����ʼ����պ�
	void DrawCommon();//����ָ��
	void Draw(const Vector3& cameraPos);//������պ�
protected:
private:
	GLuint m_Texture[6];//6����
	GLint m_FastDrawCall;//��ʾ�б�
};