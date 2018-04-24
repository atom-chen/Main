#pragma  once
#include "ggl.h"

//v��ͷ �������� vt=texcoord vn=normal 
//f��ͷ ����ָ��-��face��ĳ��������Щ�����
//���ڴ洢v����
struct VertexData
{
	float position[3];//λ����Ϣ
	float nromal[3];//������Ϣ
	float texcoord[2];//uv��Ϣ
};

class Model
{
public:
	Model();
	bool Init(const char* path);
	void Draw();
public:
	void SetTexture(const char* bmpPath);
protected:
private:
	VertexData *m_Vertexes;//��������
	unsigned short *m_Indexes;//������������
	int32_t m_IndexCount;//��������
	GLuint m_Texture;//��ģ�͵�����
};