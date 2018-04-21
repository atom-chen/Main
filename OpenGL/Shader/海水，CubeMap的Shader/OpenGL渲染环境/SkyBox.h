#pragma  once
#include "ggl.h"
#include "Shader.h"
#include "VertexBuffer.h"
class SkyBox
{
public:
	SkyBox();
public:
	bool Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath);//�ṩ6�����bitmap��ַ����ʼ����պ�
	void SetForward(const char* picPath);
	void SetBack(const char* picPath);
	void SetTop(const char* picPath);
	void SetBottom(const char* picPath);
	void SetRight(const char* picPath);
	void SetLeft(const char* picPath);

	void Update(const vec3& cameraPos);
	void Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix);//������պ�
protected:
private:
	VertexBuffer m_VertexBuf[6];//6����,˳��ǰ����������
	Shader m_Shader[6];
	glm::mat4 m_ModelMatrix;
	bool m_IsInit = 0;
};