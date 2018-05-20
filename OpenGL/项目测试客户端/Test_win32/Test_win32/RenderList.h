#pragma once
#include "Camera.h"
#include "RenderAble.h"

class RenderDomain
{
public:
	VertexBuffer vertexBuf;
	Shader shader;
	mat4 modelMatrix;
	RenderOption options;
	RenderDomain(const VertexBuffer& vertexBuffer, const Shader& shader, const mat4& modelMatrix)
	{
		this->vertexBuf = vertexBuffer;
		this->shader = shader;
		this->modelMatrix = modelMatrix;
	}
	RenderDomain(const RenderDomain& other)
	{
		this->vertexBuf = other.vertexBuf;
		this->shader = other.shader;
		this->modelMatrix = other.modelMatrix;
		this->options = other.options;
	}
	RenderDomain& operator=(const RenderDomain& other)
	{
		this->vertexBuf = other.vertexBuf;
		this->shader = other.shader;
		this->modelMatrix = other.modelMatrix;
		this->options = other.options;
	}
};
class RenderList
{
public:
	void Draw();//绘制渲染列表中的全部顶点数据
	void Clip();//裁剪
	void Cull();//剔除
	void SetCamera(Camera_1st* camera){ this->m_pMainCamera = camera; }
public:
	void InsertToRenderList(RenderAble* render);
	void InsertToRenderList(const RenderDomain& render);
protected:
private:
	Camera_1st *m_pMainCamera;//此渲染列表所用的摄像机
	std::vector<RenderAble *> m_RendList;//SDK无关层输入的顶点数据会进到这里
	std::vector<RenderDomain> m_DomainRenderList;//特殊的输入
};