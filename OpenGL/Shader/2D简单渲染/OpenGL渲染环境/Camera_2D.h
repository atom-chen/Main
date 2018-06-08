#pragma  once
#include "ggl.h"
#include "RenderList.h"
#include "RenderAble.h"

//����FBO���л���
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

	float m_ViewportWidget = 800;//�ӿڴ�С
	float m_ViewportHeight = 600;//�ӿڴ�С
	float m_ViewportXStart = 0;
	float m_ViewportYStart = 0;
	RenderList m_RenderList;
};