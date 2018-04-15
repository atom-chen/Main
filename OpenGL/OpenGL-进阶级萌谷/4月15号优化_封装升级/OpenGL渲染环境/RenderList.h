#pragma once
#include "Camera.h"
#include "RenderAble.h"


class RenderList
{
public:
	void Draw();//������Ⱦ�б��е�ȫ����������
	void Clip();//�ü�
	void Cull();//�޳�
	void SetCamera(Camera_1st* camera){ this->m_pMainCamera = camera; }
public:
	void InsertToRenderList(RenderAble* render);
protected:
private:
	Camera_1st *m_pMainCamera;//����Ⱦ�б����õ������
	std::vector<RenderAble *> m_RendList;//SDK�޹ز�����Ķ������ݻ��������
};