#pragma  once
#include "ggl.h"
#include "RenderList.h"
#include "RenderAble.h"

//利用FBO进行绘制
class Camera_2D
{
public:
	Camera_2D();
	void Draw();
	inline mat4 GetViewMatrix() const{ return this->viewMatrix; };
	inline mat4 GetProjectMatrix() const{ return this->ProjectionMatrix; };
public:
	void InsertToRenderList(RenderAble* render);
	void InsertToRenderList(const RenderDomain& render);
protected:
private:
	mat4 viewMatrix;
	mat4 ProjectionMatrix;

	float m_ViewportWidget = 800;//视口大小
	float m_ViewportHeight = 600;//视口大小
	float m_ViewportXStart = 0;
	float m_ViewportYStart = 0;
	RenderList m_RenderList;
};