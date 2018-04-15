#pragma once
#include "ggl.h"
#include "Camera.h"
#include "RenderAble.h"


class RenderList
{
public:
	void Draw();//绘制渲染列表中的全部顶点数据
	void Clip();//裁剪
	void SetCamera(Camera_1st* camera){ this->m_pMainCamera = camera; }
public:
	void InsertToRenderList(RenderAble* render);
protected:
private:
	Camera_1st *m_pMainCamera;//此渲染列表所用的摄像机
	std::vector<RenderAble *> m_RendList;//SDK无关层输入的顶点数据会进到这里
};