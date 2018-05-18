#pragma  once
#include "ggl.h"
#include "Vector3.h"

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
	void Update(float frameTime);
	void Draw();
public:
	void SetTexture(const char* bmpPath);
	void SetAmbientMaterialColor(float r, float g, float b, float a = 1);
	void SetDiffuseMaterialColor(float r, float g, float b, float a = 1);
	void SetSpecularMaterialColor(float r, float g, float b, float a = 1);

	void MoveToLeft(bool isMove);
	void MoveToRight(bool isMove);
	void MoveToTop(bool isMove);
	void MoveToBottom(bool isMove);
	void SetMoveSpeed(float speed);
protected:
private:
	VertexData *m_Vertexes;//��������
	unsigned short *m_Indexes;//������������
	int32_t m_IndexCount;//��������
	GLuint m_Texture;//��ģ�͵�����

	//ģ�͵Ĳ���
	float m_AmbientMaterial[4], m_DiffuseMaterial[4], m_SpecularMaterial[4];

	bool m_IsMoveToLeft, m_IsMoveToRight, m_IsMoveToTop, m_IsMoveToBottom;
	float m_MoveSpeed = 5;
	Vector3 m_Position{0,0,-5};
};