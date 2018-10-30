#pragma  once
#include "ggl.h"
#include "Shader.h"
#include "VertexBuffer.h"
class SkyBox
{
public:
	SkyBox();
public:
	bool Init(const char* forwardPath, const char* backPath, const char* topPath, const char* bottomPath, const char* leftPath, const char* rightPath);//提供6个面的bitmap地址，初始化天空盒
	void SetForward(const char* picPath);
	void SetBack(const char* picPath);
	void SetTop(const char* picPath);
	void SetBottom(const char* picPath);
	void SetRight(const char* picPath);
	void SetLeft(const char* picPath);

	void Update(const vec3& cameraPos);
	void Draw();//绘制天空盒
	void Destroy();
protected:
private:
	VertexBuffer m_VertexBuf[6];//6个面,顺序前后上下左右
	Shader m_Shader[6];
	glm::mat4 m_ModelMatrix;
	bool m_IsInit = 0;
};