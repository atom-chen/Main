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
					SceneManager::SetBlendState(render->IsAlphaBlend());
					SceneManager::SetDepthTestState(render->IsDepthTest());
					SceneManager::SetProgramPointSizeState(render->IsProgramPointSize());
					switch (render->GetType())
					{
					case DRAW_POINT:
						glDrawArrays(GL_POINTS, 0, render->GetVertexBuffer().GetLenth());
						break;
					case DRAW_TRIANGLES:
						glDrawArrays(GL_TRIANGLES, 0, render->GetVertexBuffer().GetLenth());
						break;
					case DRAW_QUADS:
						glDrawArrays(GL_QUADS, 0, render->GetVertexBuffer().GetLenth());
						break;
					case DRAW_TRIANGLES_STRIP:
						glDrawArrays(GL_TRIANGLE_STRIP, 0, render->GetVertexBuffer().GetLenth());
						break;
					}
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