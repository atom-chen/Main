#pragma  once
#include "ggl.h"
#include "Vertex.h"
#include "Shader.h"

//利用FBO进行绘制
class Camera_2D
{
public:
	Camera_2D();
	void Init(FrameBuffer &fbo, const char* bufferName = "2DUI");
	void Destory();
	void Draw();
	inline mat4 GetViewMatrix() const{ return this->viewMatrix; };
	inline mat4 GetProjectMatrix() const{ return this->ProjectionMatrix; };

protected:
private:
	void SetPosition(float x, float y, float width, float height);
	VertexBuffer VBO_NAME;
	Shader SHADER_NAME;
	mat4 MODELMATRIX_NAME;
	mat4 viewMatrix;
	mat4 ProjectionMatrix;

	GLuint m_Texture;
};