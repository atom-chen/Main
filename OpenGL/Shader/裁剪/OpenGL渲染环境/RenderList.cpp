#include "RenderList.h"
#include "SceneManager.h"


void RenderList::Draw()
{
	if (m_pMainCamera == nullptr)
	{
		return;
	}
	//����domain
	for (int i = 0; i < m_DomainRenderList.size(); i++)
	{
		RenderDomain render = m_DomainRenderList[i];
			render.vertexBuf.Begin();
			{
				render.shader.Begin();
				glm::mat4 ITMatrix = glm::inverseTranspose(render.modelMatrix);
				render.shader.Bind(glm::value_ptr(render.modelMatrix), glm::value_ptr(m_pMainCamera->GetViewMatrix()), glm::value_ptr(m_pMainCamera->GetProjectionMatrix())
					, glm::value_ptr(ITMatrix));
				{
					SceneManager::SetBlendState(render.options.alphaBlend);
					SceneManager::SetDepthTestState(render.options.DepthTest);
					SceneManager::SetProgramPointSizeState(render.options.Program_Point_Size);
					glDrawArrays(render.options.DrawType, 0, render.vertexBuf.GetLenth());
				}
				render.shader.End();
			}
			render.vertexBuf.End();
		}
	m_DomainRenderList.clear();

	//����RenderList
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
					SceneManager::SetProgramPointSizeState(render->GetProgramPointSize());
					glDrawArrays(render->GetType(), 0, render->GetVertexBuffer().GetLenth());
				}
				render->GetShader().End();
			}
			render->GetVertexBuffer().End();
		}
	}
	m_RendList.clear();//��Ⱦ���˽������
}


void RenderList::Clip()
{
	SceneManager::SetScissorState(ScissorState(1, m_pMainCamera->GetXStart(), m_pMainCamera->GetYStart(), m_pMainCamera->GetWidth(), m_pMainCamera->GetHeight()));
}
void RenderList::Cull()
{

}
void RenderList::InsertToRenderList(RenderAble* render)
{
	m_RendList.push_back(render);
}
void RenderList::InsertToRenderList(const RenderDomain& render)
{
	m_DomainRenderList.push_back(render);
}