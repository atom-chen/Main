#include "Camera_2D.h"
#include "Utils.h"
#include "Resource.h"

Camera_2D::Camera_2D()
{
	ProjectionMatrix = glm::ortho(-m_ViewportWidget / 2, m_ViewportWidget / 2, -m_ViewportHeight / 2, m_ViewportHeight / 2);
	viewMatrix = glm::lookAt(vec3(0, 0, 2), vec3(0, 0, -100), vec3(0, 1, 0));
}


void Camera_2D::Draw()
{
	m_RenderList.Clip(m_ViewportXStart, m_ViewportYStart, m_ViewportWidget, m_ViewportHeight);
	m_RenderList.Draw(viewMatrix, ProjectionMatrix);
}
void Camera_2D::InsertToRenderList(RenderAble* render)
{
	m_RenderList.InsertToRenderList(render);
}
void Camera_2D::InsertToRenderList(const RenderDomain& render)
{
	m_RenderList.InsertToRenderList(render);
}
