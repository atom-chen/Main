#pragma  once
#include "ggl.h"
#include "FrameBuffer.h"
#include "Shader.h"
#include "VertexBuffer.h"

//利用FBO进行绘制
class Camera_2D
{
public:
	Camera_2D();
	void Init();
	inline mat4 GetViewMatrix() const{ return this->viewMatrix; };
	inline mat4 GetProjectMatrix() const{ return this->ProjectionMatrix; };
protected:
private:
	mat4 MODELMATRIX_NAME;
	mat4 viewMatrix;
	mat4 ProjectionMatrix;
};