#include "RenderList.h"
#include "SceneManager.h"


void RenderList::Draw()
{
	if (m_pMainCamera == nullptr)
	{
		return;
	}
	for (int i = 0; i < m_RendList.size(); i++)
	{
		RenderAble* render = m_RendList[i];
		if (render->OnEnable())
		{
			render->GetVertexBuffer().Begin();
			{
				render->GetShader().Begin();
				glm::mat4 ITMatrix = glm::inverseTranspose(render->GetModelMatrix());
				render->GetShader().Bind(glm::value_ptr(render->GetModelMatrix()), glm::value_ptr(m_pMainCamera->GetViewMatrix()), glm::value_ptr(m_pMainCamera->GetProjectionMatrix())
					, glm::value_ptr(ITMatrix));
				{
					SceneManager::SetBlendState(render->GetAlphaBlend());
					SceneManager::SetDepthTestState(render->IsDepthTest());
					SceneManager::SetProgramPointSizeState(render->IsProgramPointSize());
					glDrawArrays(render->GetType(), 0, render->GetVertexBuffer().GetLenth());
				}
				render->GetShader().End();
			}
			render->GetVertexBuffer().End();
		}
	}
	m_RendList.clear();//渲染完了进行清空
}
void RenderList::Clip()
{

}
void RenderList::Cull()
{

}
void RenderList::InsertToRenderList(RenderAble* render)
{
	m_RendList.push_back(render);
}